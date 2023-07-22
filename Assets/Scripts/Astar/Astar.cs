using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public enum SearchType
{
    CheckBlock,
    CheckOtherRoom
}
public class Astar
{
    private delegate void FriendDelegate(ref Vector2Int FriendPos, List<TileNode> Nodes);
    private Qlist Openlist = new Qlist(), Closelist = new Qlist();
    //Dictionary<Vector3Int, TileNode> alltiles;
    List<TileNode> FriendNodes = new List<TileNode>(8);
    //private float OriginalCost=0, PastCost=0, EndTileDirCost=0;
    private int TilemapSize = 0;
    private int TileSize = 1;
    TileNode EndTile = null, MainTile1 = null, SercingTile1 = null;
    //openlist = 탐색해야할 타일
    //closelist = 탐색한 타일
    float CostSum = 0;
    List<TileNode> EditRootList = new List<TileNode>(8);
    MapManager MapManager;
    public Astar(MapManager Map, int TileSize, int TilemapSize)
    {
        MapManager = Map;
        this.TileSize = TileSize;
        this.TilemapSize = TilemapSize;
    }
    //todo 대각선 검색 기능 삭제
    public List<TileNode> getRoomPath(Vector2Int Startpos, Vector2Int RoomPos)
    {
        Room EndRoom = MapManager.RoomList[RoomPos];
        List<TileNode> PathList = new List<TileNode>();
        if (EndRoom.overlap(Startpos,TileSize))
        {
            if (PathList.Count < 1)
            {
                PathList.Add(MapManager.WorldMap[Startpos]);
                return PathList;
            }
        }
        //타일맵 x * y
        Debug.Log(EndRoom.size);
        EndTile = MapManager.WorldMap[EndRoom.Nodes[new Vector2Int((int)EndRoom.size.x/2,(int)EndRoom.size.y/2)].intposition];
        QlistClear();
        MainTile1 = MapManager.WorldMap[Startpos];
        MainTile1.DirCost = CostSerch(MainTile1.intposition, EndTile.intposition);
        MainTile1.previous = null;

        ///인접 타일
        Openlist.push(MainTile1);

        while (Openlist.Count > 0)
        {
            MainTile1 = Openlist.First;
            if (!Closelist.Contains(MainTile1))
            {
                MainTile1.close = true;
                Closelist.push(MainTile1);

            }
            FriendNodes.Clear();
            FindNextTile(MainTile1, FriendNodes, (ref Vector2Int FriendPos, List<TileNode> nextNodes) =>
            {
                TileNode node;
                if (MapManager.WorldMap.ContainsKey(FriendPos))
                {
                    node = MapManager.WorldMap[FriendPos];
                }
                else
                {
                    node = new TileNode(FriendPos, TileType.HallWay);
                    MapManager.AddTile(node.intposition,node);
                }
                if (TileType.Room != node.type || EndRoom.overlap(node.mypos, TileSize))
                {
                    if (!Closelist.Contains(node))
                    {
                        node.OriginalCost = 10;
                        nextNodes.Add(node);
                    }
                }
            });

            for (int i = 0; i < FriendNodes.Count; i++)
            {
                SercingTile1 = FriendNodes[i];
                if (EndRoom.overlap(SercingTile1.mypos,TileSize))
                {
                    SercingTile1.previous = MainTile1;
                    PathSort(SercingTile1, PathList);
                    Openlist.Clear();
                    return PathList;
                }
                CostSum = SercingTile1.PastCost + SercingTile1.OriginalCost;
                if (!Openlist.Contains(SercingTile1))
                {
                    //print("보고 : " + MainTile1.mypos + "탐색대상 : " + SercingTile1.mypos);
                    SercingTile1.previous = MainTile1;
                    //SercingTile1.OriginalCost = CostTileSerch(MainTile1.mypos, SercingTile1.mypos);
                    SercingTile1.DirCost = CostSerch(SercingTile1.intposition, EndTile.intposition);
                    SercingTile1.Cost = SercingTile1.OriginalCost + SercingTile1.DirCost;
                    SercingTile1.PastCost = MainTile1.PastCost + SercingTile1.OriginalCost;
                    SercingTile1.TotalCost = MainTile1.PastCost + SercingTile1.Cost;
                    Openlist.push(SercingTile1);
                }
                else if (SercingTile1.previous != null)
                {
                    if ((SercingTile1.PastCost + CostTileSerch(SercingTile1.intposition, MainTile1.intposition)) < (MainTile1.PastCost))
                    {
                        //print("보고 : " + MainTile1.mypos + "탐색대상 : " + SercingTile1.mypos + "경로 수정 \n" + MainTile1.PastCost + " -> " + (SercingTile1.PastCost + SercingTile1.OriginalCost));
                        MainTile1.previous = SercingTile1;
                        MainTile1.OriginalCost = CostTileSerch(SercingTile1.intposition, MainTile1.intposition); //SercingTile1.OriginalCost와 같을 꺼임.
                        MainTile1.DirCost = CostSerch(MainTile1.intposition, EndTile.intposition);
                        MainTile1.Cost = MainTile1.OriginalCost + MainTile1.DirCost;
                        MainTile1.PastCost = SercingTile1.PastCost + MainTile1.OriginalCost;
                        MainTile1.TotalCost = SercingTile1.PastCost + MainTile1.Cost;
                        Openlist.sort();
                    }
                    else if (SercingTile1.PastCost > (MainTile1.PastCost + CostTileSerch(MainTile1.intposition, SercingTile1.intposition)))
                    {
                        //print("보고 : " + MainTile1.mypos + "탐색대상 : " + SercingTile1.mypos + "경로 이상 \n" + MainTile1.PastCost + " /  " + (SercingTile1.PastCost + SercingTile1.OriginalCost));
                        SercingTile1.previous = MainTile1;
                        SercingTile1.DirCost = CostSerch(SercingTile1.intposition, EndTile.intposition);
                        SercingTile1.Cost = SercingTile1.OriginalCost + SercingTile1.DirCost;
                        SercingTile1.PastCost = MainTile1.PastCost + SercingTile1.OriginalCost;
                        SercingTile1.TotalCost = MainTile1.PastCost + SercingTile1.Cost;
                        Openlist.sort();
                    }
                }
            }

        }
        return null;
    }

    //todo remove tilemap size
    private void PathSort(TileNode tile, List<TileNode> pathlistResult)
    {
        TileNode PathTile = tile;
        pathlistResult.Clear();

        for (int i = 0; i < TilemapSize; i++)
        {
            pathlistResult.Add(PathTile);
            if (PathTile.previous == null)
            {
                break;

            }
            PathTile = PathTile.previous;
            

        }
        pathlistResult.Reverse();
        if (pathlistResult.Count == 1)
        {
            Debug.LogWarning("이미 목적지 도착");
        }
        else if (pathlistResult.Count == TilemapSize)
        {
            Debug.LogError("길이 반복된다");
        }
    }

    
    /// <summary>
    /// Block타입은 걸러짐
    /// </summary>
    private void FindNextTile(TileNode SerchingStandTile, List<TileNode> Nextnodes,FriendDelegate action)
    {
        Nextnodes.Clear();
        Vector2Int FriendPos = new Vector2Int();
        Vector2Int SerchingStandTilePos = SerchingStandTile.intposition;
        int xvalue,yvalue;
        for (int index = -1; index < 2; index+=2)
        {
            xvalue = index*TileSize;
            yvalue = 0;
            for (int subindex = 0; subindex < 2; subindex++)
            {
                FriendPos.x = SerchingStandTilePos.x + xvalue;
                FriendPos.y = SerchingStandTilePos.y + yvalue;
                action(ref FriendPos, Nextnodes);
                xvalue = 0;
                yvalue = index*TileSize;
            }
        }
    }
    public void CheckNextTileBlock(ref Vector2Int FriendPos, List<TileNode> nextNodes)
    {
        if (MapManager.WorldMap.ContainsKey(FriendPos))
        {
            TileNode node = MapManager.WorldMap[FriendPos];
            if (TileType.Block != node.type)
            {
                if (!Closelist.Contains(node))
                {
                    node.OriginalCost = 10;
                    nextNodes.Add(node);
                }
            }
        }
    }

    private void QlistClear()
    {
        Openlist.Clear();
        Closelist.Clear();
    }
    private int CostSerch(Vector2Int OriginalTile, Vector2Int NextTile)
    {
        Vector2Int Serchcost2 = OriginalTile - NextTile;
        Serchcost2.x = Mathf.Abs(Serchcost2.x);
        Serchcost2.y = Mathf.Abs(Serchcost2.y);
        int result;
        if (Serchcost2.x < Serchcost2.y)
        {
            result = ((Serchcost2.y - Serchcost2.x) * 10) + (Serchcost2.x * 14);
            return result;
        }
        else
        {
            result = ((Serchcost2.x - Serchcost2.y) * 10) + (Serchcost2.y * 14);
            return result;
        }
    }
    private int CostTileSerch(Vector2Int OriginalTile, Vector2Int NextTile)
    {
        Vector2Int Serchcost = OriginalTile - NextTile;
        if (Serchcost.x == 0 || Serchcost.y == 0)
        {

            return 10;
        }
        else
        {
            return 14;
        }
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2Int position;
    //if size odd, Each edge lacks one tile.
    // it always looks like int. but when calculation, it's float
    // example is divide calculation in min, max 
    /// <summary>
    /// Size Must be Even.(2%n==0)
    /// </summary>
    public Vector2 size;
    /// <summary>
    /// find node index
    /// ((size.y-1)*(findX))+findY
    /// </summary>
    public List<GameObject> gameObjects;
    /// <summary>
    /// min index is 1<br/>
    /// max index is size
    /// </summary>
    public Dictionary<Vector2Int, TileNode> Nodes;
    public int x
    {
        get { return position.x; }
        set { position.x = value; }
    }
    public int y
    {
        get { return position.y; }
        set { position.y = value; }
    }
    private Vector2 MinTilePos;
    private Vector2 MaxTilePos;
    public Vector2 minTilePos
    {
        get { return MinTilePos; }
        //return position - (size * 0.5f * tilesize);
    }
    public Vector2 maxTilePos
    {
        get{ return MaxTilePos; }
        //return position + (size * 0.5f * tilesize);
    }
    public Room()
    {

    }
    public Room(Vector2Int pos,Vector2 Size)
    {
        position= pos;
        size = Size;
        Nodes = new Dictionary<Vector2Int, TileNode>();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2 localPosition = new Vector2((x + 0.5f - (0.5f * size.x)) * DungeonGenerator.mytilesize, (y + 0.5f - (0.5f * size.y)) * DungeonGenerator.mytilesize);
                TileNode node = new TileNode(localPosition+position,TileType.Room);
                node.localpos = new Vector2Int((int)localPosition.x,(int)localPosition.y);
                Nodes.Add(new Vector2Int(x+1, y+1), node);
            }
        }
        MinTilePos = position - (size * 0.5f * DungeonGenerator.mytilesize);
        MaxTilePos = position + (size * 0.5f * DungeonGenerator.mytilesize);
    }
    public bool overlapBolder(Room room, int tilesize)
    {
        Vector2 myMax = MaxTilePos;
        Vector2 myMin = MinTilePos;
        Vector2 RoomMax = room.MaxTilePos;
        Vector2 RoomMin = room.MinTilePos;

        //unity Vector2 +,-,*,/(and other) operator return new vector
        myMax.x += tilesize;
        myMax.y += tilesize;

        myMin.x -= tilesize;
        myMin.y -= tilesize;

        RoomMax.x += tilesize;
        RoomMax.y += tilesize;

        RoomMin.x -= tilesize;
        RoomMin.y -= tilesize;
        if (myMax.x > RoomMin.x && RoomMax.x > myMin.x && myMax.y > RoomMin.y && RoomMax.y > myMin.y)
        {
            return true;
        }
        return false;

    }
    public bool overlap(Room room)
    {
        Vector2 myMax = MaxTilePos;
        Vector2 myMin = MinTilePos;
        Vector2 RoomMax = room.MaxTilePos;
        Vector2 RoomMin = room.MinTilePos;
        if (myMax.x > RoomMin.x&& RoomMax.x > myMin.x && myMax.y > RoomMin.y && RoomMax.y> myMin.y)
        {
            return true;
        }
        return false;
    }
    public bool overlap(Vector2 position)
    {
        Vector2 myMax = MaxTilePos;
        Vector2 myMin = MinTilePos;
        if (myMax.x > position.x && position.x > myMin.x && myMax.y > position.y && position.y > myMin.y)
        {
            return true;
        }
        return false;
    }
    public void Move(Vector2 move, int tilesize)
    {
        move.x = move.x * tilesize;
        move.y = move.y * tilesize;
        position.x += (int)move.x;
        position.y += (int)move.y;
        resetNodesPosition();
        gameObjects[0].transform.position = new Vector3(position.x, position.y);
        MinTilePos = position - (size * 0.5f * tilesize);
        MaxTilePos = position + (size * 0.5f * tilesize);
    }
    /// <summary>
    /// 실제 위치 변경없이 데이터만 바꾼다.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="tilesize"></param>
    public void TempMove(int x, int y, int tilesize)
    {
        position.x += x * tilesize;
        position.y += y * tilesize;
        MinTilePos.x = position.x - (size.x * 0.5f * tilesize);
        MinTilePos.y = position.y - (size.y * 0.5f * tilesize);
        MaxTilePos.x = position.x + (size.x * 0.5f * tilesize);
        MaxTilePos.y = position.y + (size.y * 0.5f * tilesize);
    }
    public void complateTempMove()
    {
        gameObjects[0].transform.position = new Vector3(position.x, position.y);
        resetNodesPosition();
        
    }
    private void resetNodesPosition()
    {
        foreach(TileNode tilenode in Nodes.Values)
        {
            tilenode.mypos = tilenode.localpos + position;
        }
    }
    public Vector2 getDirection(Room TargetRoom)
    {
        Vector2Int dir = TargetRoom.position - position;
        return ((Vector2)dir).normalized;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.FullSerializer;
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
    public List<GameObject> gameObjects;
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
    public Vector2 min(int tilesize)
    {
        return position - (size * 0.5f * tilesize);
    }
    public Vector2 max(int tilesize)
    {
        return position + (size * 0.5f * tilesize);
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
                TileNode node = new TileNode((int)((x + 0.5 - (0.5 * size.x))*DungeonGenerator.mytilesize),(int)((y + 0.5 - (0.5 * size.y))*DungeonGenerator.mytilesize), TileType.Room);
                Nodes.Add(new Vector2Int(x, y), node);
            }
        }

    }
    public bool overlap(Room room, int tilesize)
    {
        Vector2 myMax = max(tilesize);
        Vector2 myMin = min(tilesize);
        Vector2 RoomMax = room.max(tilesize);
        Vector2 RoomMin = room.min(tilesize);
        
        if (myMax.x > RoomMin.x&& RoomMax.x > myMin.x && myMax.y > RoomMin.y && RoomMax.y> myMin.y)
        {
            return true;
        }
        return false;
    }
    public void Move(Vector2 move, int tilesize)
    {
        move *= tilesize;
        position +=new Vector2Int((int)move.x,(int)move.y);
        gameObjects[0].transform.position = new Vector3(position.x,position.y);
    }
    /// <summary>
    /// x축에서 호출한 인스턴스와 인자를 비교하여 호출한 인스턴스 기준으로 인자가 상대적으로 어떤 위치에 있는지 반환한다.<br/>
    /// 정확히 겹칠 경우 1을 반환한다.
    /// </summary>
    /// <param name="TargetRoom">첫 인자와 위치를 비교할 대상</param>
    /// <returns>1 : 오른쪽 <br/>-1 : 왼쪽</returns>
    public int GetRoomRelativePosX(Room TargetRoom, int tilesize)
    {
        if (min(tilesize).x > TargetRoom.max(tilesize).x)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
    /// <summary>
    /// y축에서 호출한 인스턴스와 인자를 비교하여 호출한 인스턴스 기준으로 인자가 상대적으로 어떤 위치에 있는지 반환한다.<br/>
    /// 정확히 겹칠 경우 1을 반환한다.
    /// </summary>
    /// <param name="TargetRoom">첫 인자와 위치를 비교할 대상</param>
    /// <returns>1 : 위쪽 <br/>-1 : 아레쪽</returns>
    public int GetRoomRelativePosY(Room TargetRoom, int tilesize)
    {
        if (min(tilesize).y > TargetRoom.max(tilesize).y)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}

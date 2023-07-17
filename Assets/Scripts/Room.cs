using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Room
{
    public Vector2Int position;
    /// <summary>
    /// it always looks like int. but when calculation, it's float
    /// example divide in min, max 
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
        return position - ((size / 2) * tilesize);
    }
    public Vector2 max(int tilesize)
    {
        return position + ((size / 2) * tilesize);
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
        position+=new Vector2Int((int)move.x,(int)move.y);
        gameObjects[0].transform.position = new Vector3(position.x,position.y);
    }

}

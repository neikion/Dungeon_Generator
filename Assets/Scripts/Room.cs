using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Room
{
    public Vector2 position;
    public Vector2 size;
    public List<GameObject> gameObjects;
    public float x
    {
        get { return position.x; }
        set { position.x = value; }
    }
    public float y
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
    public Room(Vector2 pos)
    {
        position= pos;
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
        position+=move*tilesize;
        gameObjects[0].transform.position = position;
    }

}

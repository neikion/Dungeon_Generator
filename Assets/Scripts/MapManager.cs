using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager
{
    public Dictionary<Vector2Int,TileNode> WorldMap;
    /// <summary>
    /// Room is Dictionary<Vecter2Int,TileNode> Room
    /// </summary>
    public Dictionary<Vector2,Room> RoomList;
    public MapManager()
    {
        WorldMap = new Dictionary<Vector2Int, TileNode>();
        RoomList=new Dictionary<Vector2,Room>();
    }
    public void AddRoomtoWorld(Room room)
    {
        RoomList.Add(room.position, room);
        foreach(TileNode tilenode in room.Nodes.Values)
        {
            WorldMap.Add(new Vector2Int(room.position.x+tilenode.mypos.x,room.position.y + tilenode.mypos.y),tilenode);
        }
    }
}

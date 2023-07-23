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
            AddTile(tilenode.intposition, tilenode);
        }
    }
    public bool AddTile(Vector2Int position, TileNode tile)
    {
        if (!WorldMap.ContainsKey(position))
        {
            WorldMap.Add(position, tile);
            return true;
        }
        return false;
    }
}

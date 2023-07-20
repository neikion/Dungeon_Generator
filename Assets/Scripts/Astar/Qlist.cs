using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qlist
{
    private List<TileNode> tilelist = new List<TileNode>();
    public void push(TileNode tile)
    {
        tilelist.Add(tile);
        tilelist.Sort();
    }
    public void remove(TileNode tile)
    {

        tilelist.Remove(tile);
        tilelist.Sort();

    }
    public TileNode pop(int index)
    {
        return tilelist[index];
    }
    public int Count
    {
        get
        {
            return tilelist.Count;
        }
    }
    public void Clear()
    {
        tilelist.Clear();
    }
    public TileNode First
    {
        get
        {
            tilelist.Sort();
            
            if (tilelist.Count > 0)
            {
                TileNode temptile = tilelist[0];
                tilelist.RemoveAt(0);
                tilelist.Sort();
                return temptile;
            }
            return null;
        }
    }
    public bool Contains(TileNode obj)
    {
        for(int i = 0; i < tilelist.Count; i++)
        {
            if (tilelist[i].mypos.Equals(obj.mypos))
            {
                return true;
            }
        }
        return false;

    }
    public void sort()
    {
        tilelist.Sort();
    }
}
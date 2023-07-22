using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    Ground,
    Block,
    HallWay,
    Room
}
public class TileNode : IComparable
{

    public TileType type;
    /// <summary>
    /// 전 타일에서 오는데 걸리는 코스트
    /// </summary>
    public int OriginalCost=0;
    /// <summary>
    /// 방향 코스트
    /// </summary>
    public int DirCost=0;
    /// <summary>
    /// 전 타일에서 오는데 걸리는 코스트 + 방향 코스트
    /// </summary>
    public int Cost = 0;
    /// <summary>
    /// 시작점에서 현재 노드까지 오는데 걸리는 코스트
    /// </summary>
    public int PastCost = 0;
    /// <summary>
    /// 총합 코스트
    /// </summary>
    public int TotalCost=0;
    private Vector2 Position;
    public Vector2 mypos
    {
        get { return Position; }
        set 
        { 
            Position = value;
            if (IntPosition != null)
            {
                IntPosition.x = (int)Position.x;
                IntPosition.y = (int)Position.y;
            }
            else
            {
                Position = new Vector2(value.x, value.y);
            }
        }
    }
    private Vector2Int IntPosition;
    public Vector2Int intposition
    {
        get { return IntPosition; }
    }
    public Vector2Int localpos;
    private TileNode previous1;

    public TileNode previous
    {
        set
        {
            previous1 = value;
            if (previous1 != null)
            {
                previousPos = value.mypos;
            }
        }
        get
        {
            return previous1;
        }
        
    }
    public float tilecost=0;
    public Vector2 previousPos;

    public bool close = false;

    public TileNode(Vector2 worldPosition, TileType type)
    {
        mypos = worldPosition;
        this.type = type;
        TilecostSerch();
        localpos = new Vector2Int(0, 0);
        previousPos=new Vector2();
    }
    private void TilecostSerch()
    {
        switch (type)
        {
            case TileType.Block: tilecost = 0;
                break;
            case TileType.Ground: tilecost = 0;
                break;
        }
    }
    public int CompareTo(object obj)
    {
        TileNode t2 = (TileNode)obj;
        if (TotalCost < t2.TotalCost)
        {
            return -1;
        }
        else if (TotalCost > t2.TotalCost)
        {
            return 1;
        }
        if (DirCost < t2.DirCost)
        {
            return -1;
        }
        else if (DirCost > t2.DirCost)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}

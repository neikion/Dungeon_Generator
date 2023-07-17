using System.Collections.Generic;
using UnityEngine;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using Edge = bowyer.Edge;
using bowyer;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    List<Room> pos = new List<Room>();
    public const int mytilesize = 2;
    [SerializeField]
    GameObject floorObject;


    [SerializeField]
    bool ShowLibFlag = false;
    [SerializeField]
    bool ShowMyLibFlag = false;

    [SerializeField]
    bool ShowPrimFlag = false;
    List<Edge> PrimEdge = new List<Edge>();
   
    [SerializeField]
    bool ShowPrimRmoveFlag = false;
    List<Edge> PrimRemoveEdge = new List<Edge>();

    [SerializeField]
    bool ShowPrimEditedFlag = false;
    List<Edge> PrimEditedEdge=new List<Edge>();

    //알고리즘 진행상황 확인 코드
    List<List<bowyer.Triangle>> ProcessLog = new List<List<bowyer.Triangle>>();
    [SerializeField]
    bool ShowProcess = false;
    float ProcessWaitTime = 1;
    int ProcessLogIndex = 0;

    Delaunator LibObj;
    bowyer.bowyer_watson bowyerLib = new bowyer.bowyer_watson();

    MapManager MapManager;
    // Start is called before the first frame update
    void Start()
    {

        MapManager = new MapManager();
        
        //library code
        //testRoomCreate(Vector2.one * 12, 4);
        createRandomCase(5);
        resetRoomPosition();
        spreateRoom();
        AddAllRoomToMapManager(ref pos,ref MapManager);
        //testMyLib();
        LibObj = setDelaunator();
        getEdge(LibObj, out List<Edge> edges);
        startGraphSetting(edges,out Dictionary<Vertex,GraphNode> nodeset);
        List<Edge> PrimResult = startPrim(ref nodeset,out List<Edge> removeEdge);
        PrimEdge = PrimResult;
        PrimRemoveEdge = removeEdge;
        List<Edge> ModifyEdge = AddRandomEdge(ref PrimEdge, ref removeEdge,out PrimEditedEdge);
    }
    void createRandomCase(int count)
    {
        Vector2 RandomPos;
        for (int i = 0; i < count; i++)
        {
            RandomPos = getRandomPointinCircle(1);
            Room room = new Room(Vector2Int.zero, Vector2Int.one * Random.Range(2,6));
            room.x = getPixelPoint(RandomPos.x, (int)(room.size.x * 2));
            room.y = getPixelPoint(RandomPos.y, (int)(room.size.y * 2));
            pos.Add(room);
        }
    }
    void resetRoomPosition()
    {
        for (int i = 0; i < pos.Count; i++)
        {
            pos[i].gameObjects = createRectangleRoom(pos[i].size, mytilesize);
            pos[i].gameObjects[0].transform.position = new Vector2(pos[i].position.x, pos[i].position.y);
        }
    }


    
    void testMyLib()
    {
        ShowMyLibFlag = true;
        for (int i = 0; i < pos.Count; i++)
        {
            bowyerLib.vertices.Add(new bowyer.Vertex(pos[i].x, pos[i].y));
        }
        bowyerLib.main((List<bowyer.Triangle> list) =>
        {
            ProcessLog.Add(new List<bowyer.Triangle>(list));
        });
    }
    Delaunator setDelaunator()
    {
        List<IPoint> pointlist = new List<IPoint>();
        for (int i = 0; i < pos.Count; i++)
        {
            pointlist.Add(new Point(pos[i].x, pos[i].y));
        }
        return new Delaunator(pointlist.ToArray());
    }
    List<Edge> startPrim(ref Dictionary<Vertex,GraphNode> nodeset,out List<Edge> RemoveEdge)
    {
        MyPrim prim=new MyPrim();
        prim.main(ref nodeset,out List<Edge> edges);
        RemoveEdge = prim.RemoveEdgeList;
        return edges;
    }
    void startGraphSetting(List<Edge> edges,out Dictionary<Vertex,GraphNode> nodelist)
    {
        MyDungeonGraph graph=new MyDungeonGraph();
        nodelist=graph.main(edges);
    }
    void getEdge(Delaunator delaunator,out List<Edge> edges)
    {
        edges = new List<Edge>();
        foreach (DelaunatorSharp.Edge edge in MyDelaunatorSharpExtension.getEdge(delaunator, out _))
        {
            edges.Add(new Edge(new Vertex(edge.P.X, edge.P.Y), new Vertex(edge.Q.X, edge.Q.Y)));
        }
    }
    void getEdge(Delaunator delaunator, out MyHeap<Edge> edges)
    {
        edges = new MyHeap<Edge>(Edge.CompareDistanceMin);
        foreach (DelaunatorSharp.Edge edge in MyDelaunatorSharpExtension.getEdge(delaunator, out _))
        {
            edges.Push(new Edge(new Vertex(edge.P.X, edge.P.Y), new Vertex(edge.Q.X, edge.Q.Y)));
        }
    }

    void testRoomCreate(Vector2Int mainpos, int space)
    {
        Room room;
        
        room = new Room(mainpos, Vector2Int.one * 2);
        pos.Add(room);
        
        room = new Room(mainpos + Vector2Int.up * space,Vector2Int.one*2);
        pos.Add(room);
        
        room = new Room(mainpos + Vector2Int.right * space, Vector2Int.one * 2);
        pos.Add(room);
        
        room = new Room(mainpos + Vector2Int.down * space, Vector2Int.one * 2);
        pos.Add(room);
        
        room = new Room(mainpos + Vector2Int.left * space, Vector2Int.one * 2);
        pos.Add(room);
        
        room = new Room(mainpos + Vector2Int.left * space * 2, Vector2Int.one * 2);
        pos.Add(room);
        /*
        room = new Room(mainpos + (Vector2.right * space) + (Vector2.down * space));
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        
        
        room = new Room(mainpos + Vector2.one * space);
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        
        room = new Room(mainpos - Vector2.one * space);
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        
        room = new Room(mainpos + (Vector2.left * space) + (Vector2.up * space));
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        */

        
    }
    
    private void Update()
    {
        if (ShowProcess)
        {
            ProcessWaitTime-=Time.deltaTime;
            if (ProcessWaitTime < 0)
            {
                ProcessWaitTime = 1;
                ProcessLogIndex = (ProcessLogIndex+1)%ProcessLog.Count;
            }
        }


    }
    public Vector2 Vertex2vector(Vertex vertex)
    {
        return new Vector2((float)vertex.x, (float)vertex.y);
    }
    public void OnDrawGizmos()
    {
        if (ShowMyLibFlag)
        {
            
            Gizmos.color = Color.white;
            if(bowyerLib.super!=null)
            {
                Gizmos.DrawLine(Vertex2vector(bowyerLib.super.v1), Vertex2vector(bowyerLib.super.v2));
                Gizmos.DrawLine(Vertex2vector(bowyerLib.super.v2), Vertex2vector(bowyerLib.super.v3));
                Gizmos.DrawLine(Vertex2vector(bowyerLib.super.v3), Vertex2vector(bowyerLib.super.v1));
            }
            if (bowyerLib.triangles.Count > 0)
            {
                foreach (bowyer.Triangle triangle in bowyerLib.triangles)
                {
                    Gizmos.DrawLine(Vertex2vector(triangle.v1), Vertex2vector(triangle.v2));
                    Gizmos.DrawLine(Vertex2vector(triangle.v2), Vertex2vector(triangle.v3));
                    Gizmos.DrawLine(Vertex2vector(triangle.v3), Vertex2vector(triangle.v1));
                }
            }
        }

        //library show
        if (ShowLibFlag)
        {
            Gizmos.color = Color.blue;
            LibObj.ForEachTriangle((edge) =>
            {
                for (int i = 0; i < edge.Points.ToVectors2().Length; i++)
                {
                    Gizmos.DrawLine(edge.Points.ToVectors2()[0], edge.Points.ToVectors2()[1]);
                    Gizmos.DrawLine(edge.Points.ToVectors2()[1], edge.Points.ToVectors2()[2]);
                    Gizmos.DrawLine(edge.Points.ToVectors2()[2], edge.Points.ToVectors2()[0]);

                }

            });
        }
        if (ShowProcess)
        {
            foreach (bowyer.Triangle triangle in ProcessLog[ProcessLogIndex])
            {
                Gizmos.DrawLine(Vertex2vector(triangle.v1), Vertex2vector(triangle.v2));
                Gizmos.DrawLine(Vertex2vector(triangle.v2), Vertex2vector(triangle.v3));
                Gizmos.DrawLine(Vertex2vector(triangle.v3), Vertex2vector(triangle.v1));
            }
        }
        if (ShowPrimFlag)
        {
            Gizmos.color = Color.green;
            foreach(Edge edge in PrimEdge)
            {
                Gizmos.DrawLine(Vertex2vector(edge.v1), Vertex2vector(edge.v2));
            }
        }
        if (ShowPrimRmoveFlag)
        {
            Gizmos.color = new Color32(255, 0, 0, 255);
            foreach (Edge edge in PrimRemoveEdge)
            {
                Gizmos.DrawLine(Vertex2vector(edge.v1), Vertex2vector(edge.v2));
            }
        }
        if (ShowPrimEditedFlag)
        {
            Gizmos.color = new Color32(160, 32, 240,255);
            foreach (Edge edge in PrimEditedEdge)
            {
                Gizmos.DrawLine(Vertex2vector(edge.v1), Vertex2vector(edge.v2));
            }
        }
        
    }
    void spreateRoom()
    {
        int i = 0;
        List<Vector2> beforepos=new List<Vector2>();
        while(isOverLappedAnyRoom())
        {
            i++;
            
            if (i > pos.Count * pos.Count)
            {
                Debug.LogError("적당한 위치를 찾을 수 없음");
                break;
            }
            for (int current = 0; current < pos.Count; current++)
            {
                for (int other = 0; other < pos.Count; other++)
                {
                    if (current == other || !pos[current].overlap(pos[other],mytilesize)) continue;

                    Vector2 direction = (pos[other].position - pos[current].position);
                    direction=direction.normalized;
                    if (direction.Equals(Vector2.zero))
                    {
                        direction = changeOverlapPos();
                    }
                    direction.x = Mathf.RoundToInt(direction.x);
                    direction.y = Mathf.RoundToInt(direction.y);
                    pos[other].Move(direction, mytilesize);
                    pos[current].Move(-direction, mytilesize);
                    //print($"direction {direction} currnet {pos[current].position} other {pos[other].position}");
                }
            }
        }
    }
    private Vector2 changeOverlapPos()
    {
        int x = Random.Range(-1, 2);
        int y = Random.Range(1, 3);
        if (x == 0)
        {
            if (y == 1) y = -y;
        }
        else
        {
            y -= 1;
        }
        return new Vector2(x, y);
    }
    bool isOverLappedAnyRoom()
    {
        for(int current=0;current<pos.Count; current++)
        {
            for(int other = 0; other < pos.Count; other++)
            {
                if (current == other)
                {
                    continue;
                }
                if (pos[current].overlap(pos[other], mytilesize))
                {
                    //print($"overlap {pos[current].position} and {pos[other].position}");
                    return true;
                }
            }
        }
        return false;
    }
    List<GameObject> createRectangleRoom(Vector2 size,int tilesize)
    {

        List<GameObject> room = new List<GameObject>();
        GameObject parent = new GameObject();
        room.Add(parent);
        for(int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                GameObject roomobj = Instantiate(floorObject,parent.transform);
                roomobj.transform.localPosition = new Vector2(x+0.5f-(0.5f*size.x),y + 0.5f - (0.5f * size.y))*tilesize;
                room.Add(roomobj);
            }
        }
        return room;
    }
    //recpos필요 없음. 만들고 나서 옮기면 되지
    bool checkInRectangle(Vector2 pos, Vector2 RectangleSize)
    {
        if (pos.x < 0 && pos.x > RectangleSize.x)
        {
            return false;
        }
        if (pos.y < 0 && pos.y > RectangleSize.y)
        {
            return false;
        }
        return true;
    }
    int getPixelPoint(float value,int tile_size)
    {
        int target = Mathf.RoundToInt(value);
        int error=target%tile_size;
        int result;
        double tilestand = tile_size / 2;
        if (error > 0)
        {
            if (error > tilestand)
            {
                result = target + (tile_size - error);
            }
            else
            {
                result = target - error;
            }
        }
        else
        {
            if (-error > tilestand)
            {
                result = target - (tile_size + error);
            }
            else
            {
                result = target - error;
            }
        }
        
        return result;
    }
    Vector2 getRandomPointinCircle(float radius)
    {
        float r = radius * Mathf.Sqrt(UnityEngine.Random.value);
        float theta = 2 *Mathf.PI* UnityEngine.Random.value;
        return new Vector2(r*Mathf.Cos(theta),r*Mathf.Sin(theta));
    }
    List<Edge> AddRandomEdge(ref List<Edge> edges,ref List<Edge> removeEdge)
    {
        List<Edge> result = new List<Edge>(edges);
        for(int i = 0; i < (removeEdge.Count/3); i++)
        {
            int r = Random.Range(0, removeEdge.Count);
            result.Add(removeEdge[r]);
        }
        return result;
    }

    List<Edge> AddRandomEdge(ref List<Edge> edges, ref List<Edge> removeEdge,out List<Edge> AddedEdges)
    {
        List<Edge> result = new List<Edge>(edges);
        AddedEdges=new List<Edge>();
        for (int i = 0; i < (removeEdge.Count / 3); i++)
        {
            int r = Random.Range(0, removeEdge.Count);
            result.Add(removeEdge[r]);
            AddedEdges.Add(removeEdge[r]);
        }
        return result;
    }
    /// <summary>
    /// Must be executed after moving every room.
    /// </summary>
    /// <param name="roomlist"></param>
    /// <param name="manager"></param>
    private void AddAllRoomToMapManager(ref List<Room> roomlist, ref MapManager manager)
    {
        foreach(Room room in roomlist)
        {
            manager.AddRoomtoWorld(room);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using bowyer;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using System.Threading;

public class DungeonGenerator : MonoBehaviour
{
    //todo 삼각분할의 외곽선 한 곳만 안 이어진채로 나옴. 확인필요
    //note ccw에 문제가 있음. 뒤집어 줘야 할때 뒤집지 못해서 위와 같은 문제가 발생하는 듯
    List<Room> pos = new List<Room>();
    int mytilesize = 2;
    [SerializeField]
    GameObject floorObject;


    [SerializeField]
    bool ShowLibFlag = false;
    [SerializeField]
    bool ShowMyLibFlag = false;

    //알고리즘 진행상황 확인 코드
    List<List<bowyer.Triangle>> ProcessLog = new List<List<bowyer.Triangle>>();
    [SerializeField]
    bool ShowProcess = false;
    float ProcessWaitTime = 1;
    int ProcessLogIndex = 0;

    Delaunator test;
    bowyer_watson bowyer = new bowyer_watson();
    // Start is called before the first frame update
    void Start()
    {
        
        
        //library code
        testRoomCreate(Vector2.one * 12, 4);
        //createRandomCase(10);
        resetRoomPosition();
        spreateRoom();
        testMyLib();
        showAnser();
        

    }
    void createRandomCase(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Room room = new Room(getRandomPointinCircle(1) * UnityEngine.Random.Range(-10, 10));
            room.size.x = 2;
            room.size.y = 2;
            room.x = getPixelPoint(room.x, (int)(room.size.x * 2));
            room.y = getPixelPoint(room.y, (int)(room.size.y * 2));
            pos.Add(room);

        }
    }
    void resetRoomPosition()
    {
        for (int i = 0; i < pos.Count; i++)
        {
            pos[i].gameObjects = createRectangleRoom(pos[i].size, mytilesize);
            pos[i].gameObjects[0].transform.position = pos[i].position;
        }
    }


    
    void testMyLib()
    {
        ShowMyLibFlag = true;
        for (int i = 0; i < pos.Count; i++)
        {
            bowyer.vertices.Add(new Vertex(pos[i].x, pos[i].y));
        }
        bowyer.main((List<bowyer.Triangle> list) =>
        {
            ProcessLog.Add(new List<bowyer.Triangle>(list));
        });
    }
    void showAnser()
    {
        //library code
        List<IPoint> dd = new List<IPoint>();
        for (int i = 0; i < pos.Count; i++)
        {
            dd.Add(new Point(pos[i].x, pos[i].y));
            bowyer.vertices.Add(new Vertex(pos[i].x, pos[i].y));
        }
        test = new Delaunator(dd.ToArray());
    }

    void testRoomCreate(Vector2 mainpos, int space)
    {
        Room room;
        /*
        room = new Room(mainpos);
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        /*
        room = new Room(mainpos + Vector2.up * space);
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        room = new Room(mainpos + Vector2.right * space);
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        
        room = new Room(mainpos + Vector2.down * space);
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        */
        room = new Room(mainpos + Vector2.left * space);
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        /*
        room = new Room(mainpos + Vector2.one * space);
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        /*
        room = new Room(mainpos - Vector2.one * space);
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);

        room = new Room(mainpos + (Vector2.left * space) + (Vector2.up * space));
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);*/
        room = new Room(mainpos + (Vector2.right * space) + (Vector2.down * space));
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);

        room = new Room(mainpos + Vector2.left * space*2);
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
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

    Vector2 v1, v2, v3,min,max;


    void getSuperTriangle(List<Room> mypos)
    {
        double minx = double.MaxValue, miny = double.MaxValue, maxx = double.MinValue, maxy = double.MinValue;
        for (int i = 0; i < mypos.Count; i++)
        {
            if (minx > mypos[i].x)
            {
                minx = mypos[i].x;
            }
            if (miny > mypos[i].y)
            {
                miny = mypos[i].y;
            }
            if (maxx < mypos[i].x)
            {
                maxx = mypos[i].x;
            }
            if (maxy < mypos[i].y)
            {
                maxy = mypos[i].y;
            }
        }
        minx += -10;
        maxx += 10;
        miny += -10;
        maxy += 10;
        min.x = (float)minx;
        min.y = (float)miny;
        max.x = (float)maxx;
        max.y = (float)maxy;
        double width = (maxx - minx)*2;
        double height = (maxy - miny)*2;
        v1 = new Vector2((float)minx, (float)miny);
        v2 = new Vector2((float)minx, (float)(min.y + height));
        v3 = new Vector2((float)(width + min.x), (float)miny);
        //print($"{min.x}  {max.x} {maxx - minx} {width} {maxx + width}");
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
            if(bowyer.super!=null)
            {
                Gizmos.DrawLine(Vertex2vector(bowyer.super.v1), Vertex2vector(bowyer.super.v2));
                Gizmos.DrawLine(Vertex2vector(bowyer.super.v2), Vertex2vector(bowyer.super.v3));
                Gizmos.DrawLine(Vertex2vector(bowyer.super.v3), Vertex2vector(bowyer.super.v1));
            }
            if (bowyer.triangles.Count > 0)
            {
                foreach (bowyer.Triangle triangle in bowyer.triangles)
                {
                    Gizmos.DrawLine(Vertex2vector(triangle.v1), Vertex2vector(triangle.v2));
                    Gizmos.DrawLine(Vertex2vector(triangle.v2), Vertex2vector(triangle.v3));
                    Gizmos.DrawLine(Vertex2vector(triangle.v3), Vertex2vector(triangle.v1));
                }
            }
            /*
            Gizmos.DrawLine(v1, v2);
            Gizmos.DrawLine(v2, v3);
            Gizmos.DrawLine(v3, v1);
            Gizmos.DrawCube(min, Vector3.one);
            Gizmos.DrawCube(max, Vector3.one);
            Gizmos.DrawLine(max, new Vector3(max.x,min.y,0));
            Gizmos.DrawLine(max, new Vector3(min.x, max.y, 0));*/
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(new Vector3(-6,-2),new Vector3(-6,46));
            Gizmos.DrawLine(new Vector3(-6, 46), new Vector3(16,8));
            Gizmos.DrawLine(new Vector3(-6, -2), new Vector3(16, 8));


        }

        //library show
        if (ShowLibFlag)
        {
            Gizmos.color = Color.blue;
            test.ForEachTriangle((edge) =>
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

                    Vector2 direction = (pos[other].position - pos[current].position).normalized;
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
        //getSuperTriangle(pos);

    }
    private Vector2 changeOverlapPos()
    {
        int x = UnityEngine.Random.Range(-1, 2);
        int y = UnityEngine.Random.Range(1, 3);
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

}

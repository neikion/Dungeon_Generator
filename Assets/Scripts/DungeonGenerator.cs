using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    List<Room> pos=new List<Room>();
    int mytilesize = 2;
    [SerializeField]
    GameObject floorObject;

    bool start = true;
    // Start is called before the first frame update
    void Start()
    {
        
/*        for(int i = 0; i < 2; i++)
        {
            Room room = new Room(getRandomPointinCircle(1) * UnityEngine.Random.Range(-10, 10));
            room.size.x = 2;
            room.size.y = 2;
            room.x = getPixelPoint(room.x, (int)(room.size.x*2));
            room.y = getPixelPoint(room.y, (int)(room.size.y*2));
            pos.Add(room);
            
        }*/

        int space=4;
        Room room;
        room = new Room(new Vector2(8,8));
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        room = new Room(new Vector2(8, 8 + space));
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        
        room = new Room(new Vector2(8, 8 - space));
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        room = new Room(new Vector2(8 + space, 8));
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        room = new Room(new Vector2(8 - space, 8));
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        room = new Room(new Vector2(8 + space, 8 + space));
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        room = new Room(new Vector2(8 - space, 8 - space));
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        room = new Room(new Vector2(8 - space, 8 + space));
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);
        room = new Room(new Vector2(8 + space, 8 - space));
        room.size.x = 2;
        room.size.y = 2;
        pos.Add(room);


        for (int i=0;i<pos.Count;i++)
        {
            pos[i].gameObjects = createRectangleRoom(pos[i].size, mytilesize);
            pos[i].gameObjects[0].transform.position = pos[i].position;
        }
        print("result "+isOverLappedAnyRoom());
        //spreateRoom();
    }
    private void Update()
    {
        if(start)
        {
            StartCoroutine(spreateRoom());
            start = false;
        }
       
        
    }
    IEnumerator spreateRoom()
    {
        int i = 0;
        while(isOverLappedAnyRoom())
        {
            i++;
            if (i == 10)
            {
                print("dd");
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
                        direction = Vector2.right;
                    }
                    direction.x = Mathf.RoundToInt(direction.x);
                    direction.y = Mathf.RoundToInt(direction.y);
                    pos[other].Move(direction, mytilesize);
                    print($"direction {direction} currnet {pos[current].position} other {pos[other].position}");
                    yield return new WaitForSeconds(1);
                }
            }
        }
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
                    print($"overlap {pos[current].position} and {pos[other].position}");
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
                roomobj.transform.localPosition = new Vector2(x*tilesize,y*tilesize);
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

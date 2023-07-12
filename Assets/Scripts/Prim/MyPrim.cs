using bowyer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPrim
{
    MyHeap<Edge> heap;
    List<MyNode> nodes=new List<MyNode>();
    List<Edge> edges=new List<Edge>();
    public void main(List<MyNode> nodelist, out List<Edge> result)
    {
        nodes.Add(nodelist[0]);
        nodelist.RemoveAt(0);
        heap = new MyHeap<Edge>(Edge.CompareDistanceMin);
        for(int i = 0; i < nodes[0].edges.Count; i++)
        {
            heap.Push(nodes[0].edges.List[i]);
        }
        
        int limit = 0;
        while (nodelist.Count>0 && limit<99)
        {
            
            Edge MinEdge=null;
            Vertex SelectVertex=null;
            Vertex MyVertex = null;
            bool OtherNode = false;
            while (heap.List.Count > 0 && !OtherNode)
            {
                MinEdge = heap.Pop();
                //포함되지 않은 노드가 어느쪽 점인지 확인
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (MinEdge.v1.Equals(nodes[i].Vertex))
                    {
                        SelectVertex = MinEdge.v2;
                        MyVertex = MinEdge.v1;
                        OtherNode = true;
                    }
                    else if (MinEdge.v2.Equals(nodes[i].Vertex))
                    {
                        SelectVertex = MinEdge.v1;
                        MyVertex = MinEdge.v2;
                        OtherNode = true;
                    }
                }
            }
            if (!OtherNode)
            {
                Debug.LogError($"새로 연결할 노드 없음");
                break;
            }
            for(int i = 0; i < nodelist.Count; i++)
            {
                if (nodelist[i].Vertex.Equals(SelectVertex))
                {
                    //다른 비용의 간선 제거를 위해 미리 추가
                    
                    foreach (Edge Edge in nodelist[i].edges.List)
                    {
                        heap.Push(Edge);
                        if (!checkNodesOverlap(Edge))
                        {
                            heap.Push(Edge);
                        }
                    }
                    nodes.Add(nodelist[i]);
                    nodelist.RemoveAt(i);
                    edges.Add(MinEdge);
                    break;
                }
            }
        }
        showedge();
        result = edges;
    }
    bool checkNodesOverlap(Edge edge)
    {
        //포함되지 않은 노드가 어느쪽 점인지 확인
        for (int i = 0; i < nodes.Count; i++)
        {
            if (edge.v1.Equals(nodes[i].Vertex) || edge.v2.Equals(nodes[i].Vertex))
            {
                return true;
            }
        }
        return false;
    }
    bool checkNodesOverlap(Vertex vertex)
    {
        //포함되지 않은 노드가 어느쪽 점인지 확인
        for (int i = 0; i < nodes.Count; i++)
        {
            if (vertex.Equals(nodes[i].Vertex))
            {
                return true;
            }
        }
        return false;
    }
    void showedge()
    {
        string s = $"edge count {edges.Count} \n";
        foreach (Edge edge in edges)
        {
            s += edge.ToString();
        }
        Debug.Log(s);
    }
    void shownode()
    {
        string s = $"node count {nodes.Count} \n";
        foreach (MyNode node in nodes)
        {
            s += node.Vertex+"\n";
        }
        Debug.Log(s);
    }
    void shownodelist(List<MyNode> nodelist)
    {
        string s = $"nodelist count {nodelist.Count} \n";
        foreach (MyNode node in nodelist)
        {
            s += node.Vertex + "\n";
        }
        Debug.Log(s);
    }
}

using bowyer;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyPrim
{
    MyHeap<Edge> heap;
    Dictionary<Vertex,MyNode> nodes=new Dictionary<Vertex, MyNode> ();
    List<Edge> edges = new List<Edge>();
    public void main(ref Dictionary<Vertex, MyNode> nodeset, out List<Edge> result)
    {
        MyNode node = nodeset.Values.ElementAt(0);
        nodes.Add(node.Vertex,node);
        nodeset.Remove(node.Vertex);
        heap = new MyHeap<Edge>(Edge.CompareDistanceMin);
        for (int i = 0; i < node.edges.Count; i++)
        {
            heap.Push(node.edges.List[i]);
        }
        while (nodeset.Count > 0)
        {
            Edge MinEdge = null;
            Vertex SelectVertex = null;
            Vertex MyVertex = null;
            bool OtherNode = false;
            while (heap.List.Count > 0 && !OtherNode)
            {
                MinEdge = heap.Pop();
                if (nodes.ContainsKey(MinEdge.v1) && !nodes.ContainsKey(MinEdge.v2))
                {
                    SelectVertex = MinEdge.v2;
                    MyVertex = MinEdge.v1;
                    OtherNode = true;
                }
                else if (nodes.ContainsKey(MinEdge.v2) && !nodes.ContainsKey(MinEdge.v1))
                {
                    SelectVertex = MinEdge.v1;
                    MyVertex = MinEdge.v2;
                    OtherNode = true;
                }

            }
            if (!OtherNode)
            {
                Debug.LogError($"새로 연결할 노드 없음");
                break;
            }
            MyNode SelectNode = nodeset[SelectVertex];
            foreach (Edge Edge in SelectNode.edges.List)
            {
                if (!nodes.ContainsKey(Edge.v1) && !nodes.ContainsKey(Edge.v2) && !Edge.Equals(MinEdge))
                {
                    heap.Push(Edge);
                }
            }
            nodes.Add(SelectNode.Vertex, SelectNode);

            nodeset.Remove(SelectVertex);
            edges.Add(MinEdge);
            
        }
        
        result = edges;
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
        string s = $"nodes count {nodes.Count} \n";
        foreach (MyNode node in nodes.Values)
        {
            s += node.Vertex + "\n";
        }
        Debug.Log(s);
    }
    void shownode(Dictionary<Vertex,MyNode> nodeset)
    {
        string s = $"nodeset count {nodeset.Count} \n";
        foreach (MyNode node in nodeset.Values)
        {
            s += node.Vertex + "\n";
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

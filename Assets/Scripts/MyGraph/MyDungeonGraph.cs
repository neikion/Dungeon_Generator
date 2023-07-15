using bowyer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Edge = bowyer.Edge;

public class MyDungeonGraph
{
    /*
    todo
    prim이던지 kruskal이던지 모두 한 노드가 다른 노드와 얼마나 연결되어 있는지에 대한 정보가 필요하다.
    현재 한 노드에서 다른 노드로 얼마나 연결되어 있는지 알 수 없으므로 새롭게 만들어야 한다.
    heap은 어차피 중간에 새로 삽입할 거 아니면 introsort를 사용하는 array.sort를 써도 상관없을 듯 하다.
    
    introsort
    파티션 크기가 16개 요소보다 작거나 같으면 삽입 정렬(삽입 정렬을 배열 크기가 잘을 때는 매우 효율적)
    파티션의 크기가 2*logN (N은 입력 배열의 범위)일 때 힙 정렬
    이외에는 퀵정렬
    */
    Dictionary<Vertex,GraphNode> nodeset=new Dictionary<Vertex, GraphNode>();
    public ref Dictionary<Vertex, GraphNode> main(List<Edge> edges)
    {
        for(int i=edges.Count-1; i >-1; i--)
        {
            if (nodeset.ContainsKey(edges[i].v1))
            {
                nodeset[edges[i].v1].edges.List.Add(edges[i]);
            }
            if (nodeset.ContainsKey(edges[i].v2))
            {
                nodeset[edges[i].v2].edges.List.Add(edges[i]);
            }

            if (!nodeset.ContainsKey(edges[i].v1))
            {
                GraphNode node = new GraphNode();
                node.Vertex = edges[i].v1;
                node.edges = new MyHeap<Edge>(Edge.CompareDistanceMin);
                node.edges.Push(edges[i]);
                nodeset.Add(node.Vertex, node);
            }
            if (!nodeset.ContainsKey(edges[i].v2))
            {
                GraphNode node = new GraphNode();
                node.Vertex = edges[i].v2;
                node.edges = new MyHeap<Edge>(Edge.CompareDistanceMin);
                node.edges.Push(edges[i]);
                nodeset.Add(node.Vertex, node);
            }
            

        }
        return ref nodeset;
    }
    void showDictionary()
    {
        string s = $"show dictionary \n dictionary count {nodeset.Count} \n";
        foreach(KeyValuePair<Vertex,GraphNode> value in nodeset)
        {
            s+=$"{value.Key} {value.Value} \n";

        }
        Debug.Log(s);
    }
    void showDictionaryDetail()
    {
        string s = $"show dictionary \n dictionary count {nodeset.Count} \n";
        foreach (KeyValuePair<Vertex, GraphNode> value in nodeset)
        {
            s += $"{value.Key} {value.Value} \n {{";
            foreach(Edge edge in value.Value.edges.List)
            {
                s += $"{edge} ";
            }
            s += "}\n";

        }
        Debug.Log(s);
    }
}

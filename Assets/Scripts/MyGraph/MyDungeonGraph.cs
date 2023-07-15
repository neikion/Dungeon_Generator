using bowyer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Edge = bowyer.Edge;

public class MyDungeonGraph
{
    /*
    todo
    prim�̴��� kruskal�̴��� ��� �� ��尡 �ٸ� ���� �󸶳� ����Ǿ� �ִ����� ���� ������ �ʿ��ϴ�.
    ���� �� ��忡�� �ٸ� ���� �󸶳� ����Ǿ� �ִ��� �� �� �����Ƿ� ���Ӱ� ������ �Ѵ�.
    heap�� ������ �߰��� ���� ������ �� �ƴϸ� introsort�� ����ϴ� array.sort�� �ᵵ ������� �� �ϴ�.
    
    introsort
    ��Ƽ�� ũ�Ⱑ 16�� ��Һ��� �۰ų� ������ ���� ����(���� ������ �迭 ũ�Ⱑ ���� ���� �ſ� ȿ����)
    ��Ƽ���� ũ�Ⱑ 2*logN (N�� �Է� �迭�� ����)�� �� �� ����
    �̿ܿ��� ������
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

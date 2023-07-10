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
    Dictionary<Vertex,MyNode> nodeset=new Dictionary<Vertex, MyNode>();
    public void main(MyHeap<Edge> edges)
    {
        List<Edge> list = edges.List;
        for(int i=list.Count-1; i >-1; i--)
        {
            if (nodeset.ContainsKey(list[i].v1))
            {
                nodeset[list[i].v1].edges.Add(list[i]);
            }
            if (nodeset.ContainsKey(list[i].v2))
            {
                nodeset[list[i].v2].edges.Add(list[i]);
            }
            if (!nodeset.ContainsKey(list[i].v1))
            {
                MyNode node = new MyNode();
                node.Vertex = list[i].v1;
                node.edges = new List<Edge> { list[i] };
                nodeset.Add(node.Vertex, node);
            }
            if (!nodeset.ContainsKey(list[i].v2))
            {
                MyNode node = new MyNode();
                node.Vertex = list[i].v2;
                node.edges = new List<Edge> { list[i] };
                nodeset.Add(node.Vertex, node);
            }
            showDictionary();

        }
        
        List<MyNode> nodelist=new List<MyNode>();
        //string s="";
        foreach(MyNode node in nodeset.Values)
        {
            nodelist.Add(node);
            //s += node.Vertex.ToString()+"\n";
        }
        //Debug.Log(s);
    }
    public void showDictionary()
    {
        string s = $"show dictionary \n dictionary count {nodeset.Count} \n";
        foreach(KeyValuePair<Vertex,MyNode> value in nodeset)
        {
            s+=$"{value.Key} {value.Value} \n";

        }
        Debug.Log(s);
    }
}

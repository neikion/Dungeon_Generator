using bowyer;
using System.Collections.Generic;
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
            if (!nodeset.ContainsKey(list[i].v1))
            {
                MyNode node = new MyNode();
                node.Vertex = list[i].v1;
                node.edges.Add(list[i]);
            }else if (!nodeset.ContainsKey(list[i].v2))
            {
                MyNode node = new MyNode();
                node.Vertex = list[i].v2;
                node.edges.Add(list[i]);
            }
            
        }
    }
}

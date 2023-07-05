using System;
using System.Collections.Generic;
public class MyHeap<T>
{
    private List<T> list=new List<T>();
    Comparison<T> comparer;
    public int Count
    {
        get { return list.Count; }
    }

    public MyHeap(Comparison<T> comparison)
    {
        comparer = comparison;
    }

    public void Push(T item)
    {
        list.Add(item);
        int index = list.Count-1;
        int parent = (index-1) / 2;
        while (index > 0 && comparer(item,list[parent])<0)
        {
            swap(parent, index);
            index = parent;
            parent = (parent-1) / 2;
        }
    }
    public T Pop()
    {
        if(list.Count == 0)
        {
            throw new InvalidOperationException("heap is empty");
        }
        T result = list[0];
        list[0] = list[list.Count-1];
        list.RemoveAt(list.Count-1);
        int parent=0,child;
        while (parent < list.Count-1)
        {
            child = (parent * 2) + 1;
            if (child + 1 < list.Count && comparer(list[child + 1], list[child])<0)
            {
                ++child;
            }
            if (child>=list.Count || comparer(list[parent], list[child])<0)
            {
                break;
            }
            
            swap(parent, child);
            parent = child;
        }

        return result;
    }
    private void swap(int parent_index, int child_index)
    {
        T item = list[parent_index];
        list[parent_index] = list[child_index];
        list[child_index] = item;
    }
}

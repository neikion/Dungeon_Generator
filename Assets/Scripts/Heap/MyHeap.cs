using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class MyHeap<T> where T : IComparable<T>
{
    private List<T> list=new List<T>();
    public void Push(T item)
    {
        list.Add(item);
        int index = list.Count;
        int parent = (index-1) / 2;
        while (index > 0 && item.CompareTo(list[parent]) < 0)
        {
            swap(parent, ref item);
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
        int parent=0;
        int child=1;
        while (parent < list.Count-1)
        {
            child = (parent * 2) + 1;
            if (child+1<list.Count-1&&list[child+1].CompareTo(list[child]) < 0)
            {
                ++child;
            }
            if (child<list.Count&&list[parent].CompareTo(list[child]) <= 0)
            {
                break;
            }

            swap(parent, child);
            parent = child;
        }

        return result;
    }
    private void swap(int parent_index, ref T child)
    {
        T item=list[parent_index];
        list[parent_index] = child;
        child = item;
    }
    private void swap(int parent_index, int child_index)
    {
        T item = list[parent_index];
        list[parent_index] = list[child_index];
        list[child_index] = item;
    }
}

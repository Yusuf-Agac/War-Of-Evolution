using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Heap<T> where T : Heap<T>.IHeapItem<T>
{
    private T[] items;
    private int itemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = itemCount;
        items[item.HeapIndex] = item;
        SortUp(item);
        itemCount++;
    }

    public T RemoveFirstItem()
    {
        T removedItem = items[0];
        itemCount--;
        items[0] = items[itemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return removedItem;
    }

    public bool Contains(T item)
    {
        return Equals(item, items[item.HeapIndex]);
    }

    public int Count
    {
        get
        {
            return itemCount;
        }
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }
    
    private void SortDown(T item)
    {
        while (true)
        {
            int leftChildIndex = (item.HeapIndex * 2) + 1;
            int rightChildIndex = (item.HeapIndex * 2) + 2;
            T leftChild = items[leftChildIndex];
            T rightChild = items[rightChildIndex];
            if(leftChildIndex < itemCount)
            {
                int swapIndex = leftChildIndex;
                if (rightChildIndex < itemCount)
                {
                    if (rightChild.CompareTo(leftChild) > 0)
                    {
                        swapIndex = rightChildIndex;
                    }
                }

                if (items[swapIndex].CompareTo(item) > 0)
                {
                    SwapItem(items[swapIndex], item);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
    
    private void SortUp(T item)
    {
        while (true)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                SwapItem(parentItem, item);
            }
            else
            {
                break;
            }
        }
    }

    private void SwapItem(T itemA, T itemB)
    {
        (itemA.HeapIndex, itemB.HeapIndex) = (itemB.HeapIndex, itemA.HeapIndex);
        items[itemA.HeapIndex] = itemA;
        items[itemB.HeapIndex] = itemB;
    }
    
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex
        {
            get;
            set;
        }
    }
}

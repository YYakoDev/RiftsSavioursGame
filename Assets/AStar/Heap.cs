using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    T[] _items;
    int _currentItemCount = 0;
    public int Count => _currentItemCount;

    public Heap(int maxHeapSize)
    {
        _items = new T[maxHeapSize];
    }

    public void Insert(T item)
    {
        if(_currentItemCount >= _items.Length)
        {
            Array.Resize(ref _items, _currentItemCount);
        }
        item.Index = _currentItemCount;
        _items[_currentItemCount] = item;
        SortUp(item);
        _currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = _items[0];
        _currentItemCount--;
        _items[0] = _items[_currentItemCount];
        _items[0].Index = 0;
        SortDown(_items[0]);
        return firstItem;
    }

    public bool Contains(T item)
    {
        if(item.Index < _currentItemCount) return Equals(_items[item.Index], item);
        else return false;
    }
    public void UpdateItem(T item)
    {
        SortUp(item);
    }
    public void Clear()
    {
        _currentItemCount = 0;
    }

    void SortUp(T item)
    {
        int parentIndex = (item.Index-1)/2;
        while (true)
        {
            T parentItem = _items[parentIndex];
            if(item.CompareTo(parentItem) > 0)
            {
                Swap(parentItem, item);
            }else
            {
                break;
            }
            parentIndex = (item.Index-1)/2;
        }
    }

    void SortDown(T item)
    {
        while(true)
        {
            int childIndexLeft = item.Index * 2 + 1;
            int childIndexRight = item.Index * 2 + 2;
            int swapIndex = 0;

            if(childIndexLeft < _currentItemCount)
            {
                swapIndex = childIndexLeft;
                if(childIndexRight < _currentItemCount)
                {
                    if(_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if(item.CompareTo(_items[swapIndex]) < 0)
                {
                    Swap(item, _items[swapIndex]);
                }else
                {
                    break;
                }
            }else
            {
                break;
            }
        }
    }

    void Swap(T itemA, T itemB)
    {
        _items[itemA.Index] = itemB;
        _items[itemB.Index] = itemA;

        int itemAIndex = itemA.Index;
        itemA.Index = itemB.Index;
        itemB.Index = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    public int Index {get;set;}
}


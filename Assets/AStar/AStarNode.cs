using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : IHeapItem<AStarNode>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX, gridY;
    public int heapIndex;
    public int gCost, hCost; // G Cost: The distance FROM the start node
                             // H Cost: The distance TO the end node
    public int FCost => gCost + hCost;
    public int Index { get => heapIndex; set => heapIndex = value; }

    public AStarNode parent;
    private int _neighboursCount = 0;
    private AStarNode[] _neighbours = new AStarNode[8];
    public AStarNode[] Neighbours => _neighbours;

    public AStarNode(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public void AssignNeighbour(AStarNode neighbour)
    {
        if(neighbour == null || _neighboursCount >= _neighbours.Length)return;
        _neighbours[_neighboursCount] = neighbour;
        _neighboursCount++;
    }

    public int CompareTo(AStarNode nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}

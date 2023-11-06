using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AStarPathfinding : MonoBehaviour
{

    AStarPathRequestManager _requestManager;
    AStarGrid _grid;
    Heap<AStarNode> _openSet;
    HashSet<AStarNode> _closedSet;
    AStarNode startNode; 
    AStarNode targetNode;
    Vector3[] _waypoints = new Vector3[0];

    private void Awake() {
        _grid = GetComponent<AStarGrid>();
        _requestManager = GetComponent<AStarPathRequestManager>();
    }

    private void Start() {
        _openSet = new(_grid.GridTotalSize);
        _closedSet = new();
    }
    /*private void Update()
    {
        if(Input.GetButtonDown("Jump")) FindPath(_seeker.position, target.position);
    }*/

    public void StartFindPath(Vector3 start, Vector3 end)
    {
        AStarPathResult result = FindPath(start, end);
        if(result == AStarPathResult.PathSuccessful)
        {
            _waypoints = RetracePath(startNode, targetNode);
        }
        _requestManager.FinishedProcessingPath(_waypoints, result);
    }

    IEnumerator FrameSkip(int numberOfFrames = 1)
    {
        for(int i = 0; i < numberOfFrames; i++)
        {
            yield return null;
        }
    }
    AStarPathResult FindPath(Vector3 startPos, Vector3 targetPos)
    {
        _openSet.Clear();
        _closedSet.Clear();

        startNode = _grid.NodeFromWorldPoint(startPos);
        targetNode = _grid.NodeFromWorldPoint(targetPos);

        AStarPathResult pathResult = AStarPathResult.PathUnsuccessful;

        if(startNode == null || targetNode == null) return AStarPathResult.PathUnsuccessful;
        if(!startNode.walkable) return AStarPathResult.StartingPositionUnwalkable;
        if(!targetNode.walkable) return AStarPathResult.TargetUnreachable;
      
        _openSet.Insert(startNode);

        while (_openSet.Count > 0)
        {
            AStarNode currentNode = _openSet.RemoveFirst();
            _closedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                pathResult = AStarPathResult.PathSuccessful;
                break;
            }
            AStarNode[] neighbours = currentNode.Neighbours;
            foreach(AStarNode neighbour in neighbours)
            {
                if(neighbour == null || !neighbour.walkable || _closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetNodeDistance(currentNode, neighbour);
                bool isNeighbourOnOpenSet = _openSet.Contains(neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !isNeighbourOnOpenSet)
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetNodeDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;
                    if(!isNeighbourOnOpenSet)
                    {
                        _openSet.Insert(neighbour);
                    }
                }
            }
        }
        StartCoroutine(FrameSkip());

        return pathResult;
    }

    Vector3[] RetracePath(AStarNode startNode, AStarNode endNode)
    {
        List<AStarNode> path = new();
        AStarNode currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints =  SimplifyPath(path, startNode);
        Array.Reverse(waypoints);
        return waypoints;
        //_grid.path = path;
    }

    Vector3[] SimplifyPath(List<AStarNode> path, AStarNode startNode)
    {
        List<Vector3> waypoints = new();
        Vector2 lastDirection = Vector2.zero;
        for(int i = 1; i < path.Count; i++)
        {   
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if(directionNew != lastDirection)
            {
                waypoints.Add(path[i].worldPosition);
                lastDirection = directionNew;
            }
            if(i == path.Count -1)
            {
                waypoints.Add(path[i].worldPosition);
            }
        }
        return waypoints.ToArray();
    }

    int GetNodeDistance(AStarNode nodeA, AStarNode nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if(distanceX > distanceY) return 14 * distanceY + 10 * (distanceX - distanceY); //the 14 & 10 values are a variation of the "Manhattan distance"
        else return 14 * distanceX + 10 * (distanceY - distanceX);
    }


}

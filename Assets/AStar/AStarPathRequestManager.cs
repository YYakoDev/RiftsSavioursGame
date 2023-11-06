using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathRequestManager : MonoBehaviour
{
    public static AStarPathRequestManager i;
    AStarPathfinding _aPathfinding;
    Queue<PathRequest> _pathRequestQueue = new();
    PathRequest _currentPathRequest;


    bool _isProcessingPath;
    bool _isFindingNode;

    private void Awake()
    {
        if(i != null && i != this)
        {
            Debug.Log("Deleting duplicated PathRequestManager");
            Destroy(this);
        }else
        {
            i = this;
        }

        _aPathfinding = GetComponent<AStarPathfinding>();

    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], AStarPathResult> callback)
    {
        //here you can translate the path start and path end into the position of the grid they are currently in and send that grid to the pathfinding
        PathRequest newRequest = new(pathStart, pathEnd, callback);
        i._pathRequestQueue.Enqueue(newRequest);
        i.TryProcessNext();
    }
    void TryProcessNext()
    {
        if(!_isProcessingPath && _pathRequestQueue.Count > 0)
        {
            _currentPathRequest = _pathRequestQueue.Dequeue();
            _isProcessingPath = true;
            _aPathfinding.StartFindPath(_currentPathRequest.pathStart, _currentPathRequest.pathEnd);
        }
    }
    public void FinishedProcessingPath(Vector3[] path, AStarPathResult result)
    {
        _currentPathRequest.callback(path, result);
        _isProcessingPath = false;
        TryProcessNext();
    }
    struct PathRequest
    {
        public Vector3 pathStart, pathEnd;
        public Action<Vector3[], AStarPathResult> callback;

        public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], AStarPathResult> callback)
        {
            pathStart = start;
            pathEnd = end;
            this.callback = callback;
        }
    }
}

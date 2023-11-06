using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    [SerializeField]Vector2 _gridWorldSize;
    [SerializeField]LayerMask _obstacleLayer;
    [SerializeField]float _nodeRadius = 1;
    AStarNode[,] _grid;
    [SerializeField]bool _displayGizmosGrid;

    float _nodeDiameter;
    int _gridSizeX, _gridSizeY;

    public int GridTotalSize => _gridSizeX * _gridSizeY;
    
    // Start is called before the first frame update
    void Awake()
    {
        _nodeDiameter = _nodeRadius*2;
        _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x/_nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y/_nodeDiameter);
        CreateGrid();
        foreach(AStarNode node in _grid)
        {
            FindNeighbours(node);
        }
    }

    void CreateGrid()
    {
        _grid = new AStarNode[_gridSizeX, _gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * (_gridWorldSize.x/2 ) - Vector3.up * (_gridWorldSize.y/2);
        for(int x = 0; x < _gridSizeX; x++)
        {
            for(int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.up * (y * _nodeDiameter + _nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, _nodeRadius, _obstacleLayer));
                _grid[x,y] = new AStarNode(walkable, worldPoint, x, y);
            }            
        }
    }


    void FindNeighbours(AStarNode node)
    {
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0) continue;
                
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                //if it is within the grid
                if(checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY) 
                {
                    node.AssignNeighbour(_grid[checkX,checkY]);
                }
            }
        }
    }

    public AStarNode NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = GetPercent(worldPosition.x, _gridWorldSize.x);
        float percentY = GetPercent(worldPosition.y, _gridWorldSize.y);

        int x = Mathf.RoundToInt((_gridSizeX-1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY-1) * percentY);

        if(x < 0 || y < 0) 
        {
            Debug.Log("node in negative position outside the grid");
            return null;
        }
        if(x >= _grid.GetLength(0) || y >= _grid.GetLength(1))
        {
            Debug.Log("node outside the grid");
            return null;
        }
        
        return _grid[x,y];

        float GetPercent(float worldPointComponent, float gridWorldSizeComponent)
        {
            float result = worldPointComponent/gridWorldSizeComponent + 0.5f;
            result = Mathf.Clamp01(result);
            return result;
        }
    }



    private void OnDrawGizmos() {
        if(!_displayGizmosGrid) return;
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, _gridWorldSize);
        if(_grid != null)
        {
            foreach(AStarNode n in _grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one*(_nodeDiameter - 0.1f));
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridWidth = 6;
    public int gridHeight = 6;
    public float spacing = 1.1f;
    public GameObject parentObject;

    private Dictionary<Vector2Int, GridInfo> allGridInfos = new Dictionary<Vector2Int, GridInfo>();

    private void Start()
    {
        GenerateGrid();
        AssignNeighbors();
    }

    private void GenerateGrid()
    {
        if (parentObject == null)
            parentObject = new GameObject( "GridParent" );
        allGridInfos.Clear();
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3( x * spacing, y * spacing, 0 );
                GameObject tile = Instantiate( tilePrefab, position, Quaternion.identity );
                tile.name = $"Parent Grid {x},{y}";
                tile.transform.parent = parentObject.transform;
                NameAndTrackChildGridInfos( tile, x, y );
            }
        }
    }

    private void NameAndTrackChildGridInfos( GameObject parentTile, int parentX, int parentY )
    {
        GridInfo[] gridInfos = parentTile.GetComponentsInChildren<GridInfo>();

        foreach (GridInfo gridInfo in gridInfos)
        {
            Vector3 localPos = gridInfo.transform.localPosition;

            int globalX = parentX * 2 + (localPos.x > 0 ? 1 : 0);
            int globalY = parentY * 2 + (localPos.y > 0 ? 1 : 0);

            Vector2Int gridKey = new Vector2Int( globalX, globalY );
            gridInfo.name = $"Grid {globalX},{globalY}";
            if (!allGridInfos.ContainsKey( gridKey ))
                allGridInfos[gridKey] = gridInfo;
            else
                Debug.LogWarning( "Duplicate Grid Info" );
        }
    }

    private void AssignNeighbors()
    {
        foreach (var kvp in allGridInfos)
        {
            Vector2Int pos = kvp.Key;
            GridInfo current = kvp.Value;

            if (allGridInfos.TryGetValue( pos + Vector2Int.up, out GridInfo topNeighbor ))
                current.topNeighbor = topNeighbor;
            if (allGridInfos.TryGetValue( pos + Vector2Int.down, out GridInfo bottomNeighbor ))
                current.bottomNeighbor = bottomNeighbor;
            if (allGridInfos.TryGetValue( pos + Vector2Int.left, out GridInfo leftNeighbor ))
                current.leftNeighbor = leftNeighbor;
            if (allGridInfos.TryGetValue( pos + Vector2Int.right, out GridInfo rightNeighbor ))
                current.rightNeighbor = rightNeighbor;
        }
    }
}

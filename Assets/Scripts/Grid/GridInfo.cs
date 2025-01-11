using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo : MonoBehaviour
{
    public GameObject occupyingBlock;

    public GridInfo topNeighbor;
    public GridInfo bottomNeighbor;
    public GridInfo leftNeighbor;
    public GridInfo rightNeighbor;

    public void AdjustGridOccupyingBlock( GameObject[] cubes )
    {
        if (cubes == null || cubes.Length == 0)
            return;
        float minDistance = Mathf.Infinity;
        GameObject closestCube = null;
        foreach (GameObject cube in cubes)
        {
            if (cube == null) continue;
            float distance = Vector3.Distance( transform.position, cube.transform.position );
            if (distance < minDistance)
            {
                minDistance = distance;
                closestCube = cube;
            }
        }
        occupyingBlock = closestCube;
    }
}

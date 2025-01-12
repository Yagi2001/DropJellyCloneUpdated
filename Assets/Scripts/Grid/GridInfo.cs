using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo : MonoBehaviour
{
    private DetectMatch _detectMatch;
    public GameObject occupyingBlock;

    public GridInfo topNeighbor;
    public GridInfo bottomNeighbor;
    public GridInfo leftNeighbor;
    public GridInfo rightNeighbor;
    private void Start()
    {
        _detectMatch = GetComponent<DetectMatch>();
    }
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
        StartCoroutine( DetectMatchingNeighborsCoroutine() );
    }
    //Using coroutine in here to wait for all grids adjustments
    private IEnumerator DetectMatchingNeighborsCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        if (_detectMatch != null)
            _detectMatch.DetectMatchingNeighbors();
    }
}

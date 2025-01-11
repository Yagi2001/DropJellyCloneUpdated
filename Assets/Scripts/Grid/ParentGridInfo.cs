using System.Collections;
using UnityEngine;

public class ParentGridInfo : MonoBehaviour
{
    public bool isOccupied;

    private void Start()
    {
        isOccupied = false;
    }

    public void CheckAndAdjustOccupation( GameObject[] cubes )
    {
        AdjustAllChildGridOccupyingBlocks( cubes );
        CheckOccupation();
    }

    private void AdjustAllChildGridOccupyingBlocks( GameObject[] cubes )
    {
        GridInfo[] childGridInfos = GetComponentsInChildren<GridInfo>();
        foreach (var gridInfo in childGridInfos)
        {
            gridInfo.AdjustGridOccupyingBlock( cubes );
        }
    }

    private void CheckOccupation()
    {
        GridInfo[] childGridInfos = GetComponentsInChildren<GridInfo>();
        foreach (var gridInfo in childGridInfos)
        {
            if (gridInfo.occupyingBlock != null)
            {
                isOccupied = true;
                return;
            }
        }
        isOccupied = false;
    }
}

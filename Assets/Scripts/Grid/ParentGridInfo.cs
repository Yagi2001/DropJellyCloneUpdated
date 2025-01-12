using System.Collections;
using UnityEngine;

public class ParentGridInfo : MonoBehaviour
{
    public bool isOccupied;
    public GameObject occupyingBlock;
    public GameObject topParentGrid;
    private ParentGridInfo _topParentGridInfo;

    private void Start()
    {
        isOccupied = false;
        if (topParentGrid != null)
            _topParentGridInfo = topParentGrid.GetComponent<ParentGridInfo>();
    }

    private void Update()
    {
        if (occupyingBlock == null)
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
        if (occupyingBlock != null)
        {
            isOccupied = true;
            return;
        }

        Destroy( occupyingBlock );
        isOccupied = false;

        if (_topParentGridInfo != null && _topParentGridInfo.isOccupied && _topParentGridInfo.occupyingBlock != null)
        {
            BlockMovement blockMovement = _topParentGridInfo.occupyingBlock.GetComponent<BlockMovement>();
            if (blockMovement != null)
            {
                blockMovement.FallToNonOccupiedGrid( gameObject );
            }
        }
    }
}

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

        FindAndAssignOccupyingBlockAtStart();
    }

    public void CheckAndAdjustOccupation( GameObject[] cubes )
    {
        AdjustAllChildGridOccupyingBlocks( cubes );
        CheckOccupation();
    }

    private void FindAndAssignOccupyingBlockAtStart()
    {
        GameObject[] levelCubes = GameObject.FindGameObjectsWithTag( "LevelCube" );

        foreach (GameObject cube in levelCubes)
        {
            float distance = Vector3.Distance( transform.position, cube.transform.position );
            if (distance <= 0.1f)
            {
                isOccupied = true;
                occupyingBlock = cube;
                BlockInfo blockInfo = cube.GetComponent<BlockInfo>();
                if (blockInfo != null)
                {
                    var cornerObjects = blockInfo.GetCornerStates().Values;
                    GameObject[] cornersArray = new GameObject[cornerObjects.Count];
                    cornerObjects.CopyTo( cornersArray, 0 );
                    CheckAndAdjustOccupation( cornersArray );
                }
                return;
            }
        }
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
            _topParentGridInfo.isOccupied = false;
            BlockMovement blockMovement = _topParentGridInfo.occupyingBlock.GetComponent<BlockMovement>();
            if (blockMovement != null)
            {
                blockMovement.FallToNonOccupiedGrid( gameObject );
            }
        }
    }
}

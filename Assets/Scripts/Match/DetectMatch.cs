using System.Collections;
using UnityEngine;

public class DetectMatch : MonoBehaviour
{
    private GridInfo _gridInfo;

    private void Start()
    {
        _gridInfo = GetComponent<GridInfo>();
    }

    public void DetectMatchingNeighbors()
    {
        if (_gridInfo.occupyingBlock == null) return;

        string blockTag = _gridInfo.occupyingBlock.tag;
        Transform blockParent = _gridInfo.occupyingBlock.transform.parent;

        // Check neighbors for matches and destroy if found
        if (HasMatchingTagAndParent( _gridInfo.topNeighbor, blockTag, blockParent ))
            StartCoroutine( DestroyMatchingBlocks( _gridInfo.topNeighbor ) );
        if (HasMatchingTagAndParent( _gridInfo.bottomNeighbor, blockTag, blockParent ))
            StartCoroutine( DestroyMatchingBlocks( _gridInfo.bottomNeighbor ) );
        if (HasMatchingTagAndParent( _gridInfo.leftNeighbor, blockTag, blockParent ))
            StartCoroutine( DestroyMatchingBlocks( _gridInfo.leftNeighbor ) );
        if (HasMatchingTagAndParent( _gridInfo.rightNeighbor, blockTag, blockParent ))
            StartCoroutine( DestroyMatchingBlocks( _gridInfo.rightNeighbor ) );
    }

    private bool HasMatchingTagAndParent( GridInfo neighbor, string blockTag, Transform blockParent )
    {
        return neighbor != null &&
               neighbor.occupyingBlock != null &&
               neighbor.occupyingBlock.tag == blockTag &&
               neighbor.occupyingBlock.transform.parent != blockParent;
    }

    private IEnumerator DestroyMatchingBlocks( GridInfo matchingNeighbor )
    {
        //We store necessary referances
        ParentGridInfo parentGridNeighbor = matchingNeighbor.transform.parent.GetComponent<ParentGridInfo>();
        ParentGridInfo parentGrid = transform.parent.GetComponent<ParentGridInfo>();
        FillBlock parentFillBlockNeighbor = matchingNeighbor.occupyingBlock.transform.parent.GetComponent<FillBlock>();
        FillBlock currentParentFillBlock = _gridInfo.occupyingBlock.transform.parent.GetComponent<FillBlock>();
        BlockInfo blockInfoNeighbor = matchingNeighbor.occupyingBlock.transform.parent.GetComponent<BlockInfo>();
        BlockInfo currentBlockInfo = _gridInfo.occupyingBlock.transform.parent.GetComponent<BlockInfo>();
        // We destroy matching block of neighbor
        Destroy( matchingNeighbor.occupyingBlock );
        matchingNeighbor.occupyingBlock = null;
        yield return null;
        // We destroy matching block of our current
        parentFillBlockNeighbor.CheckAndFillEmptyPieces();
        Destroy( _gridInfo.occupyingBlock );
        _gridInfo.occupyingBlock = null;
        yield return null;
        currentParentFillBlock.CheckAndFillEmptyPieces();
        //Resizing done
        yield return null;
        // We check and adjust tiles again
        GameObject[] cornersArrayNeighbor = GetCornerObjects( blockInfoNeighbor);
        GameObject[] cornersArrayCurrent = GetCornerObjects( currentBlockInfo);
        parentGridNeighbor.CheckAndAdjustOccupation(cornersArrayNeighbor);
        parentGrid.CheckAndAdjustOccupation( cornersArrayCurrent );
    }
    private GameObject[] GetCornerObjects( BlockInfo blockParent )
    {
        if (blockParent == null) return new GameObject[0];

        BlockInfo blockInfo = blockParent.GetComponent<BlockInfo>();
        if (blockInfo == null) return new GameObject[0];

        var cornerObjects = blockInfo.GetCornerStates().Values;
        GameObject[] cornersArray = new GameObject[cornerObjects.Count];
        cornerObjects.CopyTo( cornersArray, 0 );
        return cornersArray;
    }
}

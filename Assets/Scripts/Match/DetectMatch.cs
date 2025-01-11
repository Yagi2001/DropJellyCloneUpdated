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
        FillBlock parentFillBlock = matchingNeighbor.occupyingBlock.transform.parent.GetComponent<FillBlock>();
        FillBlock currentParentFillBlock = _gridInfo.occupyingBlock.transform.parent.GetComponent<FillBlock>();
        Destroy( matchingNeighbor.occupyingBlock );
        matchingNeighbor.occupyingBlock = null;
        yield return null;
        parentFillBlock.CheckAndFillEmptyPieces();
        Destroy( _gridInfo.occupyingBlock );
        _gridInfo.occupyingBlock = null;
        yield return null;
        currentParentFillBlock.CheckAndFillEmptyPieces();
    }
}

using System.Collections;
using UnityEngine;

public class DetectMatch : MonoBehaviour
{
    private GridInfo _gridInfo;
    [SerializeField]
    private float _animationDuration = 0.3f;

    private void Start()
    {
        _animationDuration = 0.3f;
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
        // We store necessary references
        ParentGridInfo parentGridNeighbor = matchingNeighbor.transform.parent.GetComponent<ParentGridInfo>();
        ParentGridInfo parentGrid = transform.parent.GetComponent<ParentGridInfo>();
        FillBlock parentFillBlockNeighbor = matchingNeighbor.occupyingBlock.transform.parent.GetComponent<FillBlock>();
        FillBlock currentParentFillBlock = _gridInfo.occupyingBlock.transform.parent.GetComponent<FillBlock>();
        BlockInfo blockInfoNeighbor = matchingNeighbor.occupyingBlock.transform.parent.GetComponent<BlockInfo>();
        BlockInfo currentBlockInfo = _gridInfo.occupyingBlock.transform.parent.GetComponent<BlockInfo>();

        // Animate the blocks before destroying them
        yield return AnimateAndDestroy( matchingNeighbor.occupyingBlock, _gridInfo.occupyingBlock );

        // We destroy matching block of neighbor
        Destroy( matchingNeighbor.occupyingBlock );
        matchingNeighbor.occupyingBlock = null;

        // Resizing done
        yield return null;
        if(parentFillBlockNeighbor!=null)
            parentFillBlockNeighbor.CheckAndFillEmptyPieces();

        // We destroy matching block of our current
        Destroy( _gridInfo.occupyingBlock );
        _gridInfo.occupyingBlock = null;
        yield return null;
        if(currentParentFillBlock != null)
            currentParentFillBlock.CheckAndFillEmptyPieces();

        // We check and adjust tiles again
        GameObject[] cornersArrayNeighbor = GetCornerObjects( blockInfoNeighbor );
        GameObject[] cornersArrayCurrent = GetCornerObjects( currentBlockInfo );
        yield return null;
        parentGridNeighbor.CheckAndAdjustOccupation( cornersArrayNeighbor );
        yield return null;
        parentGrid.CheckAndAdjustOccupation( cornersArrayCurrent );
    }

    //Since this is a basic animation I used codes to animate instead of clips
    private IEnumerator AnimateAndDestroy( GameObject block1, GameObject block2 )
    {
        float duration = _animationDuration; // Duration of the animation
        float elapsedTime = 0f;

        Vector3 startScale1 = block1.transform.localScale;
        Vector3 startScale2 = block2.transform.localScale;
        Vector3 endScale1 = startScale1 * 1.5f; // Target scale
        Vector3 endScale2 = startScale2 * 1.5f;

        Vector3 startPosition1 = block1.transform.position;
        Vector3 startPosition2 = block2.transform.position;
        Vector3 targetPosition = (startPosition1 + startPosition2) / 2; // Midpoint

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            //Creating animations by changing positions and scales
            block1.transform.position = Vector3.Lerp( startPosition1, targetPosition, elapsedTime / duration );
            block2.transform.position = Vector3.Lerp( startPosition2, targetPosition, elapsedTime / duration );

            block1.transform.localScale = Vector3.Lerp( startScale1, endScale1, elapsedTime / duration );
            block2.transform.localScale = Vector3.Lerp( startScale2, endScale2, elapsedTime / duration );

            yield return null;
        }
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

using System.Collections;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Coroutine _fallCoroutine;
    public BlockInfo blockInfo;

    private void Start()
    {
        blockInfo = GetComponent<BlockInfo>();
    }

    public void FallToNonOccupiedGrid( GameObject targetGrid )
    {
        if (targetGrid == null)
            return;

        if (_fallCoroutine != null)
            StopCoroutine( _fallCoroutine );

        _fallCoroutine = StartCoroutine( MoveToTargetGrid( targetGrid ) );
    }

    private IEnumerator MoveToTargetGrid( GameObject targetGrid )
    {
        float targetPosY = targetGrid.transform.position.y;

        while (Mathf.Abs( transform.position.y - targetPosY ) > 0.01f)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = Mathf.MoveTowards( transform.position.y, targetPosY, _speed * Time.deltaTime );
            transform.position = newPosition;
            yield return null;
        }
        Vector3 finalPosition = transform.position;
        finalPosition.y = targetPosY;
        transform.position = finalPosition;
        AdjustGridInfo( targetGrid );
        _fallCoroutine = null;
    }

    private void AdjustGridInfo( GameObject parentGrid )
    {
        if (parentGrid == null)
            return;
        if (blockInfo == null)
            return;
        ParentGridInfo parentGridInfo = parentGrid.GetComponent<ParentGridInfo>();
        parentGridInfo.occupyingBlock = gameObject;
        parentGridInfo.isOccupied = true;
        // Converting corner objects to an array for GridInfo to look
        var cornerObjects = blockInfo.GetCornerStates().Values;
        GameObject[] cornersArray = new GameObject[cornerObjects.Count];
        cornerObjects.CopyTo( cornersArray, 0 );
        int childCount = parentGrid.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = parentGrid.transform.GetChild( i );
            GridInfo gridInfo = child.GetComponent<GridInfo>();
            gridInfo.AdjustGridOccupyingBlock( cornersArray );
        }
    }

}

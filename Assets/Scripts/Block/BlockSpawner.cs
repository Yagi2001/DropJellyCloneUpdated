using System.Collections;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    private FillBlock _fillBlock;
    [SerializeField]
    private DragAndDrop _dragAndDrop;
    [SerializeField]
    private GameObject[] _blockPrefab;
    [SerializeField]
    private GameObject[] _colorPrefabs;

    private void Start()
    {
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        GameObject selectedBlockPrefab = _blockPrefab[Random.Range( 0, _blockPrefab.Length )];
        GameObject newBlock = Instantiate( selectedBlockPrefab, transform.position, Quaternion.identity );
        int childCount = newBlock.transform.childCount;
        int numToDestroy = Random.Range( 1, childCount );
        _fillBlock = newBlock.GetComponent<FillBlock>();
        DestroyRandomChildren( newBlock, numToDestroy );
        _dragAndDrop.block = newBlock;
    }

    private void DestroyRandomChildren( GameObject parent, int numToDestroy )
    {
        int destroyedCount = 0;

        for (int i = 0; i < numToDestroy; i++)
        {
            int currentChildCount = parent.transform.childCount;
            if (currentChildCount <= 1)
                break;

            int randomIndex = Random.Range( 0, currentChildCount );
            Transform child = parent.transform.GetChild( randomIndex );
            Destroy( child.gameObject );
            destroyedCount++;
        }
        StartCoroutine( CallCheckAndFillEmptyPieces() );
    }

    private IEnumerator CallCheckAndFillEmptyPieces()
    {
        yield return null;
        _fillBlock.CheckAndFillEmptyPieces();
    }
}

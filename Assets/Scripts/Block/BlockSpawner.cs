using System;
using System.Collections;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public static Action BlocksSettled;
    private int _activeBlockCount;
    private FillBlock _fillBlock;
    [SerializeField]
    private DragAndDrop _dragAndDrop;
    [SerializeField]
    private GameObject[] _blockPrefab;
    [SerializeField]
    private GameObject[] _colorPrefabs;
    private void OnEnable()
    {
        BlocksSettled += SpawnBlock;
        DragAndDrop.placedBlock += ReduceActiveBlock;
    }

    private void OnDisable()
    {
        BlocksSettled -= SpawnBlock;
        DragAndDrop.placedBlock -= ReduceActiveBlock;
    }
    private void Start()
    {
        _activeBlockCount = 0;
        SpawnBlock();
    }

    private void SpawnBlock()
    {
        if (_activeBlockCount <= 0)
        {
            GameObject selectedBlockPrefab = _blockPrefab[UnityEngine.Random.Range( 0, _blockPrefab.Length )];
            GameObject newBlock = Instantiate( selectedBlockPrefab, transform.position, Quaternion.identity );
            int childCount = newBlock.transform.childCount;
            int numToDestroy = UnityEngine.Random.Range( 1, childCount );
            _fillBlock = newBlock.GetComponent<FillBlock>();
            DestroyRandomChildren( newBlock, numToDestroy );
            _activeBlockCount++;
            _dragAndDrop.block = newBlock;
        }
    }

    private void DestroyRandomChildren( GameObject parent, int numToDestroy )
    {
        int destroyedCount = 0;

        for (int i = 0; i < numToDestroy; i++)
        {
            int currentChildCount = parent.transform.childCount;
            if (currentChildCount <= 1)
                break;

            int randomIndex = UnityEngine.Random.Range( 0, currentChildCount );
            Transform child = parent.transform.GetChild( randomIndex );
            Destroy( child.gameObject );
            destroyedCount++;
        }
        StartCoroutine( CallCheckAndFillEmptyPieces() );
    }

    private void ReduceActiveBlock()
    {
        _activeBlockCount--;
    }

    private IEnumerator CallCheckAndFillEmptyPieces()
    {
        yield return null;
        _fillBlock.CheckAndFillEmptyPieces();
    }
}

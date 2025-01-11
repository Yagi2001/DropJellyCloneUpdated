using System.Collections;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Coroutine _fallCoroutine;

    public void FallToNonOccupiedGrid( float targetPosY )
    {
        if (_fallCoroutine != null)
            StopCoroutine( _fallCoroutine );
        _fallCoroutine = StartCoroutine( MoveToTargetY( targetPosY ) );
    }

    private IEnumerator MoveToTargetY( float targetPosY )
    {
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
        _fallCoroutine = null;
    }
}

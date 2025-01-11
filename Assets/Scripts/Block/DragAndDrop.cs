using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private float _minX;
    private float _maxX;
    private Camera _mainCamera;
    private Vector3 _mouseOffset;
    [SerializeField]
    private Transform[] _gridGroups;
    private Transform _previousClosestGroup;
    public GameObject block;

    private void Start()
    {
        _mainCamera = Camera.main;
        _minX = Mathf.Min( GetGridGroupXPositions() );
        _maxX = Mathf.Max( GetGridGroupXPositions() );
    }

    private void OnMouseDown()
    {
        if (block != null)
        {
            _mouseOffset = block.transform.position - GetMouseWorldPosition();
        }
    }

    private void OnMouseDrag()
    {
        if (block != null)
        {
            Vector3 newPosition = block.transform.position;
            newPosition.x = GetMouseWorldPosition().x + _mouseOffset.x;
            newPosition.x = Mathf.Clamp( newPosition.x, _minX, _maxX );
            block.transform.position = newPosition;

            // Highlight the closest group while dragging
            Transform closestGroup = FindClosestGroup();
            if (closestGroup != null)
            {
                if (_previousClosestGroup != closestGroup)
                {
                    if (_previousClosestGroup != null)
                    {
                        var previousHighlight = _previousClosestGroup.GetComponent<HighlightGrids>();
                        if (previousHighlight != null)
                            previousHighlight.UnhighlightAllChildren();
                    }
                    var currentHighlight = closestGroup.GetComponent<HighlightGrids>();
                    if (currentHighlight != null)
                        currentHighlight.HighlightAllChildren();
                    _previousClosestGroup = closestGroup;
                }
            }
        }
    }

    private void OnMouseUp()
    {
        if (block != null && _previousClosestGroup != null)
        {
            Vector3 newPosition = block.transform.position;
            newPosition.x = _previousClosestGroup.position.x;
            block.transform.position = newPosition;
            var highlight = _previousClosestGroup.GetComponent<HighlightGrids>();
            BlockMovement blockMovement = block.GetComponent<BlockMovement>();
            GameObject targetGrid = FindFurthestAvailableGrid();
            blockMovement.FallToNonOccupiedGrid( targetGrid );
            if (highlight != null)
                highlight.UnhighlightAllChildren();
            _previousClosestGroup = null;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        return _mainCamera.ScreenToWorldPoint( mousePosition );
    }

    private Transform FindClosestGroup()
    {
        float minDistance = Mathf.Infinity;
        Transform closestGroup = null;

        foreach (var group in _gridGroups)
        {
            if (group == null) continue;

            float distance = Vector3.Distance( block.transform.position, group.position );
            if (distance < minDistance)
            {
                minDistance = distance;
                closestGroup = group;
            }
        }
        return closestGroup;
    }

    private float[] GetGridGroupXPositions()
    {
        float[] xPositions = new float[_gridGroups.Length];
        for (int i = 0; i < _gridGroups.Length; i++)
        {
            xPositions[i] = _gridGroups[i].position.x;
        }
        return xPositions;
    }

    private GameObject FindFurthestAvailableGrid()
    {
        if (_previousClosestGroup == null)
            return null;
        GameObject furthestGrid = null;
        float minY = Mathf.Infinity;

        for (int i = 0; i < _previousClosestGroup.childCount; i++)
        {
            Transform child = _previousClosestGroup.GetChild( i );
            ParentGridInfo gridInfo = child.GetComponent<ParentGridInfo>();
            if (gridInfo != null && !gridInfo.isOccupied)
            {
                if (child.position.y < minY)
                {
                    minY = child.position.y;
                    furthestGrid = child.gameObject;
                }
            }
        }
        return furthestGrid;
    }
}

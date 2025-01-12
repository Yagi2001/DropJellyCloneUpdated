using System.Collections.Generic;
using UnityEngine;

public class FillBlock : MonoBehaviour
{
    private BlockInfo _blockInfo;

    private enum Corner
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    private void Start()
    {
        _blockInfo = GetComponent<BlockInfo>();
    }

    public void CheckAndFillEmptyPieces()
    {
        Debug.Log( "Called" );
        var corners = GetCorners();
        var nullCorners = GetNullCorners( corners );

        // If all corners are null, destroy the game object
        if (nullCorners.Count == 4)
        {
            Destroy( gameObject );
            return;
        }

        foreach (var missingCorner in nullCorners)
        {
            // Prioritize vertical growth
            if (!TryFillCorner( missingCorner, corners, prioritizeVertical: true ))
            {
                // Horizontal Growth
                TryFillCorner( missingCorner, corners, prioritizeVertical: false );
            }
        }
    }

    private Dictionary<Corner, GameObject> GetCorners()
    {
        return new Dictionary<Corner, GameObject>
        {
            { Corner.TopLeft, _blockInfo.topLeft },
            { Corner.TopRight, _blockInfo.topRight },
            { Corner.BottomLeft, _blockInfo.bottomLeft },
            { Corner.BottomRight, _blockInfo.bottomRight }
        };
    }

    private List<Corner> GetNullCorners( Dictionary<Corner, GameObject> corners )
    {
        var nullCorners = new List<Corner>();
        foreach (var corner in corners)
        {
            if (corner.Value == null)
                nullCorners.Add( corner.Key );
        }
        return nullCorners;
    }

    private bool TryFillCorner( Corner missingCorner, Dictionary<Corner, GameObject> corners, bool prioritizeVertical )
    {
        var primaryPartner = prioritizeVertical ? GetVerticalPartner( missingCorner ) : GetHorizontalPartner( missingCorner );
        var secondaryPartner = prioritizeVertical ? GetHorizontalPartner( missingCorner ) : GetVerticalPartner( missingCorner );
        if (corners[primaryPartner] != null && AdjustGrowth( corners[primaryPartner], prioritizeVertical ))
        {
            UpdateBlockInfoReference( missingCorner, corners[primaryPartner] );
            return true;
        }
        if (corners[secondaryPartner] != null && AdjustGrowth( corners[secondaryPartner], !prioritizeVertical ))
        {
            UpdateBlockInfoReference( missingCorner, corners[secondaryPartner] );
            return true;
        }
        return false;
    }

    private bool AdjustGrowth( GameObject obj, bool vertical )
    {
        var scale = obj.transform.localScale;
        if ((vertical ? scale.y : scale.x) < 0.75f)
        {
            var position = obj.transform.localPosition;
            if (vertical) position.y = 0f; else position.x = 0f;
            obj.transform.localPosition = position;

            if (vertical) scale.y = 1f; else scale.x = 1f;
            obj.transform.localScale = scale;

            return true;
        }
        return false;
    }

    private Corner GetVerticalPartner( Corner corner ) =>
        corner == Corner.TopLeft || corner == Corner.TopRight
            ? corner == Corner.TopLeft ? Corner.BottomLeft : Corner.BottomRight
            : corner == Corner.BottomLeft ? Corner.TopLeft : Corner.TopRight;

    private Corner GetHorizontalPartner( Corner corner ) =>
        corner == Corner.TopLeft || corner == Corner.BottomLeft
            ? corner == Corner.TopLeft ? Corner.TopRight : Corner.BottomRight
            : corner == Corner.TopRight ? Corner.TopLeft : Corner.BottomLeft;

    private void UpdateBlockInfoReference( Corner corner, GameObject newObject )
    {
        switch (corner)
        {
            case Corner.TopLeft:
                _blockInfo.topLeft = newObject;
                break;
            case Corner.TopRight:
                _blockInfo.topRight = newObject;
                break;
            case Corner.BottomLeft:
                _blockInfo.bottomLeft = newObject;
                break;
            case Corner.BottomRight:
                _blockInfo.bottomRight = newObject;
                break;
        }
    }
}

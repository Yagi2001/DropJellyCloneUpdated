using System.Collections;
using UnityEngine;

public class HighlightGrids : MonoBehaviour
{
    public void HighlightAllChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild( i );
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
                childRenderer.material.color = Color.red;
        }
    }

    public void UnhighlightAllChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild( i );
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
                childRenderer.material.color = Color.white;
        }
    }
}

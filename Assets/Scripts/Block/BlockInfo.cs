using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    public GameObject topLeft;
    public GameObject topRight;
    public GameObject bottomLeft;
    public GameObject bottomRight;

    private Dictionary<string, GameObject> corners;

    private void Start()
    {
        corners = new Dictionary<string, GameObject>
        {
            { "topLeft", topLeft },
            { "topRight", topRight },
            { "bottomLeft", bottomLeft },
            { "bottomRight", bottomRight }
        };
    }

    public Dictionary<string, GameObject> GetCornerStates()
    {
        return corners;
    }
}

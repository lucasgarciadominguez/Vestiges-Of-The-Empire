using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTilerExtractiveStructures : MonoBehaviour
{
    [SerializeField]
    Material materialSquareHeatMaps;
    public void SetTileSize(int size)
    {
        materialSquareHeatMaps.mainTextureScale = new Vector2(size,size);

    }
    public Material ReturnMaterialSquareHeatMaps()
    {
        return materialSquareHeatMaps;
    }
}

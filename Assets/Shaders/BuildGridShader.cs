using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGridShader : MonoBehaviour
{
    [SerializeField]
    Material gridBuildMaterial;

    public void SetSizeBuildGridShader(int sizeGrid)
    {
        Vector2 size = new Vector2(sizeGrid, sizeGrid);
        gridBuildMaterial.SetVector("_DefaultScale", size);

    }
    public Material ReturnGridMaterialShader()
    {
        return gridBuildMaterial;
    }
}

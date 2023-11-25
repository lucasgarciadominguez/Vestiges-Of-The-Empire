using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class WorldTexture : MonoBehaviour
{
    [SerializeField]
    private Material materialgrass;
    [SerializeField]
    private Material materialwater;
    [SerializeField]
    private Material materialEdge;
    [SerializeField]
    private Texture2D grassTexture2D;
    public void DrawTextureTerrainGrass(Cell[,] grid,int size,GameObject grassGO, float[,] noisemap)
    {
        Texture2D texture = new Texture2D(size, size);  //create a texture2d of the dimensions of the size
        Color[] colorMap = new Color[size * size];
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, z];

                if (!cell.isWater)
                {
                    if (noisemap[x, z] >0.66f)
                    {
                        colorMap[z * size + x] = new Color(0.5828587f, 0.6981132f, 0.6116722f, 1);  //variation of green


                    }
                    else if (noisemap[x, z] > 0.43f)
                    {
                        colorMap[z * size + x] = new Color(0.5217604f, 0.6320754f, .5532789f, 1);  //variation of green

                    }
                    else
                    {
                        colorMap[z * size + x] = new Color(0.490477f, 0.6226414f, 0.5235181f, 1);  //variation of green

                    }
                }
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();
        MeshRenderer meshRenderer = grassGO.GetComponent<MeshRenderer>();
        meshRenderer.material = materialgrass;
        meshRenderer.material.mainTexture = texture;
        materialgrass = meshRenderer.material;
    }
    public void DrawTextureTerrainWater(Cell[,] grid,int size,GameObject waterGO)
    {
        Texture2D texture = new Texture2D(size, size);  //create a texture2d of the dimensions of the size
        Color[] colorMap = new Color[size * size];
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, z];

                if (cell.isWater)
                {
                    colorMap[z * size + x] = Color.blue;
                }
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply(true);
        MeshRenderer meshRenderer = waterGO.GetComponent<MeshRenderer>();
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.material = materialwater;
        meshRenderer.material.mainTexture = texture;
        materialwater = meshRenderer.material;
    }
    public void DrawTextureTerrainEdgeSand(GameObject edgeObj)
    {
        edgeObj.GetComponent<MeshRenderer>().material = materialEdge;


    }
    public Material ReturnMaterialGrass()
    {
        return materialgrass;
    }

}

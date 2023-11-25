using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ExtractiveResourcesHeatMap
{
    [SerializeField]
    public ExtractiveResources nameNoiseMap;
    [SerializeField]
    private float seed;
    public float[,] noiseMapHeatMap { get; private set; }
    public int[,] noiseMapValuesInt { get; private set; }
    [SerializeField]
    private Material material;

    public void FillNoiseMap(int size, Vector2 Seed)
    {
        noiseMapHeatMap = new float[size, size];

        for (int z = 0; z < size; z++)
        {
            Seed.y = seed + z;
            for (int x = 0; x < size; x++)
            {
                Seed.x = seed + x;
                float noiseValue = Mathf.PerlinNoise(((float)x + Seed.x) / size, ((float)z + Seed.y) / size);

                noiseMapHeatMap[x, z] = noiseValue;
            }
        }
    }

    public void ChangeMaterial(Texture2D texture, Color[] colorMap)
    {

    }
    public void CreateHeatMap(int size, Texture2D heatMapTexture)
    {
        noiseMapValuesInt = new int[size, size];
        Texture2D texture = new Texture2D(size, size);  //create a texture2d of the dimensions of the size
        Color[] colorMap = new Color[size * size];
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                float heatMapValue = noiseMapHeatMap[x, z] * 100;

                Color a;
                if (heatMapValue > 50)
                {
                    a = heatMapTexture.GetPixel(50, 0); //sets a value standard of efectiviness of the heatmap
                    noiseMapValuesInt[x, z] = 50;
                }
                else
                {
                    a = heatMapTexture.GetPixel((int)heatMapValue, 0);
                    noiseMapValuesInt[x, z] = (int)heatMapValue;
                }

                colorMap[z * size + x] = a;  //applies the colors
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply(true);
        material.mainTexture = texture;
        ChangeMaterial(texture, colorMap);
    }
    public Material GetMaterial()
    {
        return material;
    }
    public int ReturnNoiseValueForCellValues(int x, int z)
    {
        return noiseMapValuesInt[x, z];
    }
}

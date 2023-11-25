using System;
using UnityEngine;

[Serializable]
public struct StructurePrefabWeighted   //structure for the prefabs of the type of houses
{
    public GameObject prefab;
    [Range(0, 1)]
    public float weight;
}


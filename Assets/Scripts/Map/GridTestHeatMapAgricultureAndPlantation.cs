using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GridTestHeatMapAgricultureAndPlantation : MonoBehaviour
{
    public GameObject[] treePrefabs;
    public ShaderGrid heatmap;
    public Material materialgrass;
    public Texture2D heatMapTexture;
    public Material materialHeatMap;
    public Texture2D textureSquare;

    public Material materialwater;
    public Material materialRock;

    [SerializeField]
    private GameObject grassGO;

    [SerializeField]
    private GameObject waterGO;

    [SerializeField]
    private GameObject edgeObj;

    private GameObject rockGO;
    public float treeNoiseScale = 0.05f;
    public float treeDensity = 0.5f;
    public Vector2 Seed;
    public float[] seed;

    public Material edgeTerrain;

    [SerializeField]
    public int size = 50;

    [SerializeField]
    private int distanceTile = 1;

    public CellTestHeatMap[,] gridArray;  //array bidimensional de casillas
    public List<ExtractiveResourcesHeatMap> heatMapsItems;
    private float[,] noiseMap;
    public float waterLevel = 0.7f;
    public List<Vector3> AdjacentCells;
    public List<GameObject> trees = new List<GameObject>();

    [SerializeField]
    private int terrainLayerNum;

    // Start is called before the first frame update
    private void Awake()
    {
        GenerateProceduralTerrain();
    }

    public void GenerateProceduralTerrain()
    {
        this.gridArray = new CellTestHeatMap[size, size];
        noiseMap = new float[size, size];

        for (int i = 0; i < heatMapsItems.Count; i++)
        {
            heatMapsItems[i].FillNoiseMap(size, Seed);
        }
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * 0.1f, z * 0.1f);
                noiseMap[x, z] = noiseValue;
            }
        }
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValueCell = noiseMap[x, z];
                if (noiseValueCell < waterLevel)
                {
                    bool isWater = false;
                    CellTestHeatMap cell = new CellTestHeatMap(this, x, z, isWater, false, CellType.Empty);
                    gridArray[x, z] = cell;
                }
                else
                {
                    bool isWater = true;
                    Debug.Log("hoooo");

                    CellTestHeatMap cell = new CellTestHeatMap(this, x, z, isWater, false, CellType.Empty);
                    gridArray[x, z] = cell;
                }
            }
        }
        DrawGrass(gridArray);
        DrawWater(gridArray);
        DrawTextureTerrainGrass(gridArray);
        DrawTextureTerrainWater(gridArray);
        DrawHeatMaps();
        foreach (var item in gridArray)
        {
            if (item.isWater)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.position = new Vector3(item.x, 0, item.z);
                // go.transform.localScale = new Vector3(item.transform.localScale.x * .5f, item.transform.localScale.y * .5f, item.transform.localScale.z * .5f);
                go.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }

    private void DrawHeatMaps()
    {
        foreach (ExtractiveResourcesHeatMap map in heatMapsItems)
        {
            map.CreateHeatMap(size, heatMapTexture);
        }
    }

    private void DrawWater(CellTestHeatMap[,] grid)
    {
        //make a list of vertices, triangkles and uvs instead of an array because you don't know how much wiil be stored
        Mesh mesh = new Mesh(); //single mesh for the entire grid
        List<Vector3> vertices = new List<Vector3>();    //list with vertexs that are used for knowing the 3d coordinates of each point
        List<int> triangles = new List<int>();    //index of all the triangles who are in all the mesh
        List<Vector2> uvs = new List<Vector2>();  //coordinates 2d for knowing where to display the texture
        int quadId = 0;
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                CellTestHeatMap cell = grid[x, z];
                if (cell.isWater)
                {
                    CreateQuad(x, -0.1f, z, vertices, triangles, uvs, ref quadId);
                }
            }
        }
        mesh.vertices = vertices.ToArray(); //you could store all the vertices/triangles/uvs you wanted due to the decision of making it a list. Now that is over the storage, you convert
        mesh.SetTriangles(triangles.ToArray(), 0);
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        //add meshfilyer, meshrenderer and meshcollider to the gameobject grid
        //waterGO = new GameObject("Water");
        //waterGO.transform.SetParent(this.transform);
        MeshFilter meshFilter = waterGO.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh; //add mesh to the meshfilter
        MeshRenderer meshRenderer = waterGO.AddComponent<MeshRenderer>();
        MeshCollider collider = waterGO.AddComponent<MeshCollider>();    //TODO decide if i am going to make it 2D
        collider.sharedMesh = mesh; //add the mesh shape created to the meshcollider
        waterGO.layer = terrainLayerNum;
    }

    public void DrawGrass(CellTestHeatMap[,] grid)
    {
        //make a list of vertices, triangkles and uvs instead of an array because you don't know how much wiil be stored
        Mesh mesh = new Mesh(); //single mesh for the entire grid
        List<Vector3> vertices = new List<Vector3>();    //list with vertexs that are used for knowing the 3d coordinates of each point
        List<int> triangles = new List<int>();    //index of all the triangles who are in all the mesh
        List<Vector2> uvs = new List<Vector2>();  //coordinates 2d for knowing where to display the texture
        int quadId = 0;

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                CellTestHeatMap cell = grid[x, z];
                if (!cell.isWater)
                {
                    CreateQuad(x, 0, z, vertices, triangles, uvs, ref quadId);
                }
            }
        }
        mesh.vertices = vertices.ToArray(); //you could store all the vertices/triangles/uvs you wanted due to the decision of making it a list. Now that is over the storage, you convert
        mesh.triangles = triangles.ToArray();   //it to an array that is what is needed for the mesh.vertices/triangles/uv.
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        //add meshfilyer, meshrenderer and meshcollider to the gameobject grid
        //grassGO = new GameObject("Grass");
        //grassGO.transform.SetParent(this.transform);
        MeshFilter meshFilter = grassGO.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh; //add mesh to the meshfilter
        MeshRenderer meshRenderer = grassGO.AddComponent<MeshRenderer>();
        MeshCollider collider = grassGO.AddComponent<MeshCollider>();    //TODO decide if i am going to make it 2D
        collider.sharedMesh = mesh; //add the mesh shape created to the meshcollider
        grassGO.gameObject.layer = terrainLayerNum;
    }

    public void DrawTextureTerrainWater(CellTestHeatMap[,] grid)
    {
        Texture2D texture = new Texture2D(size, size);  //create a texture2d of the dimensions of the size
        Color[] colorMap = new Color[size * size];
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                CellTestHeatMap cell = grid[x, z];

                if (cell.isWater)
                {
                    colorMap[z * size + x] = Color.blue;
                }
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();
        MeshRenderer meshRenderer = waterGO.GetComponent<MeshRenderer>();
        meshRenderer.material = materialwater;
        meshRenderer.material.mainTexture = texture;
        materialwater = meshRenderer.material;
    }

    public void DrawTextureTerrainGrass(CellTestHeatMap[,] grid)
    {
        Texture2D texture = new Texture2D(size, size);  //create a texture2d of the dimensions of the size
        Color[] colorMap = new Color[size * size];
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                CellTestHeatMap cell = grid[x, z];

                if (!cell.isWater)
                {
                    colorMap[z * size + x] = Color.green;
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

    // Update is called once per frame
    private void CreateQuad(int x, float y, int z, List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, ref int indexOffset)
    {
        vertices.Add(new Vector3(x, y, z));
        vertices.Add(new Vector3(x + 1, y, z));
        vertices.Add(new Vector3(x, y, z + 1));
        vertices.Add(new Vector3(x + 1, y, z + 1));

        triangles.Add(indexOffset + 0);
        triangles.Add(indexOffset + 3);
        triangles.Add(indexOffset + 1);
        triangles.Add(indexOffset + 0);
        triangles.Add(indexOffset + 2);
        triangles.Add(indexOffset + 3);

        uvs.Add(new Vector2(x / (float)size, z / (float)size));
        uvs.Add(new Vector2((x + 1) / (float)size, z / (float)size));
        uvs.Add(new Vector2(x / (float)size, (z + 1) / (float)size));
        uvs.Add(new Vector2((x + 1) / (float)size, (z + 1) / (float)size));

        indexOffset += 4;
    }

    public void ChangeHeatMap(int i)
    {
        switch (i)
        {
            case 0:
                grassGO.GetComponent<MeshRenderer>().material = heatMapsItems[0].GetMaterial();
                break;

            case 1:
                grassGO.GetComponent<MeshRenderer>().material = heatMapsItems[1].GetMaterial();
                break;

            case 2:
                grassGO.GetComponent<MeshRenderer>().material = heatMapsItems[2].GetMaterial();
                break;

            case 3:
                grassGO.GetComponent<MeshRenderer>().material = heatMapsItems[3].GetMaterial();
                break;

            case 4:
                grassGO.GetComponent<MeshRenderer>().material = heatMapsItems[4].GetMaterial();
                break;

            default:
                break;
        }
    }
}
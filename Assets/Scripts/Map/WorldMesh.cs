using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class WorldMesh : MonoBehaviour
{
    [SerializeField]
    private GameObject grassGO;

    [SerializeField]
    private GameObject waterGO;

    [SerializeField]
    private GameObject edgeObj;


    [SerializeField]
    private int terrainLayerNum;
    [SerializeField]
    private int distance;
    public void DrawCLiffsLimitMap(Cell[,] grid, int size, List<GameObject> cliffs,GameObject cliff,GameObject cliffCorner)
    {
        Vector3 rotation = new Vector3(0, -90, 0);
        for (int i = 0; i < size; i++)
        {

            if (i==0)
            {
                Instantiate(cliff, new Vector3(i, 0, 0), Quaternion.identity, this.transform);

            }
            else
            {
                Instantiate(cliff, new Vector3(i += distance, 0, 0), Quaternion.identity, this.transform);

            }


        }
        for (int i = 0; i < size; i++)
        {

            if (i == 0)
            {
                Instantiate(cliff, new Vector3(i, 0, size), Quaternion.identity, this.transform);

            }
            else
            {
                Instantiate(cliff, new Vector3(i += distance, 0, size), Quaternion.identity, this.transform);

            }


        }
        for (int i = 0; i < size; i++)
        {
            if (i == 0)
            {
                Instantiate(cliff, new Vector3(0, 0, i), Quaternion.Euler( rotation), this.transform);

            }
            else
            {
                Instantiate(cliff, new Vector3(0, 0, i += distance), Quaternion.Euler(rotation), this.transform);

            }



        }
        for (int i = 0; i < size; i++)
        {
            if (i==0)
            {
                Instantiate(cliff, new Vector3(size, 0, i), Quaternion.Euler(rotation), this.transform);

            }
            else
            {
                Instantiate(cliff, new Vector3(size, 0, i += distance), Quaternion.Euler(rotation), this.transform);

            }



        }
    }
    public void DrawGrass(Cell[,] grid,int size)
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
                Cell cell = grid[x, z];
                if (!cell.isWater)
                {
                    CreateQuad(x, 0, z, vertices, triangles, uvs, ref quadId,size);
                }
            }
        }

        mesh.vertices = vertices.ToArray(); //you could store all the vertices/triangles/uvs you wanted due to the decision of making it a list. Now that is over the storage, you convert
        mesh.triangles = triangles.ToArray();   //it to an array that is what is needed for the mesh.vertices/triangles/uv.
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        if (!grassGO.GetComponent<MeshFilter>() && !grassGO.GetComponent<MeshRenderer>() && !grassGO.GetComponent<MeshCollider>())
        {
            grassGO.AddComponent<MeshFilter>();
            grassGO.AddComponent<MeshRenderer>();
            grassGO.AddComponent<MeshCollider>();
        }
        grassGO.GetComponent<MeshFilter>().mesh.Clear();
        grassGO.GetComponent<MeshFilter>().mesh = mesh; //add mesh to the meshfilter
        grassGO.GetComponent<MeshCollider>().sharedMesh = mesh; //add the mesh shape created to the meshcollider
        grassGO.gameObject.layer = terrainLayerNum;
    }
    public void DrawWater(Cell[,] grid,int size)
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
                Cell cell = grid[x, z];
                if (cell.isWater)
                {
                    CreateQuad(x, -0.1f, z, vertices, triangles, uvs, ref quadId,size);
                }
            }
        }
        mesh.vertices = vertices.ToArray(); //you could store all the vertices/triangles/uvs you wanted due to the decision of making it a list. Now that is over the storage, you convert
        mesh.triangles = triangles.ToArray();   //it to an array that is what is needed for the mesh.vertices/triangles/uv.
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        if (!waterGO.GetComponent<MeshFilter>() && !waterGO.GetComponent<MeshRenderer>() && !waterGO.GetComponent<MeshCollider>())
        {
            waterGO.AddComponent<MeshFilter>();
            waterGO.AddComponent<MeshRenderer>();
            waterGO.AddComponent<MeshCollider>();
        }
        waterGO.GetComponent<MeshFilter>().mesh.Clear();
        waterGO.GetComponent<MeshFilter>().mesh = mesh; //add mesh to the meshfilter
        waterGO.GetComponent<MeshCollider>().sharedMesh = mesh; //add the mesh shape created to the meshcollider
        waterGO.gameObject.layer = terrainLayerNum;
    }
    public void DrawEdgeMesh(Cell[,] grid,int size)
    {
        //the first vertex and the last one are changed between them
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        int quadId = 0;
        List<Vector2> uvs = new List<Vector2>();  //coordinates 2d for knowing where to display the texture
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, z];
                if (!cell.isWater)
                {
                    if (x > 0)
                    {
                        Cell left = grid[x - 1, z];
                        if (left.isWater)
                        {
                            vertices.Add(new Vector3(x, -1, z));
                            vertices.Add(new Vector3(x, 0, z));
                            vertices.Add(new Vector3(x, -1, z + 1));
                            vertices.Add(new Vector3(x, 0, z + 1));

                            triangles.Add(quadId + 0);
                            triangles.Add(quadId + 3);
                            triangles.Add(quadId + 1);
                            triangles.Add(quadId + 0);
                            triangles.Add(quadId + 2);
                            triangles.Add(quadId + 3);
                            uvs.Add(new Vector2(x / (float)size, z / (float)size));
                            uvs.Add(new Vector2((x + 1) / (float)size, z / (float)size));
                            uvs.Add(new Vector2(x / (float)size, (z + 1) / (float)size));
                            uvs.Add(new Vector2((x + 1) / (float)size, (z + 1) / (float)size));

                            quadId += 4;
                        }
                    }
                    if (x < size - 1)
                    {
                        Cell right = grid[x + 1, z];
                        if (right.isWater)
                        {
                            vertices.Add(new Vector3(x + 1, -1, z + 1));
                            vertices.Add(new Vector3(x + 1, 0, z + 1));
                            vertices.Add(new Vector3(x + 1, -1, z));
                            vertices.Add(new Vector3(x + 1, 0, z));

                            triangles.Add(quadId + 0);
                            triangles.Add(quadId + 3);
                            triangles.Add(quadId + 1);
                            triangles.Add(quadId + 0);
                            triangles.Add(quadId + 2);
                            triangles.Add(quadId + 3);

                            uvs.Add(new Vector2(x / (float)size, z / (float)size));
                            uvs.Add(new Vector2((x + 1) / (float)size, z / (float)size));
                            uvs.Add(new Vector2(x / (float)size, (z + 1) / (float)size));
                            uvs.Add(new Vector2((x + 1) / (float)size, (z + 1) / (float)size));

                            quadId += 4;
                        }
                    }
                    if (z > 0)
                    {
                        Cell down = grid[x, z - 1];
                        if (down.isWater)
                        {
                            vertices.Add(new Vector3(x + 1, -1, z));
                            vertices.Add(new Vector3(x + 1, 0, z));
                            vertices.Add(new Vector3(x, -1, z));
                            vertices.Add(new Vector3(x, 0, z));

                            triangles.Add(quadId + 0);
                            triangles.Add(quadId + 3);
                            triangles.Add(quadId + 1);
                            triangles.Add(quadId + 0);
                            triangles.Add(quadId + 2);
                            triangles.Add(quadId + 3);

                            uvs.Add(new Vector2(x / (float)size, z / (float)size));
                            uvs.Add(new Vector2((x + 1) / (float)size, z / (float)size));
                            uvs.Add(new Vector2(x / (float)size, (z + 1) / (float)size));
                            uvs.Add(new Vector2((x + 1) / (float)size, (z + 1) / (float)size));

                            quadId += 4;
                        }
                    }
                    if (z < size - 1)
                    {
                        Cell up = grid[x, z + 1];
                        if (up.isWater)
                        {
                            vertices.Add(new Vector3(x, -1, z + 1));
                            vertices.Add(new Vector3(x, 0, z + 1));
                            vertices.Add(new Vector3(x + 1, -1, z + 1));
                            vertices.Add(new Vector3(x + 1, 0, z + 1));

                            triangles.Add(quadId + 0);
                            triangles.Add(quadId + 3);
                            triangles.Add(quadId + 1);
                            triangles.Add(quadId + 0);
                            triangles.Add(quadId + 2);
                            triangles.Add(quadId + 3);

                            uvs.Add(new Vector2(x / (float)size, z / (float)size));
                            uvs.Add(new Vector2((x + 1) / (float)size, z / (float)size));
                            uvs.Add(new Vector2(x / (float)size, (z + 1) / (float)size));
                            uvs.Add(new Vector2((x + 1) / (float)size, (z + 1) / (float)size));

                            quadId += 4;
                        }
                    }
                }
            }
        }
        mesh.vertices = vertices.ToArray(); //you could store all the vertices/triangles/uvs you wanted due to the decision of making it a list. Now that is over the storage, you convert
        mesh.triangles = triangles.ToArray();   //it to an array that is what is needed for the mesh.vertices/triangles/uv.
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        if (!edgeObj.GetComponent<MeshFilter>() && !edgeObj.GetComponent<MeshRenderer>() && !edgeObj.GetComponent<MeshCollider>())
        {
            edgeObj.AddComponent<MeshFilter>();
            edgeObj.AddComponent<MeshRenderer>();
            edgeObj.AddComponent<MeshCollider>();
        }
        edgeObj.GetComponent<MeshFilter>().mesh.Clear();
        edgeObj.GetComponent<MeshFilter>().mesh = mesh; //add mesh to the meshfilter
        edgeObj.GetComponent<MeshCollider>().sharedMesh = mesh; //add the mesh shape created to the meshcollider
        edgeObj.gameObject.layer = terrainLayerNum;
    }
    public void DrawTrees(Cell[,] grid,int size, float[,] noiseMap,List<GameObject> trees,float treeNoiseScale,float treeDensity, GameObject[] treePrefabs)
    {
        foreach (var item in trees)
        {
            Destroy(item.gameObject);
        }
        trees.Clear();
        noiseMap = new float[size, size];

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * treeNoiseScale * 0.1f, z * treeNoiseScale * 0.1f);
                noiseMap[x, z] = noiseValue;
            }
        }
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, z];
                if (!cell.isWater)
                {
                    float v = UnityEngine.Random.Range(0f, treeDensity);
                    if (noiseMap[x, z] < v)
                    {
                        GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                        GameObject tree = Instantiate(prefab, transform);
                        tree.transform.position = new Vector3(x, 0, z);
                        tree.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
                        tree.transform.localScale = Vector3.one * Random.Range(.8f, 1.2f);
                        trees.Add(tree);
                    }
                }
            }
        }
    }
    public void DrawRocks(Cell[,] grid,int size,float[,] noiseMap,List<GameObject> rocks,float rockNoiseScale,float rockDensity, GameObject[] rockPrefabs)
    {
        foreach (var item in rocks)
        {
            Destroy(item.gameObject);
        }
        rocks.Clear();
        noiseMap = new float[size, size];

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * rockNoiseScale * 0.1f, z * rockNoiseScale * 0.1f);
                noiseMap[x, z] = noiseValue;
            }
        }
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, z];
                if (!cell.isWater)
                {
                    float v = UnityEngine.Random.Range(0f, rockDensity);
                    if (noiseMap[x, z] < v)
                    {
                        GameObject prefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
                        GameObject rock = Instantiate(prefab, transform);
                        rock.transform.position = new Vector3(x, 0, z);
                        rock.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
                        rock.transform.localScale = Vector3.one * Random.Range(.8f, 1.2f);
                        rocks.Add(rock);
                    }
                }
            }
        }
    }
    public void DrawBush(Cell[,] grid, int size, float[,] noiseMap, List<GameObject> bushes, float bushNoiseScale, float bushDensity, GameObject[] bushPrefabs)
    {
        foreach (var item in bushes)
        {
            Destroy(item.gameObject);
        }
        bushes.Clear();
        noiseMap = new float[size, size];

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * bushNoiseScale * 0.1f, z * bushNoiseScale * 0.1f);
                noiseMap[x, z] = noiseValue;
            }
        }
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, z];
                if (!cell.isWater)
                {
                    float v = UnityEngine.Random.Range(0f, bushDensity);
                    if (noiseMap[x, z] < v)
                    {
                        GameObject prefab = bushPrefabs[Random.Range(0, bushPrefabs.Length)];
                        GameObject brush = Instantiate(prefab, transform);
                        brush.transform.position = new Vector3(x, 0, z);
                        brush.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
                        brush.transform.localScale = Vector3.one * Random.Range(.8f, 1.2f);
                        bushes.Add(brush);
                    }
                }
            }
        }
    }
    private void CreateQuad(int x, float y, int z, List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, ref int indexOffset,int size)
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
    public GameObject ReturnGrassGO()
    {
        return grassGO;
    }
    public GameObject ReturnWaterGO()
    {
        return waterGO;
    }
    public GameObject ReturnEdgeSandGO()
    {
        return edgeObj;
    }
}

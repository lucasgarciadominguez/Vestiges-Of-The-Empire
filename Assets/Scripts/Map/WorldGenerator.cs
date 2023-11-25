using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using Pathfinding;
using Unity.VisualScripting;
/// <summary>
/// LUCAS GARCIA DOMINGUEZ
/// </summary>
public enum ExtractiveResources
{
    None,
    Cow,
    Oil,
    Pig,
    Wheat,
    Grape,
    Linen,
    Gold,
    Iron,
    Marble,
    Silver
}

public class WorldGenerator : MonoBehaviour
{
    private AutoTilerExtractiveStructures autoTilerExtractiveStructures;
    private WorldMesh worldMesh;
    private WorldTexture worldTexture;
    public int size;
    public GameObject[] treePrefabs;
    public GameObject[] rockPrefabs;
    public GameObject[] bushesPrefabs;

    [SerializeField]
    private GameObject cliff;

    [SerializeField]
    private GameObject cliffCorner;

    public ShaderGrid heatmap;
    private Material materialMineralHeatMap;
    public Texture2D heatMapTexture;
    public List<ExtractiveResourcesHeatMap> heatMapsItems;

    [SerializeField]
    public Vector2 Seed { get; private set; }

    [SerializeField]
    [Range(0, 1)]
    private float treeNoiseScale = 0.05f;
    [SerializeField]
    [Range(0, 1)]
    float riverNoiseScale;

    [SerializeField]
    int rivers = 5;

    [SerializeField]
    [Range(0, 1)]
    private float treeDensity = 0.5f;

    [SerializeField]
    [Range(0, 1)]
    private float rockNoiseScale = 0.01f;

    [SerializeField]
    [Range(0, 1)]
    private float rockDensity = 0.05f;

    [SerializeField]
    private float distanceTile;

    public Cell[,] gridArray;  //array bidimensional de casillas
    private float[,] noiseMap;

    [SerializeField]
    private float seedMesh;

    [SerializeField]
    [Range(0, 1)]
    private float waterLevel = 0.7f;

    private List<Vector3> AdjacentCells;
    public List<GameObject> trees { get; private set; } = new List<GameObject>();
    private List<GameObject> rocks = new List<GameObject>();
    private List<GameObject> bushes = new List<GameObject>();
    private List<GameObject> cliffs = new List<GameObject>();
    private bool check = true;

    private void Awake()
    {
        autoTilerExtractiveStructures = GetComponent<AutoTilerExtractiveStructures>();
        worldMesh = GetComponent<WorldMesh>();
        worldTexture = GetComponent<WorldTexture>();
        GenerateProceduralTerrain();
    }

    public void GenerateProceduralTerrain()
    {
        if (check)
        {
            check = false;
        }
        else
        {
            this.gridArray = null;
            noiseMap = null;
            heatMapsItems.Clear();
        }

        this.gridArray = new Cell[size, size];
        noiseMap = new float[size, size];

        for (int i = 0; i < heatMapsItems.Count; i++)
        {
            heatMapsItems[i].FillNoiseMap(size, Seed);

        }
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x*0.1f  + xOffset, z*0.1f + yOffset);
                noiseMap[x, z] = noiseValue;
            }
        }
        float[,] falloffMap = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValueCell = noiseMap[x, z];
                noiseValueCell -= falloffMap[x, z];
                bool isWater = noiseValueCell < waterLevel;
                Cell cell = new Cell(this, x, z, isWater, false, CellType.Empty);
                gridArray[x, z] = cell;


            }
        }
        GenerateRivers(this.gridArray);
        worldMesh.DrawGrass(gridArray, size);
        worldMesh.DrawWater(gridArray, size);
        worldMesh.DrawEdgeMesh(gridArray, size);
        worldTexture.DrawTextureTerrainGrass(gridArray, size, worldMesh.ReturnGrassGO(),noiseMap);
        worldTexture.DrawTextureTerrainWater(gridArray, size, worldMesh.ReturnWaterGO());
        worldTexture.DrawTextureTerrainEdgeSand(worldMesh.ReturnEdgeSandGO());
        worldMesh.DrawTrees(gridArray, size, noiseMap, trees, treeNoiseScale, treeDensity, treePrefabs);
        worldMesh.DrawRocks(gridArray, size, noiseMap, rocks, rockNoiseScale, rockDensity, rockPrefabs);
       // worldMesh.DrawBush(gridArray, size, noiseMap, bushes, bushNoiseScale, bushDensity, bushesPrefabs);
        DrawHeatMaps();
        AddHeatMapsValuesToCells();
        SetCornersWater();
        autoTilerExtractiveStructures.SetTileSize(size);
        //List<Vector3> listPath = GridSearchIA.AStarSearchNormalGridForPeople(this, new Vector3(1, 0, 1), new Vector3(3, 0, 3));
        //foreach (var item in listPath)
        //{
        //    Debug.Log(item.x + "," + item.z);
        //    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    go.transform.position = item;
        //}
    }
    void AddHeatMapsValuesToCells()
    {
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                if (!gridArray[x,z].isWater)
                {
                    for (int i = 0; i < heatMapsItems.Count; i++)
                    {
                        int valueHeatMap = heatMapsItems[i].ReturnNoiseValueForCellValues(x, z);
                        gridArray[x, z].ChangeValueHeatMap(heatMapsItems[i].nameNoiseMap, valueHeatMap);
                    }
                }
                else
                {
                    if (gridArray[x,z].ReturnIsCorner())
                    {
                        for (int i = 0; i < heatMapsItems.Count; i++)
                        {
                            int valueHeatMap = heatMapsItems[i].ReturnNoiseValueForCellValues(x, z);
                            gridArray[x, z].ChangeValueHeatMap(heatMapsItems[i].nameNoiseMap, valueHeatMap);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < heatMapsItems.Count; i++)
                        {
                            int valueHeatMap = 0;
                            gridArray[x, z].ChangeValueHeatMap(heatMapsItems[i].nameNoiseMap, valueHeatMap);
                        }
                    }


                }
            }
        }
    }
    void GenerateRivers(Cell[,] grid)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * riverNoiseScale + xOffset, y * riverNoiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        GridGraph gg = AstarData.active.graphs[0] as GridGraph;
        gg.center = new Vector3(size / 2f - .5f, 0, size / 2f - .5f);
        gg.SetDimensions(size, size, 1);
        AstarData.active.Scan(gg);
        AstarData.active.AddWorkItem(new AstarWorkItem(ctx => {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    GraphNode node = gg.GetNode(x, y);
                    node.Walkable = noiseMap[x, y] > .4f;
                }
            }
        }));
        AstarData.active.FlushGraphUpdates();

        int k = 0;
        for (int i = 0; i < rivers; i++)
        {
            GraphNode start = gg.nodes[Random.Range(16, size - 16)];
            GraphNode end = gg.nodes[Random.Range(size * (size - 1) + 16, size * size - 16)];
            ABPath path = ABPath.Construct((Vector3)start.position, (Vector3)end.position, (Path result) => {
                for (int j = 0; j < result.path.Count; j++)
                {
                    GraphNode node = result.path[j];
                    int x = Mathf.RoundToInt(((Vector3)node.position).x);
                    int y = Mathf.RoundToInt(((Vector3)node.position).z);
                    grid[x, y].isWater = true;
                    int neighbour=Random.Range(0,4);
                    //grid[x - 1, y].isWater = true;
                    //grid[x + 1, y].isWater = true;
                    //grid[x, y - 1].isWater = true;
                    //grid[x, y + 1].isWater = true;
  
                    if (x > 0)
                    {
                        grid[x - 1, y].isWater = true;
                    }

                    if (x < size - 1)
                    {
                        grid[x + 1, y].isWater = true;
                    }

                    if (y > 0)
                    {
                        grid[x, y - 1].isWater = true;
                    }

                    if (y < size - 1)
                    {
                        grid[x, y + 1].isWater = true;
                    }

                }
                k++;
            });
            AstarPath.StartPath(path);
            AstarPath.BlockUntilCalculated(path);
        }
    }
    public void SetCornersWater()
    {
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = gridArray[x, z];
                if (!cell.isWater)
                {
                    if (x > 0)
                    {
                        Cell left = gridArray[x - 1, z];
                        if (left.isWater)
                        {
                            gridArray[x, z].isCorner = true;

                            if (z + 1 < size)
                            {
                                if (!gridArray[x, z + 1].isWater)
                                {
                                    gridArray[x, z + 1].isCorner = true;
                                }
                            }
                        }
                    }
                    if (x < size - 1)
                    {
                        Cell right = gridArray[x + 1, z];
                        if (right.isWater)
                        {
                            gridArray[x + 1, z].isCorner = true;

                            if (z + 1 < size)
                            {
                                if (gridArray[x + 1, z + 1].isWater)
                                {
                                    gridArray[x + 1, z + 1].isCorner = true;
                                }
                            }
                        }
                    }
                    if (z > 0)
                    {
                        Cell down = gridArray[x, z - 1];
                        if (down.isWater)
                        {
                            gridArray[x, z].isCorner = true;
                        }
                    }
                    if (z < size - 1)
                    {
                        Cell up = gridArray[x, z + 1];
                        if (up.isWater)
                        {
                            gridArray[x, z + 1].isCorner = true;
                        }
                    }
                }
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

    public void ChangeExtractingMode(bool check)
    {
        if (check)
        {
            worldMesh.ReturnGrassGO().GetComponent<MeshRenderer>().material = materialMineralHeatMap;
        }
        else
        {
            MeshRenderer mesh = worldMesh.ReturnGrassGO().GetComponent<MeshRenderer>();

            mesh.material = worldTexture.ReturnMaterialGrass();
            Material[] matArray = mesh.materials;
            matArray[1] = null;
            mesh.materials = matArray;
        }
    }

    public void EnableBuildingMode(Material buildMaterial)
    {
        MeshRenderer mesh = worldMesh.ReturnGrassGO().GetComponent<MeshRenderer>();
        Material[] matArray = new Material[2];
        mesh.materials = new Material[2];
        matArray[0] = worldTexture.ReturnMaterialGrass();

        matArray[1] = buildMaterial;
        mesh.materials = matArray;
    }

    public void DisableBuildingMode()
    {
        MeshRenderer mesh = worldMesh.ReturnGrassGO().GetComponent<MeshRenderer>();
        Material[] matArray=new Material[1];
        matArray[0] = mesh.material;
        mesh.materials = matArray;
    }

    public void ChangeExtractingMode(Material mat)
    {
        MeshRenderer mesh = worldMesh.ReturnGrassGO().GetComponent<MeshRenderer>();
        mesh.materials = new Material[2];
        mesh.material = mat;
        Material[] matArray = mesh.materials;
        matArray[1] = autoTilerExtractiveStructures.ReturnMaterialSquareHeatMaps();
        mesh.materials = matArray;
    }

    public Vector3Int GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position; //if it's not in (0,0), you reset it to a neutral position

        int xCount = Mathf.RoundToInt(position.x / distanceTile);       //round floats to ints
        int yCount = Mathf.RoundToInt(position.y / distanceTile);
        int zCount = Mathf.RoundToInt(position.z / distanceTile);

        Vector3 result = new Vector3(   //convert them again into floats
            (float)xCount * distanceTile,
            0,
            (float)zCount * distanceTile);

        result += transform.position;   // you give the vector3 to the original position

        Vector3Int resultToInt = new Vector3Int(Convert.ToInt32(result.x), Convert.ToInt32(result.y), Convert.ToInt32(result.z));
        return resultToInt;
    }

    public bool IsNotWater(Vector3 positionForTheObject)
    {
        string positionresult = positionForTheObject.x.ToString() + "," + positionForTheObject.z.ToString();    //convert the position to string to compare it
        foreach (Cell c in gridArray)
        {
            if (positionresult == c.getPosition())    //look if is the same position as one cell and see if it's a water tile
            {
                if (!c.isWater)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool IsNotWater(Vector3 positionForTheObject, StructureParent structureParent, Vector3 actualRotation)
    {
        if (actualRotation.y >= 0 && actualRotation.y < 90)
        {
            for (int auxY = 0; auxY <= structureParent.rowsOcuppation; auxY++)
            {
                for (int auxX = 0; auxX <= structureParent.columnsOcuppation; auxX++)
                {
                    Vector2Int positionToCheck = new Vector2Int((int)(positionForTheObject.x + auxX), (int)(positionForTheObject.z - auxY));
                    if (gridArray[positionToCheck.x, positionToCheck.y].isWater)
                    {
                        if (gridArray[positionToCheck.x, positionToCheck.y].ReturnIsCorner())
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
        else if (actualRotation.y >= 90 && actualRotation.y < 180)
        {
            for (int auxY = 0; auxY <= structureParent.rowsOcuppation; auxY++)
            {
                for (int auxX = 0; auxX <= structureParent.columnsOcuppation; auxX++)
                {
                    Vector2Int positionToCheck = new Vector2Int((int)(positionForTheObject.x - auxY), (int)(positionForTheObject.z - auxX));

                    if (gridArray[positionToCheck.x, positionToCheck.y].isWater)
                    {
                        if (gridArray[positionToCheck.x, positionToCheck.y].ReturnIsCorner())
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
        else if (actualRotation.y >= 180 && actualRotation.y < 270)
        {
            for (int auxY = 0; auxY <= structureParent.rowsOcuppation; auxY++)
            {
                for (int auxX = 0; auxX <= structureParent.columnsOcuppation; auxX++)
                {
                    Vector2Int positionToCheck = new Vector2Int((int)(positionForTheObject.x - auxX), (int)(positionForTheObject.z + auxY));
                    if (gridArray[positionToCheck.x, positionToCheck.y].isWater)
                    {
                        if (gridArray[positionToCheck.x, positionToCheck.y].ReturnIsCorner())
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
        else if (actualRotation.y >= 270)
        {
            for (int auxY = 0; auxY <= structureParent.rowsOcuppation; auxY++)
            {
                for (int auxX = 0; auxX <= structureParent.columnsOcuppation; auxX++)
                {
                    Vector2Int positionToCheck = new Vector2Int((int)(positionForTheObject.x + auxY), (int)(positionForTheObject.z + auxX));
                    if (gridArray[positionToCheck.x, positionToCheck.y].isWater)
                    {
                        if (gridArray[positionToCheck.x, positionToCheck.y].ReturnIsCorner())
                        {
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    public bool IsNotOcuppied(Vector3 positionForTheObject, CellType celltypeBuilding, bool canBuild)
    {
        if (celltypeBuilding == CellType.Road)    //only corroborates the position again for the roads for not making the dictionary check the position again and again
        {
            string positionresult = positionForTheObject.x.ToString() + "," + positionForTheObject.z.ToString();    //convert the position to string to compare it
            foreach (Cell c in gridArray)
            {
                if (positionresult == c.getPosition())    //look if is the same position as one cell and see if it's a water tile
                {
                    if (CheckIfItsOcuppiedTheCell(c, celltypeBuilding))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        else
        {
            //the other structures if can be build is true, then its an autiomatically yes
            if (canBuild)
            {
                return true;
            }
            return false;
        }
    }
    public int ReturnAverageEfectivenessExtractiveBuildings(Vector3 positionForTheObject, StructureParent structureParent, Vector3 actualRotation,List<int> listValuesForAverage,ExtractiveResources extractiveResources)
    {
        listValuesForAverage.Clear();
        if (actualRotation.y >= 0 && actualRotation.y < 90)
        {
            for (int auxY = 0; auxY <= structureParent.rowsOcuppation; auxY++)
            {
                for (int auxX = 0; auxX <= structureParent.columnsOcuppation; auxX++)
                {
                    listValuesForAverage.Add(gridArray[Convert.ToInt32(positionForTheObject.x + auxX), Convert.ToInt32(positionForTheObject.z - auxY)].ReturnValueHeatMap(extractiveResources));
                }
            }
        }
        else if (actualRotation.y >= 90 && actualRotation.y < 180)
        {
            for (int auxY = 1; auxY < structureParent.rowsOcuppation; auxY++)
            {
                for (int auxX = 1; auxX < structureParent.columnsOcuppation; auxX++)
                {

                    listValuesForAverage.Add(gridArray[Convert.ToInt32(positionForTheObject.x - auxY), Convert.ToInt32(positionForTheObject.z - auxX)].ReturnValueHeatMap(extractiveResources));
                }
            }
        }
        else if (actualRotation.y >= 180 && actualRotation.y < 270)
        {
            for (int auxY = 1; auxY < structureParent.rowsOcuppation; auxY++)
            {
                for (int auxX = 1; auxX < structureParent.columnsOcuppation; auxX++)
                {

                    listValuesForAverage.Add(gridArray[Convert.ToInt32(positionForTheObject.x - auxX), Convert.ToInt32(positionForTheObject.z + auxY)].ReturnValueHeatMap(extractiveResources));
                }
            }
        }
        else if (actualRotation.y >= 270)
        {
            for (int auxY = 1; auxY < structureParent.rowsOcuppation; auxY++)
            {
                for (int auxX = 1; auxX < structureParent.columnsOcuppation; auxX++)
                {

                    listValuesForAverage.Add(gridArray[Convert.ToInt32(positionForTheObject.x + auxY), Convert.ToInt32(positionForTheObject.z + auxX)].ReturnValueHeatMap(extractiveResources));
                }
            }
        }
        return (int)listValuesForAverage.Average();
    }
    public bool IsNotOcuppied(Vector3 positionForTheObject, StructureParent structureParent, Vector3 actualRotation, CellType celltypeBuilding)
    {
        if ((actualRotation.y >= 0 && actualRotation.y < 90) || (actualRotation.y == 360))
        {
            for (int auxY = 1; auxY < structureParent.rowsOcuppation; auxY++)
            {
                for (int auxX = 1; auxX < structureParent.columnsOcuppation; auxX++)
                {
                    if (CheckIfItsOcuppiedTheCell(gridArray[Convert.ToInt32(positionForTheObject.x + auxX), Convert.ToInt32(positionForTheObject.z - auxY)], celltypeBuilding))
                    {
                        return false;
                    }
                }
            }
            structureParent.SetRotation(RotationType.RightDown);
        }
        else if (actualRotation.y >= 90 && actualRotation.y < 180)
        {
            for (int auxY = 1; auxY < structureParent.rowsOcuppation; auxY++)
            {
                for (int auxX = 1; auxX < structureParent.columnsOcuppation; auxX++)
                {
                    if (CheckIfItsOcuppiedTheCell(gridArray[Convert.ToInt32(positionForTheObject.x - auxY), Convert.ToInt32(positionForTheObject.z - auxX)], celltypeBuilding))
                    {
                        return false;
                    }
                }
            }
            structureParent.SetRotation(RotationType.LeftDown);
        }
        else if (actualRotation.y >= 180 && actualRotation.y < 270)
        {
            for (int auxY = 1; auxY < structureParent.rowsOcuppation; auxY++)
            {
                for (int auxX = 1; auxX < structureParent.columnsOcuppation; auxX++)
                {
                    if (CheckIfItsOcuppiedTheCell(gridArray[Convert.ToInt32(positionForTheObject.x - auxX), Convert.ToInt32(positionForTheObject.z + auxY)], celltypeBuilding))
                    {
                        return false;
                    }
                }
            }
            structureParent.SetRotation(RotationType.LeftTop);
        }
        else if (actualRotation.y >= 270 && actualRotation.y < 359)
        {
            for (int auxY = 1; auxY < structureParent.rowsOcuppation; auxY++)
            {
                for (int auxX = 1; auxX < structureParent.columnsOcuppation; auxX++)
                {
                    if (CheckIfItsOcuppiedTheCell(gridArray[Convert.ToInt32(positionForTheObject.x + auxY), Convert.ToInt32(positionForTheObject.z + auxX)], celltypeBuilding))
                    {
                        return false;
                    }
                }
            }
            structureParent.SetRotation(RotationType.RightTop);
        }
        return true;
    }

    public bool CheckIfItsOcuppiedTheCell(Cell c, CellType celltypeBuilding)  //if it's not ocuppied the position, allows the players to instantiate a construction
    {
        if (c.cellType == CellType.Empty)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool CheckIfWaterIsNextWithDiagonals(Vector2Int positionToCheck)
    {
        int countWater = 0;
        if (gridArray[positionToCheck.x - 1, positionToCheck.y].isWater)
        {
            countWater++;
        }
        if (gridArray[positionToCheck.x + 1, positionToCheck.y].isWater)
        {
            countWater++;
        }
        if (gridArray[positionToCheck.x, positionToCheck.y - 1].isWater)
        {
            countWater++;
        }
        if (gridArray[positionToCheck.x, positionToCheck.y + 1].isWater)
        {
            countWater++;
        }
        if (gridArray[positionToCheck.x - 1, positionToCheck.y - 1].isWater)
        {
            countWater++;
        }
        if (gridArray[positionToCheck.x - 1, positionToCheck.y + 1].isWater)
        {
            countWater++;
        }
        if (gridArray[positionToCheck.x + 1, positionToCheck.y + 1].isWater)
        {
            countWater++;
        }
        if (gridArray[positionToCheck.x - 1, positionToCheck.y + 1].isWater)
        {
            countWater++;
        }

        if (countWater == 8)
            return true;
        else
            return false;
    }

    public int CheckIfWaterIsNext(Vector3 positionForTheObject)  //if it's not ocuppied the position, allows the players to instantiate a construction
    {
        string positionresult = positionForTheObject.x.ToString() + "," + positionForTheObject.z.ToString();    //convert the position to string to compare it
        int countWater = 0;
        int x = (int)positionForTheObject.x;
        int z = (int)positionForTheObject.z;

        foreach (Cell c in gridArray)
        {
            if (positionresult == c.getPosition())    //look if is the same position as one cell and see if it's a water tile
            {
                if (gridArray[x - 1, z].isWater || gridArray[x - 1, z].ReturnIsCorner())
                {
                    countWater++;
                }
                if (gridArray[x + 1, z].isWater || gridArray[x + 1, z].ReturnIsCorner())
                {
                    countWater++;
                }
                if (gridArray[x, z - 1].isWater || gridArray[x, z - 1].ReturnIsCorner())
                {
                    countWater++;
                }
                if (gridArray[x, z + 1].isWater || gridArray[x, z + 1].ReturnIsCorner())
                {
                    countWater++;
                }
                else
                {
                }
            }
        }
        return countWater;
    }

    //[left, up, right,down]
    //[ 0 , 1 , 2 , 3 ]
    public CellType[] GetAllAdjacentCellTypes(int x, int z)
    {
        CellType[] neighbours = { CellType.None, CellType.None, CellType.None, CellType.None };
        if (x > 0)
        {
            neighbours[0] = gridArray[x - 1, z].cellType;   //left
        }
        if (x < size - 1)
        {
            neighbours[2] = gridArray[x + 1, z].cellType;   //right
        }
        if (z > 0)
        {
            neighbours[3] = gridArray[x, z - 1].cellType;   //down
        }
        if (z < size - 1)
        {
            neighbours[1] = gridArray[x, z + 1].cellType;   //up
        }
        return neighbours;
    }


    //[left, up, right,down]
    //[ 0 , 1 , 2 , 3 ]

    public bool CheckIfOneAdjacentCellIsOfType(int x, int z, CellType type)
    {
        List<CellType> neighbours = new List<CellType>();
        if (x > 0)
        {
            neighbours.Add(gridArray[x - 1, z].cellType);   //left
        }
        if (x < size - 1)
        {
            neighbours.Add(gridArray[x + 1, z].cellType);   //right
        }
        if (z > 0)
        {
            neighbours.Add(gridArray[x, z - 1].cellType);   //down
        }
        if (z < size - 1)
        {
            neighbours.Add(gridArray[x, z + 1].cellType);   //up
        }
        bool isOneExistent = neighbours.Exists(x => x == type);
        return isOneExistent;
    }

    public bool CheckIfOneAdjacentCellIsOfType(List<Cell> cells, CellType type, CellType type2)
    {
        List<CellType> neighbours = new List<CellType>();

        foreach (var item in cells)
        {
            if (item.x > 0)
            {
                neighbours.Add(gridArray[item.x - 1, item.z].cellType);   //left
            }
            if (item.x < size - 1)
            {
                neighbours.Add(gridArray[item.x + 1, item.z].cellType);   //right
            }
            if (item.z > 0)
            {
                neighbours.Add(gridArray[item.x, item.z - 1].cellType);   //down
            }
            if (item.z < size - 1)
            {
                neighbours.Add(gridArray[item.x, item.z + 1].cellType);   //up
            }
        }

        bool isOneExistent = neighbours.Exists(x => x == type || x == type2);
        return isOneExistent;
    }
    public bool CheckBuildingEnterConnection(List<Vector3> cellsEnter, CellType type, CellType type2,ref Vector3 entryPosition)
    {
        List<CellType> neighbours = new List<CellType>();

        foreach (var item in cellsEnter)
        {
            if (item.x > 0)
            {
                neighbours.Add(gridArray[(int)item.x - 1,(int)item.z].cellType);   //left
            }
            if (item.x < size - 1)
            {
                neighbours.Add(gridArray[(int)item.x + 1,(int)item.z].cellType);   //right
            }
            if (item.z > 0)
            {
                neighbours.Add(gridArray[(int)item.x, (int)item.z - 1].cellType);   //down
            }
            if (item.z < size - 1)
            {
                neighbours.Add(gridArray[(int)item.x,(int) item.z + 1].cellType);   //up
            }
        }

        bool isOneExistent = neighbours.Exists(x => x == type || x == type2);
        bool isFound = false;
        foreach (var item in cellsEnter)
        {
            if (item.x > 0)
            {
                if (!isFound)
                {
                    if (gridArray[(int)item.x - 1, (int)item.z].cellType == CellType.PivotRoad || gridArray[(int)item.x - 1, (int)item.z].cellType == CellType.Road)  //left
                    {
                        entryPosition = item;
                        isFound = true;
                    }
                }

            }
            if (item.x < size - 1)
            {
                if (!isFound)
                {
                    if (gridArray[(int)item.x + 1, (int)item.z].cellType == CellType.PivotRoad || gridArray[(int)item.x + 1, (int)item.z].cellType == CellType.Road)  //left
                    {
                        entryPosition = item;
                        isFound = true;
                    }
                }

            }
            if (item.z > 0)
            {
                if (!isFound)
                {
                    if (gridArray[(int)item.x, (int)item.z - 1].cellType == CellType.PivotRoad || gridArray[(int)item.x, (int)item.z - 1].cellType == CellType.Road)  //left
                    {
                        entryPosition = item;
                        isFound = true;
                    }
                }

            }
            if (item.z < size - 1)
            {
                if (!isFound)
                {
                    if (gridArray[(int)item.x, (int)item.z + 1].cellType == CellType.PivotRoad || gridArray[(int)item.x, (int)item.z + 1].cellType == CellType.Road)  //left
                    {
                        entryPosition = item;
                        isFound = true;
                    }
                }
            }
        }
        return isOneExistent;
    }

    public List<Vector3> CheckAdjacentCellsOfType(int x, int z, CellType type)
    {
        List<Vector3> neighbours = new List<Vector3>();
        if (x > 0)
        {
            if (gridArray[x - 1, z].cellType == type)
            {
                neighbours.Add(new Vector3(x - 1, 0, z));   //left
                Debug.Log(gridArray[x - 1, z].cellType);
            }
        }
        if (x < size - 1)
        {
            if (gridArray[x + 1, z].cellType == type)
            {
                neighbours.Add(new Vector3(x + 1, 0, z));  //right
                Debug.Log(gridArray[x + 1, z].cellType);
            }
        }
        if (z > 0)
        {
            if (gridArray[x, z - 1].cellType == type)
            {
                neighbours.Add(new Vector3(x, 0, z - 1));   //down
                Debug.Log(gridArray[x, z - 1].cellType);
            }
        }
        if (z < size - 1)
        {
            if (gridArray[x, z + 1].cellType == type)
            {
                neighbours.Add(new Vector3(x, 0, z + 1));  //up
                Debug.Log(gridArray[x, z + 1].cellType);
            }
        }

        return neighbours;
    }

    public List<Vector3> CheckIfDifferentAdjacentCells(int x, int z, CellType type1, CellType type2)
    {
        List<Vector3> neighbours = new List<Vector3>();
        if (x > 0)
        {
            if (gridArray[x - 1, z].cellType != type1 && gridArray[x - 1, z].cellType != type2)
            {
                neighbours.Add(new Vector3(x - 1, 0, z));   //left
                Debug.Log(gridArray[x - 1, z].cellType);
            }
        }
        if (x < size - 1)
        {
            if (gridArray[x + 1, z].cellType != type1 && gridArray[x + 1, z].cellType != type2)
            {
                neighbours.Add(new Vector3(x + 1, 0, z));  //right
                Debug.Log(gridArray[x + 1, z].cellType);
            }
        }
        if (z > 0)
        {
            if (gridArray[x, z - 1].cellType != type1 && gridArray[x, z - 1].cellType != type2)
            {
                neighbours.Add(new Vector3(x, 0, z - 1));   //down
                Debug.Log(gridArray[x, z - 1].cellType);
            }
        }
        if (z < size - 1)
        {
            if (gridArray[x, z + 1].cellType != type1 && gridArray[x, z + 1].cellType != type2)
            {
                neighbours.Add(new Vector3(x, 0, z + 1));  //up
                Debug.Log(gridArray[x, z + 1].cellType);
            }
        }

        return neighbours;
    }

    public List<Vector3> GetAdjacentCellsOfType(Vector3 vector, CellType type)
    {
        List<Vector3> adjacentCells = GetAllAdjacentCells(vector);

        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (gridArray[Convert.ToInt32(adjacentCells[i].x), Convert.ToInt32(adjacentCells[i].z)].cellType != type)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        AdjacentCells = adjacentCells;
        return adjacentCells;
    }
    public List<Vector3> GetAdjacentCellsOfTypeForRoads(Vector3 vector, CellType type)
    {
        List<Vector3> adjacentCells = GetAllAdjacentCellsForRoads(vector);

        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (gridArray[Convert.ToInt32(adjacentCells[i].x), Convert.ToInt32(adjacentCells[i].z)].cellType != type)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        AdjacentCells = adjacentCells;
        return adjacentCells;
    }

    public List<Vector3> GetAdjacentCellsOfTypeForCorner(Vector3 vector, CellType type)
    {
        List<Vector3> adjacentCells = GetAllAdjacentCellsFromCorner(vector);

        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (gridArray[Convert.ToInt32(adjacentCells[i].x), Convert.ToInt32(adjacentCells[i].z)].cellType != type)
            {
                adjacentCells.RemoveAt(i);
            }
        }
        AdjacentCells = adjacentCells;
        return adjacentCells;
    }

    public List<Vector3> GetAllAdjacentCellsFromCorner(Vector3 vector3)
    {
        Vector3Int vector = new Vector3Int(Convert.ToInt32(vector3.x), 0, Convert.ToInt32(vector3.z));
        List<Vector3> cells = new List<Vector3>();
        if (vector.x > 0 && vector.x < size - 1 && vector.z < size - 1)
        {
            //RightCorner
            cells.Add(new Vector3(vector.x + 1, 0, vector.z + 1));
        }
        if (vector.x < size - 1)
        {
            //right
            cells.Add(new Vector3(vector.x + 1, 0, vector.z));
        }
        if (vector.z < size - 1)
        {
            //up
            cells.Add(new Vector3(vector.x, 0, vector.z + 1));
        }
        return cells;
    }

    public List<Vector3> GetAllAdjacentCellsForRoads(Vector3 vector3)
    {
        Vector3Int vector = new Vector3Int(Convert.ToInt32(vector3.x), 0, Convert.ToInt32(vector3.z));
        List<Vector3> cells = new List<Vector3>();
        if (vector.x > 0)
        {
            //left
            cells.Add(new Vector3(vector.x - 1, 0, vector.z));
        }
        if (vector.x < size - 1)
        {
            //right
            cells.Add(new Vector3(vector.x + 1, 0, vector.z));
        }
        if (vector.z > 0)
        {
            //down
            if (gridArray[vector.x, vector.z - 1].isWater || gridArray[vector.x, vector.z - 1].ReturnIsCorner())
            {

            }
            else
            {
                cells.Add(new Vector3(vector.x, 0, vector.z - 1));

            }
        }
        if (vector.z < size - 1)
        {
            //up
            if (gridArray[vector.x, vector.z + 1].isWater || gridArray[vector.x, vector.z + 1].ReturnIsCorner())
            {

            }
            else
            {
                cells.Add(new Vector3(vector.x, 0, vector.z + 1));

            }
        }
        return cells;
    }
    public List<Vector3> GetAllAdjacentCells(Vector3 vector3)
    {
        Vector3Int vector = new Vector3Int(Convert.ToInt32(vector3.x), 0, Convert.ToInt32(vector3.z));
        List<Vector3> cells = new List<Vector3>();
        if (vector.x > 0)
        {
            //left
            cells.Add(new Vector3(vector.x - 1, 0, vector.z));
        }
        if (vector.x < size - 1)
        {
            //right
            cells.Add(new Vector3(vector.x + 1, 0, vector.z));
        }
        if (vector.z > 0)
        {
            //down
            cells.Add(new Vector3(vector.x, 0, vector.z - 1));
        }
        if (vector.z < size - 1)
        {
            //up
            cells.Add(new Vector3(vector.x, 0, vector.z + 1));
        }
        return cells;
    }

    public List<Vector3> GetWakableAdjacentCells(Vector3 position, bool isAgent)
    {
        List<Vector3> adjacentCells = GetAllAdjacentCells(position);
        for (int i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (IsCellWakable(gridArray[Convert.ToInt32(adjacentCells[i].x), Convert.ToInt32(adjacentCells[i].z)].cellType, isAgent) == false)
            {
                //Debug.Log("heyy");
                adjacentCells.RemoveAt(i);
            }
        }
        return adjacentCells;
    }
    public List<Cell> GetAdjacentCellsLogic(Cell vector3)
    {
        List<Cell> cells = new List<Cell>();

        if (vector3.x > 0)
        {
            //left
            cells.Add(gridArray[(int)vector3.x - 1, (int)vector3.z]);
        }
        if (vector3.x < size - distanceTile)
        {
            //right
            cells.Add(gridArray[(int)vector3.x + 1, (int)vector3.z]);
        }
        if (vector3.z > 0)
        {
            //down
            cells.Add(gridArray[(int)vector3.x, (int)vector3.z - 1]);
        }
        if (vector3.z < size - distanceTile)
        {
            //up
            cells.Add(gridArray[(int)vector3.x, (int)vector3.z + 1]);
        }
        return cells;
    }

    public float GetCostOfEnteringCell(Vector3 cell)
    {

        switch (gridArray[(int)cell.x, (int)cell.z].cellType)
        {
            case CellType.Empty:
                return 0.5f;

            case CellType.PivotRoad:
                return 0.5f;

            default:
                break;
        }
        return 20;

    }
    public float GetCostOfEnteringCellForPeople(Vector3 cell)
    {

        switch (gridArray[(int)cell.x, (int)cell.z].cellType)
        {
            case CellType.PivotRoad:
                return 0.5f;
            case CellType.Road:
                return 0.5f;
            case CellType.HouseConnection:
                return 0.5f;
            case CellType.Empty:
                return 5f;
            default:
                break;
        }
        return 20;

    }
    public float GetCostOfEnteringCellForRivers(Vector3 cell)
    {

        switch (gridArray[(int)cell.x, (int)cell.z].ReturnIsWalkable())
        {
            case false:
                return 20f;

            case true:
                return 0.5f;

            default:
                break;
        }
        return 20;

    }

    public List<Vector3> GetAdjacentCells(Vector3 cell, bool isAgent)
    {
        return GetWakableAdjacentCells(cell, isAgent);
    }

    public static bool IsCellWakable(CellType cellType, bool aiAgent = false)
    {
        if (aiAgent)
        {
            return cellType == CellType.Road;
        }

        return cellType == CellType.Empty || cellType == CellType.PivotRoad || cellType == CellType.Road || cellType == CellType.StructureInsula || cellType == CellType.HouseConnection ;
    }
}
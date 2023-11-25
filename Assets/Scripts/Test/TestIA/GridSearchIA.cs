using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class GridSearchIA : MonoBehaviour
{
    public struct SearchResult
    {
        public List<Vector3> Path { get; set; }
    }

    public static List<Vector3> AStarSearch(GridIA grid, Vector3 startPosition, Vector3 endPosition, bool isAgent = false)
    {
        List<Vector3> path = new List<Vector3>();

        List<Vector3> positionsTocheck = new List<Vector3>();
        Dictionary<Vector3, float> costDictionary = new Dictionary<Vector3, float>();
        Dictionary<Vector3, float> priorityDictionary = new Dictionary<Vector3, float>();
        Dictionary<Vector3, Vector3> parentsDictionary = new Dictionary<Vector3, Vector3>();

        positionsTocheck.Add(startPosition);
        priorityDictionary.Add(startPosition, 0);
        costDictionary.Add(startPosition, 0);
        parentsDictionary.Add(startPosition, Vector3.zero);

        while (positionsTocheck.Count > 0)  //while we need to check one more position
        {
            Vector3 current = GetClosestVertex(positionsTocheck, priorityDictionary);
            positionsTocheck.Remove(current);   //removes the actuualPosition from the positions to check

            if (current.Equals(endPosition))
            {
                path = GeneratePath(parentsDictionary, current);    //if it is in the endposition, just generates the path
                return path;
            }

            foreach (Vector3 neighbour in grid.GetAdjacentCells(current, isAgent))  //search all the current neighbours cells
            {
                float newCost = costDictionary[current] + grid.GetCostOfEnteringCell(neighbour);    //calculates the cost for each cell with the actual cost of the cell + the cost of entering in it
                Debug.Log(grid.GetCostOfEnteringCell(neighbour));

                if (!costDictionary.ContainsKey(neighbour) || newCost < costDictionary[neighbour])  //if the new cost is less than the cost of the neifghbour in the dictionary
                {
                    costDictionary[neighbour] = newCost;    //the new cost is now the one from the dictionary

                    float priority = newCost + ManhattanDiscance(endPosition, neighbour);   //calculates the priority with the new cost+the manhattandistance
                    positionsTocheck.Add(neighbour);    //the positions to check adds the neighbour for lately change the road orientation or prefab
                    priorityDictionary[neighbour] = priority;   //the priority is now added to the prioritydictionary

                    parentsDictionary[neighbour] = current;
                }
            }
        }
        return path;
    }

    public static List<Vector3> AStarSearchNormalGrid(WorldGenerator grid, Vector3 startPosition, Vector3 endPosition, bool isAgent = false)
    {
        List<Vector3> path = new List<Vector3>();

        List<Vector3> positionsTocheck = new List<Vector3>();
        Dictionary<Vector3, float> costDictionary = new Dictionary<Vector3, float>();
        Dictionary<Vector3, float> priorityDictionary = new Dictionary<Vector3, float>();
        Dictionary<Vector3, Vector3> parentsDictionary = new Dictionary<Vector3, Vector3>();

        positionsTocheck.Add(startPosition);
        priorityDictionary.Add(startPosition, 0);
        costDictionary.Add(startPosition, 0);
        parentsDictionary.Add(startPosition, Vector3.zero);

        while (positionsTocheck.Count > 0)  //while we need to check one more position
        {
            Vector3 current = GetClosestVertex(positionsTocheck, priorityDictionary);
            positionsTocheck.Remove(current);   //removes the actuualPosition from the positions to check

            if (current.Equals(endPosition))
            {
                path = GeneratePath(parentsDictionary, current);    //if it is in the endposition, just generates the path
                return path;
            }

            foreach (Vector3 neighbour in grid.GetAdjacentCells(current, isAgent))  //search all the current neighbours cells
            {
                float newCost = costDictionary[current] + grid.GetCostOfEnteringCell(neighbour);    //calculates the cost for each cell with the actual cost of the cell + the cost of entering in it
                if (!costDictionary.ContainsKey(neighbour) || newCost < costDictionary[neighbour])  //if the new cost is less than the cost of the neifghbour in the dictionary
                {
                    costDictionary[neighbour] = newCost;    //the new cost is now the one from the dictionary

                    float priority = newCost + ManhattanDiscance(endPosition, neighbour);   //calculates the priority with the new cost+the manhattandistance
                    positionsTocheck.Add(neighbour);    //the positions to check adds the neighbour for lately change the road orientation or prefab
                    priorityDictionary[neighbour] = priority;   //the priority is now added to the prioritydictionary

                    parentsDictionary[neighbour] = current;
                }
            }
        }
        return path;
    }
    public static List<Vector3> AStarSearchNormalGridForPeople(WorldGenerator grid, Vector3 startPosition, Vector3 endPosition, bool isAgent = false)
    {
        List<Vector3> path = new List<Vector3>();

        List<Vector3> positionsTocheck = new List<Vector3>();
        Dictionary<Vector3, float> costDictionary = new Dictionary<Vector3, float>();
        Dictionary<Vector3, float> priorityDictionary = new Dictionary<Vector3, float>();
        Dictionary<Vector3, Vector3> parentsDictionary = new Dictionary<Vector3, Vector3>();

        positionsTocheck.Add(startPosition);
        priorityDictionary.Add(startPosition, 0);
        costDictionary.Add(startPosition, 0);
        parentsDictionary.Add(startPosition, Vector3.zero);

        while (positionsTocheck.Count > 0)  //while we need to check one more position
        {
            Vector3 current = GetClosestVertex(positionsTocheck, priorityDictionary);
            positionsTocheck.Remove(current);   //removes the actuualPosition from the positions to check

            if (current.Equals(endPosition))
            {

                path = GeneratePath(parentsDictionary, current);    //if it is in the endposition, just generates the path
                return path;
            }

            foreach (Vector3 neighbour in grid.GetAdjacentCells(current, isAgent))  //search all the current neighbours cells
            {
                float newCost = costDictionary[current] + grid.GetCostOfEnteringCellForPeople(neighbour);    //calculates the cost for each cell with the actual cost of the cell + the cost of entering in it
                if (!costDictionary.ContainsKey(neighbour) || newCost < costDictionary[neighbour])  //if the new cost is less than the cost of the neifghbour in the dictionary
                {
                    costDictionary[neighbour] = newCost;    //the new cost is now the one from the dictionary

                    float priority = newCost + ManhattanDiscance(endPosition, neighbour);   //calculates the priority with the new cost+the manhattandistance
                    positionsTocheck.Add(neighbour);    //the positions to check adds the neighbour for lately change the road orientation or prefab
                    priorityDictionary[neighbour] = priority;   //the priority is now added to the prioritydictionary

                    parentsDictionary[neighbour] = current;
                }
            }
        }
        return path;
    }

    private static Vector3 GetClosestVertex(List<Vector3> list, Dictionary<Vector3, float> distanceMap)
    {
        Vector3 candidate = list[0];
        foreach (Vector3 vertex in list)
        {
            if (distanceMap[vertex] < distanceMap[candidate])   //gets the closest vertex comparing the distant between each cell
            {
                candidate = vertex;
            }
        }
        return candidate;
    }

    private static float ManhattanDiscance(Vector3 endPos, Vector3 point)
    {
        return Math.Abs(endPos.x - point.x) + Math.Abs(endPos.z - point.z);
    }

    public static List<Vector3> GeneratePath(Dictionary<Vector3, Vector3> parentMap, Vector3 endState)
    {
        List<Vector3> path = new List<Vector3>();
        Vector3 parent = endState;
        while (parent != null && parentMap.ContainsKey(parent))
        {
            path.Add(parent);
            parent = parentMap[parent];
        }
        return path;
    }

    public static List<CellIA> FindPath(GridIA grid, Vector3 startPosition, Vector3 endPosition)
    {
        CellIA startNode = grid.ReturnCellFromVector(startPosition, grid.gridArray);
        CellIA targetNode = grid.ReturnCellFromVector(endPosition, grid.gridArray);

        List<CellIA> openSet = new List<CellIA>();
        HashSet<CellIA> closedSet = new HashSet<CellIA>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            CellIA currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (CellIA neighbour in grid.GetAdjacentCellsLogic(currentNode))
            {
                if (!neighbour.GetWakableAdjacentCellsLogic() || closedSet.Contains(neighbour)) continue;

                float newMovementCostToNeighbour = currentNode.gCost + ManhattanDiscance(currentNode.GetWorldPosition(), neighbour.GetWorldPosition());
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = ManhattanDiscance(neighbour.GetWorldPosition(), targetNode.GetWorldPosition());
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null;
    }

    public static List<Cell> FindPathNormalGrid(WorldGenerator grid, Vector3 startPosition, Vector3 endPosition)
    {
        Cell startNode = grid.gridArray[Convert.ToInt32(startPosition.x), Convert.ToInt32(startPosition.z)];
        Cell targetNode = grid.gridArray[Convert.ToInt32(endPosition.x), Convert.ToInt32(endPosition.z)];

        List<Cell> openSet = new List<Cell>();
        HashSet<Cell> closedSet = new HashSet<Cell>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Cell currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Cell neighbour in grid.GetAdjacentCellsLogic(currentNode))
            {
                if (!neighbour.GetWakableAdjacentCellsLogic() || closedSet.Contains(neighbour)) continue;

                float newMovementCostToNeighbour = currentNode.gCost + ManhattanDiscance(new Vector3(currentNode.x, 0, currentNode.z), new Vector3(neighbour.x, 0, neighbour.z));
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = ManhattanDiscance(new Vector3(neighbour.x, 0, neighbour.z), new Vector3(targetNode.x, 0, targetNode.z));
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null;
    }

    private static List<CellIA> RetracePath(CellIA startNode, CellIA targetNode)
    {
        List<CellIA> path = new List<CellIA>();
        CellIA currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private static List<Cell> RetracePath(Cell startNode, Cell targetNode)
    {
        List<Cell> path = new List<Cell>();
        Cell currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
}
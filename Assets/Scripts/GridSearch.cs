using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
public class GridSearch
{

    public struct SearchResult
    {
        public List<Vector3> Path { get; set; }
    }

    public static List<Vector3> AStarSearch(WorldGenerator grid, Vector3 startPosition, Vector3 endPosition, bool isAgent = false)
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
        return Math.Abs(endPos.x - point.z) + Math.Abs(endPos.z - point.z);
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
}
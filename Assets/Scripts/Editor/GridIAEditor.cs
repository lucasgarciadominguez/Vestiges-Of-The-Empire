using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(GridIA))]
[CanEditMultipleObjects]
public class GridIAEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        GridIA grid = (GridIA)target;
        if (GUILayout.Button("Create Path"))
        {
            //Debug.Log(grid.gridArray[0, 50].GetWorldPosition());
            //List<Vector3> list=grid.GetAllAdjacentCells(grid.gridArray[0, 50].GetWorldPosition());
            //foreach (Vector3 item in list)
            //{
            //    Debug.Log(item);
            //}
            //List<Vector3> path = grid.GetPathBetween(grid.gridArray[40, 41].GetWorldPosition(), grid.gridArray[20, 24].GetWorldPosition());
            //grid.Path = path;
            List<CellIA> path = grid.GetPathBetween2(grid.gridArray[2, 2].GetWorldPosition(), grid.gridArray[20, 15].GetWorldPosition());
            grid.Path2 = path;
        }
        if (GUILayout.Button("Paint Path"))
        {
            foreach (CellIA item in grid.Path2)
            {
                Instantiate(grid.pathObject, new Vector3(item.positionWorld.x+0.1f,0, item.positionWorld.z+ 0.1f), Quaternion.identity);
            }
        }
    }
}
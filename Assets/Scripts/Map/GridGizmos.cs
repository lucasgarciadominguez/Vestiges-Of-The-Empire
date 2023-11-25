using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridGizmos : MonoBehaviour
{
    private WorldGenerator worldGenerator;
    private int X_Axis_Length;
    private int Y_Axis_Length;
    public int font_Size;
    private GUIStyle guiStyle = new GUIStyle(); //create a new variable

    private void Start()
    {
        worldGenerator = GetComponent<WorldGenerator>();
        if (worldGenerator.size <= 80)
        {
            X_Axis_Length = worldGenerator.size;
            Y_Axis_Length = worldGenerator.size;
        }
    }

    public void OnDrawGizmos()
    {
        guiStyle.fontSize = font_Size;

        for (int yy = 0; yy < Y_Axis_Length; yy++)
        {
            for (int xx = 0; xx < X_Axis_Length; xx++)
            {
                Handles.Label(new Vector3(xx, 0, yy), (xx + "," + yy + ": " + worldGenerator.gridArray[xx, yy].cellType) + ";" + worldGenerator.gridArray[xx, yy].isWater);
            }
        }
    }
}
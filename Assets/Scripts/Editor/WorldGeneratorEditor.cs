using log4net.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
[CustomEditor(typeof(WorldGenerator))]
[CanEditMultipleObjects]
public class WorldGeneratorEditor : Editor
{
    public bool showLevels = true;

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        WorldGenerator grid = (WorldGenerator)target;
        var serializedObject = new SerializedObject(grid);

        if (GUILayout.Button("Regenerate Terrain"))
        {
            grid.GenerateProceduralTerrain();
        }
        if (GUILayout.Button("Regenerate HeatMaps"))
        {
            foreach (var item in grid.heatMapsItems)
            {
                item.FillNoiseMap(grid.size, grid.Seed);

                item.CreateHeatMap(grid.size, grid.heatMapTexture);
            }
        }

        // Triangles - Int Arrays
        //for (int y = 0; y < grid.size; y++)
        //{
        //    for (int x = 0; x < grid.size; x++)
        //    {
        //        EditorGUILayout.PropertyField
        //        (
        //        serializedObject.FindProperty(nameof(grid.size)),
        //        new GUIContent("Position: " + x + "," + y + " in the array")
        //        );
        //}


        //WorldGenerator  levels = (WorldGenerator)target;
        //EditorGUILayout.Space();

        //showLevels = EditorGUILayout.Foldout(showLevels, "Levels (" + levels.gridArray.Length + ")");
        //if (showLevels)
        //{
        //    EditorGUI.indentLevel++;
        //    for (int y = 0; y < levels.gridArray.Length; y++)
        //    {
        //        for (int x = 0; x < levels.gridArray.Length; x++)
        //        {

        //            EditorGUI.indentLevel = 0;

        //            GUIStyle tableStyle = new GUIStyle("box");
        //            tableStyle.padding = new RectOffset(10, 10, 10, 10);
        //            tableStyle.margin.left = 32;

        //            GUIStyle headerColumnStyle = new GUIStyle();
        //            headerColumnStyle.fixedWidth = 35;

        //            GUIStyle columnStyle = new GUIStyle();
        //            columnStyle.fixedWidth = 65;

        //            GUIStyle rowStyle = new GUIStyle();
        //            rowStyle.fixedHeight = 25;

        //            GUIStyle rowHeaderStyle = new GUIStyle();
        //            rowHeaderStyle.fixedWidth = columnStyle.fixedWidth - 1;

        //            GUIStyle columnHeaderStyle = new GUIStyle();
        //            columnHeaderStyle.fixedWidth = 30;
        //            columnHeaderStyle.fixedHeight = 25.5f;

        //            GUIStyle columnLabelStyle = new GUIStyle();
        //            columnLabelStyle.fixedWidth = rowHeaderStyle.fixedWidth - 6;
        //            columnLabelStyle.alignment = TextAnchor.MiddleCenter;
        //            columnLabelStyle.fontStyle = FontStyle.Bold;

        //            GUIStyle cornerLabelStyle = new GUIStyle();
        //            cornerLabelStyle.fixedWidth = 42;
        //            cornerLabelStyle.alignment = TextAnchor.MiddleRight;
        //            cornerLabelStyle.fontStyle = FontStyle.BoldAndItalic;
        //            cornerLabelStyle.fontSize = 14;
        //            cornerLabelStyle.padding.top = -5;

        //            GUIStyle rowLabelStyle = new GUIStyle();
        //            rowLabelStyle.fixedWidth = 25;
        //            rowLabelStyle.alignment = TextAnchor.MiddleRight;
        //            rowLabelStyle.fontStyle = FontStyle.Bold;

        //            GUIStyle enumStyle = new GUIStyle("popup");
        //            rowStyle.fixedWidth = 65;

        //            EditorGUILayout.BeginHorizontal(tableStyle);
        //            for ( x = -1; x < levels.gridArray.Length; x++)
        //            {
        //                EditorGUILayout.BeginVertical((x == -1) ? headerColumnStyle : columnStyle);
        //                for ( y = -1; y < levels.gridArray.Length; y++)
        //                {
        //                    if (x == -1 && y == -1)
        //                    {
        //                        EditorGUILayout.BeginVertical(rowHeaderStyle);
        //                        EditorGUILayout.LabelField("[X,Y]", cornerLabelStyle);
        //                        EditorGUILayout.EndHorizontal();
        //                    }
        //                    else if (x == -1)
        //                    {
        //                        EditorGUILayout.BeginVertical(columnHeaderStyle);
        //                        EditorGUILayout.LabelField(y.ToString(), rowLabelStyle);
        //                        EditorGUILayout.EndHorizontal();
        //                    }
        //                    else if (y == -1)
        //                    {
        //                        EditorGUILayout.BeginVertical(rowHeaderStyle);
        //                        EditorGUILayout.LabelField(x.ToString(), columnLabelStyle);
        //                        EditorGUILayout.EndHorizontal();
        //                    }

        //                    if (x >= 0 && y >= 0)
        //                    {
        //                        if (x >= 0 && y >= 0)
        //                        {
        //                            EditorGUILayout.BeginHorizontal(rowStyle);
        //                            EditorGUILayout.EnumPopup(levels.gridArray[x, y].cellType, enumStyle);
        //                            EditorGUILayout.EndHorizontal();
        //                        }
        //                    }
        //                }
        //                EditorGUILayout.EndVertical();
        //            }
        //            EditorGUILayout.EndHorizontal();

        //        }
        //        }

        //    }
        //}
    }


    



}

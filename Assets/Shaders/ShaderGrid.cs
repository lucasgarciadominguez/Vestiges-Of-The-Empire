using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class ShaderGrid : MonoBehaviour
{
    [SerializeField]
    private int maxGold;

    [SerializeField]
    private int minGold;

    [SerializeField]
    private int maxIron;

    [SerializeField]
    private int minIron;

    [SerializeField]
    private int maxMarble;

    [SerializeField]
    private int minMarble;

    public Material heatMapMatGold;
    public Material heatMapMatIron;
    public Material heatMapMatMarble;

    public GameObject grassGO;
    public PlaceBuilding builder;
    private float[] arrayXPointsGold;
    private float[] arrayYPointsGold;
    private float[] arrayXPointsIron;
    private float[] arrayYPointsIron;
    private float[] arrayXPointsMarble;
    private float[] arrayYPointsMarble;
    public int numberVeinsGold;
    public int numberVeinsIron;
    public int numberVeinsMarble;

    [SerializeField]
    private float[] mPoints;

    [SerializeField]
    public float[] rangesShader = new float[5];

    private int[] totalSupplyGold;
    private int[] totalSupplyIron;
    private int[] totalSupplyMarble;

    [SerializeField]
    private GameObject vein;

    [SerializeField]
    private GameObject veins;

    // Start is called before the first frame update
    private void Start()
    {
        totalSupplyGold = new int[numberVeinsGold];
        totalSupplyIron = new int[numberVeinsIron];
        totalSupplyMarble = new int[numberVeinsMarble];

        heatMapMatGold.mainTexture =  grassGO.GetComponent<MeshRenderer>().material.mainTexture;
        arrayXPointsGold = new float[numberVeinsGold];
        arrayYPointsGold = new float[numberVeinsGold];

        for (int i = 0; i < numberVeinsGold; i++)
        {
            arrayXPointsGold[i] = (UnityEngine.Random.Range(0f, 1f));
            arrayYPointsGold[i] = (UnityEngine.Random.Range(0f, 1f));
            totalSupplyGold[i] = (int)(UnityEngine.Random.Range(minGold, maxGold));
        }
        DrawHeatMap(heatMapMatGold, numberVeinsGold, arrayXPointsGold, arrayYPointsGold, Mineral.Gold);

        heatMapMatIron.mainTexture = grassGO.GetComponent<MeshRenderer>().material.mainTexture;
        arrayXPointsIron = new float[numberVeinsIron];
        arrayYPointsIron = new float[numberVeinsIron];

        for (int i = 0; i < numberVeinsIron; i++)
        {
            arrayXPointsIron[i] = (UnityEngine.Random.Range(0f, 1f));
            arrayYPointsIron[i] = (UnityEngine.Random.Range(0f, 1f));
            totalSupplyIron[i] = (int)(UnityEngine.Random.Range(minIron, maxIron));
        }
        DrawHeatMap(heatMapMatIron, numberVeinsIron, arrayXPointsIron, arrayYPointsIron, Mineral.Iron);

        heatMapMatMarble.mainTexture = grassGO.GetComponent<MeshRenderer>().material.mainTexture;
        arrayXPointsMarble = new float[numberVeinsMarble];
        arrayYPointsMarble = new float[numberVeinsMarble];

        for (int i = 0; i < numberVeinsMarble; i++)
        {
            arrayXPointsMarble[i] = (UnityEngine.Random.Range(0f, 1f));
            arrayYPointsMarble[i] = (UnityEngine.Random.Range(0f, 1f));
            totalSupplyMarble[i] = (int)(UnityEngine.Random.Range(minMarble, maxMarble));
        }
        DrawHeatMap(heatMapMatMarble, numberVeinsMarble, arrayXPointsMarble, arrayYPointsMarble, Mineral.Marble);
    }

    //public void ChangeMaterialSelected(int num)
    //{
    //    if (num == 1)
    //    {
    //        builder.ChangeExtractingResources(heatMapMatGold, veins.GetComponent<Veins>().veinsGold, Mineral.Gold);
    //    }
    //    else if (num == 2)
    //    {
    //        builder.ChangeExtractingResources(heatMapMatIron, veins.GetComponent<Veins>().veinsIron, Mineral.Iron);
    //    }
    //    else if (num == 3)
    //    {
    //        builder.ChangeExtractingResources(heatMapMatMarble, veins.GetComponent<Veins>().veinsMarble, Mineral.Marble);
    //    }
    //}

    public float distsq(float distance)    //gets a value between 0 to 1
    {
        float area_of_effect_size = 0.25f;
        float d = (Mathf.Clamp(Mathf.Pow(distance / (area_of_effect_size * 30), 2f), 0.0f, 1.0f));
        // Debug.Log(d);
        //if d gets negative it will returns you 0. If its >1, d will return 1. That's the use in this case for max()
        //pow() is only for getting always positive values
        //restricts it for making only not greater than area_of_effect_size percent of the texture
        //if the distance(a,b) returns 1 it would be (0-1) ==> 1 and its the maximum value
        return d;
    }

    public decimal getPercentForPixel(float distance)
    {
        if (distance <= rangesShader[0])
        {
            return 100;
        }
        if (distance >= rangesShader[4])
        {
            return 0;
        }
        else
        {
            decimal d = (decimal)(10 / distance);
            return (Decimal.Round(d));
        }
    }

    public void DrawHeatMap(Material mat, int numberVeinsMaterial, float[] arrayXPointsMineral, float[] arrayYPointsMineral, Mineral type)
    {
        int numberVeins = numberVeinsMaterial;
        float[] arrayXPoints = arrayXPointsMineral;
        float[] arrayYPoints = arrayYPointsMineral;
        mPoints = new float[32 * 3];
        for (int i = 0; i < numberVeins; i++)
        {
            mPoints[i * 3] = arrayXPoints[i];
            mPoints[i * 3 + 1] = arrayYPoints[i];
            mPoints[i * 3 + 2] = 3;
        }

        mat.SetInt("_HitCount", numberVeins);
        mat.SetFloatArray("pointranges", rangesShader);

        mat.SetFloatArray("_Hits", mPoints);
        for (int i = 0; i < numberVeins; i++)
        {
            Vector3 position = new Vector3((arrayXPoints[i] * 50), 0, (arrayYPoints[i] * 50));
            GameObject vein = this.vein;
            vein.GetComponent<Vein>().SetLocation(position);
            vein.GetComponent<Vein>().SetMineralType(type);
            vein.GetComponent<Vein>().SetSupply(ReturnSupplyValue(i, type));
            Instantiate(vein, position, Quaternion.identity, veins.transform);
        }
    }

    public int ReturnSupplyValue(int value, Mineral type)
    {
        switch (type)
        {
            case Mineral.None:
                break;

            case Mineral.Gold:
                return totalSupplyGold[value];

            case Mineral.Iron:
                return totalSupplyIron[value];

            case Mineral.Marble:
                return totalSupplyMarble[value];

            default:
                break;
        }
        return 0;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.magenta;
    //    for (int i = 0; i < numberVeinsGold; i++)
    //    {
    //        Gizmos.DrawSphere(new Vector3((arrayXPointsGold[i] * 50), 0, (arrayYPointsGold[i] * 50)), 0.75f);

    //    }
    //    Gizmos.color = Color.yellow;
    //    for (int i = 0; i < numberVeinsIron; i++)
    //    {
    //        Gizmos.DrawSphere(new Vector3((arrayXPointsIron[i] * 50), 0, (arrayYPointsIron[i] * 50)), 0.75f);

    //    }
    //    Gizmos.color = Color.red;
    //    for (int i = 0; i < numberVeinsMarble; i++)
    //    {
    //        Gizmos.DrawSphere(new Vector3((arrayXPointsMarble[i] * 50), 0, (arrayYPointsMarble[i] * 50)), 0.75f);

    //    }
    //}
}
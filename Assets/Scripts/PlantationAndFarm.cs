using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlantationAndFarm : MonoBehaviour
{
    [SerializeField]
    private WorldGenerator worldGenerator;
    [SerializeField]
    private PlaceBuilding builder;
    public void ChangeMaterialSelected(int num,CellType type)
    {
        switch (type)
        {
            case CellType.Mine:
                if (num == 1)
                {
                    builder.ChangeExtractingResources(worldGenerator.heatMapsItems.Find(j => j.nameNoiseMap == ExtractiveResources.Gold).GetMaterial(), ExtractiveResources.Gold);
                }
                else if (num == 2)
                {
                    builder.ChangeExtractingResources(worldGenerator.heatMapsItems.Find(j => j.nameNoiseMap == ExtractiveResources.Iron).GetMaterial(), ExtractiveResources.Iron);
                }
                else if (num == 3)
                {
                    builder.ChangeExtractingResources(worldGenerator.heatMapsItems.Find(j => j.nameNoiseMap == ExtractiveResources.Marble).GetMaterial(), ExtractiveResources.Marble);
                }
                else if (num == 4)
                {
                    builder.ChangeExtractingResources(worldGenerator.heatMapsItems.Find(j => j.nameNoiseMap == ExtractiveResources.Silver).GetMaterial(), ExtractiveResources.Silver);
                }
                break;
            case CellType.Plantation:
                if (num == 1)
                {
                    builder.ChangeExtractingResources(worldGenerator.heatMapsItems.Find(j => j.nameNoiseMap == ExtractiveResources.Wheat).GetMaterial(),ExtractiveResources.Wheat);
                }
                else if (num == 2)
                {
                    builder.ChangeExtractingResources(worldGenerator.heatMapsItems.Find(j => j.nameNoiseMap == ExtractiveResources.Grape).GetMaterial(), ExtractiveResources.Grape);
                }
                else if (num == 3)
                {
                    builder.ChangeExtractingResources(worldGenerator.heatMapsItems.Find(j => j.nameNoiseMap == ExtractiveResources.Oil).GetMaterial(), ExtractiveResources.Oil);
                }
                else if (num == 4)
                {
                    builder.ChangeExtractingResources(worldGenerator.heatMapsItems.Find(j => j.nameNoiseMap == ExtractiveResources.Linen).GetMaterial(), ExtractiveResources.Linen);
                }
                break;
            case CellType.Farm:
                if (num == 1)
                {
                    builder.ChangeExtractingResources(worldGenerator.heatMapsItems.Find(j => j.nameNoiseMap == ExtractiveResources.Cow).GetMaterial(), ExtractiveResources.Cow);
                }
                else if (num == 2)
                {
                    builder.ChangeExtractingResources(worldGenerator.heatMapsItems.Find(j => j.nameNoiseMap == ExtractiveResources.Pig).GetMaterial(), ExtractiveResources.Pig);
                }
                break;
            default:
                break;
        }

    }
}

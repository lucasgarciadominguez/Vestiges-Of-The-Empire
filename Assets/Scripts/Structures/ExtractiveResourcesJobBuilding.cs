using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractiveResourcesJobBuilding : JobBuilding
{
    [SerializeField]
    public int extractedSupplyForEachCycleJob;
    [SerializeField]
    public int trueEfectiviness;
    public int efectiveness;
    public void UpdateResourcesForTheGameManager()
    {
        gameManager.IncreaseRecourseAmount(resourceExtractingOrProducing, trueEfectiviness);
    }
}

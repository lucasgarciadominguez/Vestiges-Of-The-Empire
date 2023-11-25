using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElaboratedResourcesJobBuilding : JobBuilding
{
    [SerializeField]
    public int producedSupplyForEachCycleJob;
    public Sprite imageItemNeeded;
    public ItemNeeded resourceNeeded;

    public void UpdateResourcesForTheGameManager()
    {

        gameManager.IncreaseRecourseAmount(resourceExtractingOrProducing, producedSupplyForEachCycleJob);

    }
    public void GenerateElaboratedResources()
    {
        ItemNeeded[] itemNeededs = new ItemNeeded[] { resourceNeeded };
        if (gameManager.CanReduceStats(itemNeededs))
        {
            if (canWork)
            {
                gameManager.ReduceStats(itemNeededs);
                UpdateResourcesForTheGameManager();
            }
            if (states.Exists(j => j == StatesStructures.NoSupplyElaboratedResources))
            {
                DeleteUIAdvise(StatesStructures.NoSupplyElaboratedResources);
                DeleteState(StatesStructures.NoSupplyElaboratedResources);
            }

        }
        else
        {
            if (canWork)
            {
                states.Add(StatesStructures.NoSupplyElaboratedResources);
                CreateUIForNewState(StatesStructures.NoSupplyElaboratedResources);  //makes the ui show that there is no supply
            }

        }
        CheckIfCanWork();

    }
}

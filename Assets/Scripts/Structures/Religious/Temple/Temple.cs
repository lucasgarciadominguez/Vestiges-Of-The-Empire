using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temple : ExtractiveResourcesJobBuilding
{
    // Start is called before the first frame update
    private void Start()
    {
        cellType = CellType.Temple;
        resourceExtractingOrProducing = ResourcesType.Faith;
        functionJob = new FunctionTimer(GenerateFaith, timeForDoingFunctionJob, true);
        SetAllJobRequirements();
    }

    private void Update()
    {
        UpdateAllStructureRequirements();
        if (isBuild)
        {
            UpdateAllJobRequirements();
            if (canWork)
            {
                functionJob.Update();
            }
        }
    }
    public void GenerateFaith()
    {
        trueEfectiviness= extractedSupplyForEachCycleJob;
        UpdateResourcesForTheGameManager();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailorShop : ElaboratedResourcesJobBuilding
{
    private void Start()
    {
        resourceExtractingOrProducing = ResourcesType.Clothes;
        radiusAffect = 13f;
        functionJob = new FunctionTimer(GenerateElaboratedResources, timeForDoingFunctionJob, true);
        SetAllJobRequirements();

    }

    private void Update()
    {
        UpdateAllStructureRequirements();
        if (isBuild)
        {
            UpdateAllJobRequirements();
            functionJob.Update();
        }
    }
}

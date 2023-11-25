using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bakery : ElaboratedResourcesJobBuilding
{
    // Start is called before the first frame update
    private void Start()
    {
        resourceExtractingOrProducing = ResourcesType.Bread;
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumperCamp : ExtractiveResourcesJobBuilding
{
    [SerializeField]
    private GameObject actualTreeCutting;




    // Start is called before the first frame update
    private void Start()
    {
        resourceExtractingOrProducing = ResourcesType.Wood;
        functionJob = new FunctionTimer(CutTree, timeForDoingFunctionJob, true);
        SetAllJobRequirements();
        GetNearestTree();

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
    public void GetNearestTree()
    {

        float distance = radiusAffect;
        for (int i = 0; i < grid.trees.Count; i++)
        {
            if (distance < radiusAffect)
            {
                //Debug.Log(distance);
            }
            if (distance > Vector3.Distance(actualPosition, grid.trees[i].transform.position))
            {
                distance = Vector3.Distance(actualPosition, grid.trees[i].transform.position);

                actualTreeCutting = grid.trees[i].gameObject;
            }
            else
            {
                //Debug.Log(distance);
            }
        }
    }

    public void CutTree()
    {
        if (actualTreeCutting != null)
        {
            int index = grid.trees.FindIndex(c => c.Equals(actualTreeCutting));
            grid.trees.RemoveAt(index);
            Destroy(actualTreeCutting.gameObject);
            GetNearestTree();
            UpdateResourcesForTheGameManager();
        }
        else
        {
            states.Add(StatesStructures.NoSupply);
            CreateUIForNewState(StatesStructures.NoSupply);  //makes the ui show that there is no supply
        }

    }

}
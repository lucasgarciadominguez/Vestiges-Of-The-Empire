using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobBuilding : StructureParent
{
    public float radiusAffect=13;
    public int numWorkers = 2;
    public bool allJobPositionsCovered = false;
    public List<Person> workers = new List<Person>();
    public People listWorkers;
    public TypePerson[] typePeopleNeeded;
    public FunctionTimer functionFindWorkers;
    public FunctionTimer functionJob;
    public FunctionTimer functionSatisfyDemands;
    public float timeForDoingFunctionJob=5f;
    float timeForCheckingIfCanBeAccomplishedDemands = 15f;
    public float timeBetweenCheckFunctionFindWorkers = 5;
    public List<HouseBuilding> housesBuildingAffecting;
    public ResourcesType resourceExtractingOrProducing = ResourcesType.None;

    public void SetAllJobRequirements()
    {
        SetAllStructureRequirements();
        FindWorkersList();
        functionSatisfyDemands= new FunctionTimer(CheckIfCanSatisfyDemands, timeForCheckingIfCanBeAccomplishedDemands, true);
        functionFindWorkers = new FunctionTimer(FindWorkers, timeBetweenCheckFunctionFindWorkers, true);

    }
    public void UpdateAllJobRequirements()
    {
        functionFindWorkers.Update();
        functionSatisfyDemands.Update();
        CheckAllFunctionsJobComprobations();
    }
    public void CheckIfCanSatisfyDemands()
    {
        Debug.Log("trying to check how many buildings can we affect");
        List<HouseBuilding> houses = Houses.ReturnListHouses();
        housesBuildingAffecting.Clear();

        float distance = radiusAffect;
        if (houses.Count > 0)
        {
            foreach (var item in houses.ToList())
            {
                if (distance > Vector3.Distance(transform.position, item.transform.position))
                {
                    distance = Vector3.Distance(transform.position, item.transform.position);

                    housesBuildingAffecting.Add(item);
                }
                else
                {
                }
            }
        }
        foreach (var item in housesBuildingAffecting)
        {
            foreach (Person p in item.peopleLivingInHouse)
            {
                foreach (Demand demand in p.demands)
                {
                    if (demand.TypeItemNeeded ==resourceExtractingOrProducing)  
                    {
                        demand.canBeSatisfied= true;
                    }
                }
            }
        }
    }
    public void CheckAllFunctionsJobComprobations()
    {
        if (allJobPositionsCovered)
        {
            functionFindWorkers.SetContinueSearching(false);
        }
    }
    public void FindWorkersList()
    {
        if (listWorkers == null)
        {
            People[] list = FindObjectsOfType<People>();
            foreach (var item in list)
            {
                if (item.typeList == TypePeople.FreePeople)
                {
                    listWorkers = item;
                }
            }
        }
    }

    public void FindWorkers()
    {
        Debug.Log("Checking if there's workers...");
        if (workers.Count != numWorkers)
        {
            int workersNeeded = numWorkers - workers.Count;
            for (int i = 0; i < workersNeeded; i++)
            {
                Person worker = listWorkers.GetPeopleFromtheListUnemployed(typePeopleNeeded,transform.position,radiusAffect,this);
                if (worker != null)
                {
                    workers.Add(worker);
                }
                else
                {
                    Debug.LogWarning("There's no workers for that position!");
                }
            }
        }
        if (workers.Count > 0)
        {
            if (states.Exists(j => j == StatesStructures.NoWorkers))
            {
                int num = states.FindIndex(j => j == StatesStructures.NoWorkers);
                states.RemoveAt(num);
                DeleteUIAdvise(StatesStructures.NoWorkers);
            }
        }
        if (workers.Count == 0)
        {
            if (!states.Exists(j => j == StatesStructures.NoWorkers))
            {
                states.Add(StatesStructures.NoWorkers);
                CreateUIForNewState(StatesStructures.NoWorkers);
            }
        }
        if (workers.Count == numWorkers)
        {
            allJobPositionsCovered = true;
        }
        CheckIfCanWork();
    }


}
public class FunctionTimer
{
    private Action action;
    private float timer;
    private float timerCounter;
    private bool isDestroyed;
    private bool continueSearching=true;
    public FunctionTimer (Action action, float timer,bool isMoreThanOneTimeActivated)
    {
        this.action = action;
        this.timer = timer;
        this.timerCounter = timer;
        isDestroyed= false;
        continueSearching = isMoreThanOneTimeActivated;
    }
    public void Update()
    {

        if (!isDestroyed)
        {
            timer-=Time.deltaTime;
            if (timer<0)
            {
                action();
                DestroySelf();
            }
        }
        else
        {
            if (continueSearching)
            {
                isDestroyed = false;
                timer=timerCounter;
            }
        }
    }
    void DestroySelf()
    {
        isDestroyed= true;
    }
    public void SetContinueSearching(bool canWork)
    {
        continueSearching =canWork;
    }
}
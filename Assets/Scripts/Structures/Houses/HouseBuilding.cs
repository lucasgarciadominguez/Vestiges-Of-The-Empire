using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HouseBuilding : StructureParent
{
    [SerializeField]
    private int amountPeopleLiving;
    [SerializeField]
    GameObject peopleLivingMale;
    [SerializeField]
    GameObject peopleLivingFemale;
    public FunctionTimer functionSatisfyDemandsPeopleLiving;
    public float timeForSatisfyDemands=10f;
    public List<Person> peopleLivingInHouse=new List<Person>();
    bool check=false;
    Person lastPerson;
    // Start is called before the first frame update
    private void Start()
    {

        SetAllStructureRequirements();
        if (canWork)
        {
            CreatePeopleLiving();

            check= true;
        }
    }
    private void Update()
    {
        UpdateAllStructureRequirements();
        if (canWork)
        {
            if (peopleLivingInHouse.Count>0)
            {
                functionSatisfyDemandsPeopleLiving.Update();


            }
            if (!check)
            {
                check = true;
                CreatePeopleLiving();
            }
        }
    }
    public void SatisftyDemandsPoblation()
    {
        foreach (var item in peopleLivingInHouse)
        {
            item.SatisfyDemands(gameManager);
        }
    }
    void CreatePeopleLiving()
    {
        int counter = 0;
        for (int i = 0; i < amountPeopleLiving; i++)
        {
            if (counter==0)
            {
                string[] nameAndSurname=ListNamesAndSurnames.ReturnNamesAndSurnames(true,true);
                GameObject person = Instantiate(peopleLivingMale,CalculatePositionForHabitants(), Quaternion.identity);
                person.name = nameAndSurname[0] + " " + nameAndSurname[1];
                person.GetComponent<Person>().isMale=true;
                person.GetComponent<Person>().SetHouse(this);
                person.GetComponent<Person>(). Name = nameAndSurname[0];
                Debug.Log(person.GetComponent<Person>().Name);
                person.GetComponent<Person>().SurnameFamily = nameAndSurname[1];
                person.GetComponent<Person>().target = transform.position;
                peopleLivingInHouse.Add(person.GetComponent<Person>());
                lastPerson = person.GetComponent<Person>();
                counter++;
            }
            else if (counter == 1)
            {
                string[] nameAndSurname = ListNamesAndSurnames.ReturnNamesAndSurnames(false, false);
                GameObject person = Instantiate(peopleLivingFemale, CalculatePositionForHabitants(), Quaternion.identity);
                person.name = nameAndSurname[0] + " " + nameAndSurname[1];
                person.GetComponent<Person>().isMale = false;
                person.GetComponent<Person>().SetHouse(this);
                person.GetComponent<Person>().Name = nameAndSurname[0];
                person.GetComponent<Person>().SurnameFamily = nameAndSurname[1];
                person.GetComponent<Person>().target = transform.position;

                person.GetComponent<Person>().family=lastPerson;
                lastPerson.family = person.GetComponent<Person>();
                peopleLivingInHouse.Add(person.GetComponent<Person>());
                counter=0;
            }

        }
        functionSatisfyDemandsPeopleLiving = new FunctionTimer(SatisftyDemandsPoblation,timeForSatisfyDemands,true);
    }
    Vector3 CalculatePositionForHabitants()
    {
        if (rotationType==RotationType.RightDown)
        {
            Vector3 newPosition = new Vector3(transform.position.x + ((float)rowsOcuppation / 2f), 0, transform.position.z - ((float)columnsOcuppation / 2f));
            return newPosition;
        }
        else if (rotationType == RotationType.LeftDown)
        {
            Vector3 newPosition = new Vector3(transform.position.x - ((float)columnsOcuppation / 2f), 0, transform.position.z - ((float)rowsOcuppation / 2f));
            return newPosition;
        }
        else if (rotationType==RotationType.LeftTop)
        {
            Vector3 newPosition = new Vector3(transform.position.x - ((float)rowsOcuppation / 2f), 0, transform.position.z + ((float)columnsOcuppation / 2f));
            return newPosition;
        }
        else if (rotationType==RotationType.RightTop)
        {
            Vector3 newPosition = new Vector3(transform.position.x + ((float)columnsOcuppation / 2f), 0, transform.position.z + ((float)rowsOcuppation / 2f));
            return newPosition;
        }
        return Vector3.zero;

    }
}
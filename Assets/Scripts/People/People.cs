using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum TypePeople{
    None,
    Slave,
    FreePeople
}

public class People : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    public TypePeople typeList= TypePeople.None;
    [SerializeField]
    public List<Person> listPeople=new List<Person>();
    [SerializeField]
    public List<Person> listPeopleUnemployed = new List<Person>();
    public void AddPeopleToList(Person person)  //add people to the list
    {
        listPeople.Add(person);
        gameManager.RefreshUIPoblation(typeList,listPeople.Count);
    }
    public void AddPeopleToListUnemployed(Person person)  //add people to the list
    {
        listPeopleUnemployed.Add(person);
        
    }
    public Person GetPeopleFromtheListUnemployed(TypePerson[] typePersonNeededForTheJob, Vector3 positionObjective, float distanceRadius,JobBuilding job)  //obtains the first person in the list who fulfill the demands for the job (for the typePersonNeededForTheJob, the first one in the array is the most important and the second one is more of a optional
    {
        float distance = distanceRadius;
        Person nearestPerson=new Person();
        if (listPeople.Count > 0)
        {
            if (listPeopleUnemployed.Exists(x => x.personType == typePersonNeededForTheJob[0] || x.personType == typePersonNeededForTheJob[1]))
            {
                for (int i = 0; i < listPeopleUnemployed.Count; i++)
                {
                    if (distance > Vector3.Distance(positionObjective, listPeopleUnemployed[i].ReturnPositionRoof()))
                    {
                        if (listPeopleUnemployed[i].personType == typePersonNeededForTheJob[0] || listPeopleUnemployed[i].personType == typePersonNeededForTheJob[1])
                        {
                            distance = Vector3.Distance(positionObjective, listPeopleUnemployed[i].ReturnPositionRoof());

                            nearestPerson = listPeopleUnemployed[i];
                        }

                    }
                    else
                    {
                    }
                }


            }
        }
        //Person p = listPeopleUnemployed.Find(x => x.personType == typePersonNeededForTheJob[0] || x.personType == typePersonNeededForTheJob[1]); //find the person required,
        if (nearestPerson.personType!=TypePerson.None)
        {
            nearestPerson.SetEmploy(true, job);
            listPeopleUnemployed.Remove(nearestPerson);   //removes the person from the list
            return nearestPerson;
        }
        else
        {
            return null;
        }


    }
}

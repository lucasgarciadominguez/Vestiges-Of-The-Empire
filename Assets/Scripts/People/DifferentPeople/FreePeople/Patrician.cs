using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrician : Person
{
    // Start is called before the first frame update
    void Start()
    {
        personType= TypePerson.Patrician;
        demands = new Demand[6];
        demands[0] = new Demand(ResourcesType.Bread, 2);
        demands[1] = new Demand(ResourcesType.Meat, 2);
        demands[2] = new Demand(ResourcesType.Faith, 2);
        demands[3] = new Demand(ResourcesType.Oil, 2);
        demands[4] = new Demand(ResourcesType.Clothes, 2);
        demands[5] = new Demand(ResourcesType.Wine, 2);
        AddPersonToTheList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

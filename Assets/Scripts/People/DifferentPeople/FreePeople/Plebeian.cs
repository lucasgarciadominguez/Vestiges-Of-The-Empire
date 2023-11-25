using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plebeian : Person
{

    // Start is called before the first frame update
    void Start()
    {
        personType = TypePerson.Plebeian;
        demands = new Demand[3];
        demands[0] = new Demand(ResourcesType.Bread, 2);
        demands[1] = new Demand(ResourcesType.Meat, 2);
        demands[2] = new Demand(ResourcesType.Clothes, 2);

        AddPersonToTheList();
    }
}

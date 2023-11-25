using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Houses //list with all the houses types displayed in the world
{
    [SerializeField]
    static List<HouseBuilding> houses=new List<HouseBuilding>();
    //TODO use it for statics of the people or the level of the houses

    public static void AddHouse(HouseBuilding house)
    {
        houses.Add(house);
    }
    public static List<HouseBuilding> ReturnListHouses()
    {
        return houses;
    }
}



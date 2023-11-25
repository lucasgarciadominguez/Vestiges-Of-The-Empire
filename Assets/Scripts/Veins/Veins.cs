using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veins : MonoBehaviour
{
    public List<Vein> veinsGold =new List<Vein>();
    public List<Vein> veinsIron = new List<Vein>();
    public List<Vein> veinsMarble = new List<Vein>();

    public void AddVein(Vein vein,Mineral type)
    {
        switch (type)
        {
            case Mineral.None:
                break;
            case Mineral.Gold:
                veinsGold.Add(vein);
                break;
            case Mineral.Iron:
                veinsIron.Add(vein);
                break;
            case Mineral.Marble:
                veinsMarble.Add(vein);
                break;
            default:
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopInterface
{
    bool CanBuy(ItemNeeded[] resourcesNeeded);
    void ReduceStats(ItemNeeded[] resourcesNeededToReduce);
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RingElement", menuName = "RingMenu/Element", order = 2)]
public class RingMenuElement : ScriptableObject
{
    public string Name;
    public string Description;
    public string DescriptionBuildingMenu;
    public Sprite Icon;
    public Ring NextRing;
    public GameObject PlaceHolderObjectCorrect;
    public GameObject PlaceHolderObjectIncorrect;
    public ItemNeeded[] ResourcesNeededForBuilding;
}
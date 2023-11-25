using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (fileName = "RingMenu", menuName = "RingMenu/Ring", order = 1)]
public class Ring : ScriptableObject
{
    public RingMenuElement[] elements;
}

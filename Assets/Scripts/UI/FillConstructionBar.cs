using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillConstructionBar : MonoBehaviour
{
    [SerializeField]
    Image constructionBar;

    public void UpdateConstructionBar(float actualTime,float maxTime)
    {
        constructionBar.fillAmount = actualTime / maxTime;
    }
}

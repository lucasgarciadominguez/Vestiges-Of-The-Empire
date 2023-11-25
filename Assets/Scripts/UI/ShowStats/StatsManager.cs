using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField]
    UIPause pauseUI;
    [SerializeField]
    Structures structures;
    [SerializeField]
    GameObject otherStatsInspector;
    [SerializeField]
    GameObject peopleStatsInspector;
    [SerializeField]
    GameObject arrowRight;
    [SerializeField]
    GameObject arrowLeft;
    [SerializeField]
    GameObject closeIcon;
    [SerializeField]
    SwitchInspectorAndLoadItems switchInspector;
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    People freePeopleList;


    [SerializeField]
    TextMeshProUGUI averageOil;
    [SerializeField]
    TextMeshProUGUI averageClothes;
    [SerializeField]
    TextMeshProUGUI averageWine;
    [SerializeField]
    TextMeshProUGUI averageBread;
    [SerializeField]
    TextMeshProUGUI averageMeat;

    [SerializeField]
    TextMeshProUGUI patricianOil;
    [SerializeField]
    TextMeshProUGUI patricianClothes;
    [SerializeField]
    TextMeshProUGUI patricianWine;
    [SerializeField]
    TextMeshProUGUI patricianBread;
    [SerializeField]
    TextMeshProUGUI patricianMeat;

    [SerializeField]
    TextMeshProUGUI freedmanOil;
    [SerializeField]
    TextMeshProUGUI freedmanClothes;
    [SerializeField]
    TextMeshProUGUI freedmanWine;
    [SerializeField]
    TextMeshProUGUI freedmanBread;
    [SerializeField]
    TextMeshProUGUI freedmanMeat;

    [SerializeField]
    TextMeshProUGUI plebeianOil;
    [SerializeField]
    TextMeshProUGUI plebeianClothes;
    [SerializeField]
    TextMeshProUGUI plebeianWine;
    [SerializeField]
    TextMeshProUGUI plebeianBread;
    [SerializeField]
    TextMeshProUGUI plebeianMeat;

    [SerializeField]
    TextMeshProUGUI weapons;
    [SerializeField]
    TextMeshProUGUI iron;
    [SerializeField]
    TextMeshProUGUI gold;
    [SerializeField]
    TextMeshProUGUI peoplePatrician;
    [SerializeField]
    TextMeshProUGUI peopleFreedman;
    [SerializeField]
    TextMeshProUGUI peoplePlebeian;
    [SerializeField]
    TextMeshProUGUI patricianfamilies;
    [SerializeField]
    TextMeshProUGUI freedmanfamilies;
    [SerializeField]
    TextMeshProUGUI plebeianfamilies;
    [SerializeField]
    TextMeshProUGUI patricianUnmploy;
    [SerializeField]
    TextMeshProUGUI freedmanUnmploy;
    [SerializeField]
    TextMeshProUGUI plebeianUnmploy;
    public void LoadStatsPanel()
    {
        switchInspector.HideInspectors();
        if (!peopleStatsInspector.activeSelf) {
            peopleStatsInspector.SetActive(true);
            arrowRight.SetActive(true);
            arrowLeft.SetActive(false);
            closeIcon.SetActive(true);
            pauseUI.PauseForMenuStats();
            LoadAmountPeopleStats();
        }
        else
        {
            OnClose();
        }

    }
    public void OnClickOnArrowRight()
    {
        otherStatsInspector.SetActive(true);
        LoadAmountStatsOtherStats();
        peopleStatsInspector.SetActive(false);
        arrowRight.SetActive(false);
        arrowLeft.SetActive(true);
    }
    public void OnClickOnArrowLeft()
    {
        otherStatsInspector.SetActive(false);
        peopleStatsInspector.SetActive(true);
        arrowRight.SetActive(true);
        arrowLeft.SetActive(false);
    }
    public void OnClose()
    {
        closeIcon.SetActive(false);
        otherStatsInspector.SetActive(false);
        peopleStatsInspector.SetActive(false);
        arrowRight.SetActive(false);
        arrowLeft.SetActive(false);
        pauseUI.StopPauseForMenuStats();

    }
    //demands[0] = new Demand(ResourcesType.Bread, 2);
    //demands[1] = new Demand(ResourcesType.Meat, 2);
    //demands[2] = new Demand(ResourcesType.Faith, 2);
    //demands[3] = new Demand(ResourcesType.Oil, 2);
    //demands[4] = new Demand(ResourcesType.Clothes, 2);
    //demands[5] = new Demand(ResourcesType.Wine, 2);
    private void Start()
    {
        CalculateAverages();
        CalculatePatricians();
        CalculateFreedmans();
        CalculatePlebeian();
    }
    public void LoadAmountPeopleStats()
    {
        if (structures.structures.Count!=0)
        {
            CalculateAverages();
            CalculatePatricians();
            CalculateFreedmans();
            CalculatePlebeian();
        }

    }
    public void LoadAmountStatsOtherStats()
    {
        weapons.text = gameManager.items.Find(j => j.TypeItemNeeded == ResourcesType.Weapons).amountItem.ToString();
        iron.text = gameManager.items.Find(j => j.TypeItemNeeded == ResourcesType.Iron).amountItem.ToString();
        gold.text = gameManager.items.Find(j => j.TypeItemNeeded == ResourcesType.Gold).amountItem.ToString();
        peoplePatrician.text = freePeopleList.listPeople.FindAll(j => j.personType == TypePerson.Patrician).Count.ToString();
        peopleFreedman.text = freePeopleList.listPeople.FindAll(j => j.personType == TypePerson.Freedman).Count.ToString();
        peoplePlebeian.text = freePeopleList.listPeople.FindAll(j => j.personType == TypePerson.Plebeian).Count.ToString();

        patricianfamilies.text = ((int)((freePeopleList.listPeople.FindAll(j => j.personType == TypePerson.Patrician).Count)/2)).ToString();
        freedmanfamilies.text = ((int)((freePeopleList.listPeople.FindAll(j => j.personType == TypePerson.Freedman).Count)/2)).ToString();
        plebeianfamilies.text = ((int)((freePeopleList.listPeople.FindAll(j => j.personType == TypePerson.Plebeian).Count) / 2)).ToString();

        patricianUnmploy.text = ((int)((freePeopleList.listPeopleUnemployed.FindAll(j => j.personType == TypePerson.Patrician).Count))).ToString();
        freedmanUnmploy.text = ((int)((freePeopleList.listPeopleUnemployed.FindAll(j => j.personType == TypePerson.Freedman).Count))).ToString();
        plebeianUnmploy.text = ((int)((freePeopleList.listPeopleUnemployed.FindAll(j => j.personType == TypePerson.Plebeian).Count))).ToString();


    }
    public void CalculateAverages()
    {
        int counter = 0;
        float amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Patrician)
            {
                amount += item.demands[3].PercentHappiness;
                counter++;
            }
            else if (item.personType == TypePerson.Freedman)
            {
                amount += item.demands[3].PercentHappiness;
                counter++;

            }

        }
        float result = amount / counter;

        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);
            averageOil.text = result.ToString() + "%";

        }


        counter = 0;
        amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Plebeian)
            {
                amount += item.demands[2].PercentHappiness;

            }
            else
            {
                amount += item.demands[4].PercentHappiness;

            }
            counter++;
        }
        result = amount / counter;

        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);
            averageClothes.text = result.ToString() + "%";

        }


        counter = 0;
        amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Patrician)
            {
                amount += item.demands[5].PercentHappiness;
                counter++;
            }

        }
        result = amount / counter;

        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);
            averageWine.text = result.ToString() + "%";

        }


        counter = 0;
        amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {

            amount += item.demands[0].PercentHappiness;

            counter++;

        }
        result = amount / counter;

        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);
            averageBread.text = result.ToString() + "%";

        }

        counter = 0;
        amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            amount += item.demands[1].PercentHappiness;
            counter++;
        }
        result = amount / counter;

        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);

            averageMeat.text = result.ToString() + "%";

        }
    }
    public void CalculatePatricians()
    {
        int counter = 0;
        float amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType==TypePerson.Patrician)
            {
                amount += item.demands[3].PercentHappiness;
                counter++;
            }

        }
        float result = amount / counter;

        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);

            patricianOil.text = result.ToString() + "%";

        }


        counter = 0;
        amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Patrician)
            {
                amount += item.demands[4].PercentHappiness;
                counter++;
            }
        }
        result = amount / counter;

        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);

            patricianClothes.text = result.ToString() + "%";

        }


        counter = 0;
        amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Patrician)
            {
                amount += item.demands[5].PercentHappiness;
                counter++;
            }
        }
        result = amount / counter;

        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);

            patricianWine.text = result.ToString() + "%";

        }


        counter = 0;
        amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Patrician)
            {
                amount += item.demands[0].PercentHappiness;
                counter++;
            }
        }
        result = amount / counter;

        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);

            patricianBread.text = result.ToString() + "%";

        }

        counter = 0;
        amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Patrician)
            {
                amount += item.demands[1].PercentHappiness;
                counter++;
            }
        }
        result = amount / counter;

        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);

            patricianMeat.text = result.ToString() + "%";

        }
    }
    public void CalculateFreedmans()
    {
        int counter = 0;
        float amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Freedman)
            {
                amount += item.demands[3].PercentHappiness;
                counter++;
            }

        }
        float result = amount / counter;
        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);

            freedmanOil.text = result.ToString() + "%";

        }


        counter = 0;
        amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Freedman)
            {
                amount += item.demands[4].PercentHappiness;
                counter++;
            }
        }
        result = amount / counter;
        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);

            freedmanClothes.text = result.ToString() + "%";

        }


        //counter = 0;
        //amount = 0;
        //foreach (var item in freePeopleList.listPeople)
        //{
        //    if (item.personType == TypePerson.Freedman)
        //    {
        //        amount += item.demands[5].PercentHappiness;
        //        counter++;
        //    }
        //}
        //result = amount / counter;
        freedmanWine.text = "-";


        counter = 0;
        amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Freedman)
            {
                amount += item.demands[0].PercentHappiness;
                counter++;
            }
        }
        result = amount / counter;
        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);

            freedmanBread.text = result.ToString() + "%";

        }

        counter = 0;
        amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Freedman)
            {
                amount += item.demands[1].PercentHappiness;
                counter++;
            }
        }
        result = amount / counter;
        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);

            freedmanMeat.text = result.ToString() + "%";

        }
    }
    public void CalculatePlebeian()
    {
        plebeianOil.text = "-";
        plebeianWine.text = "-";


        int counter = 0;
        float amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Plebeian)
            {
                amount += item.demands[2].PercentHappiness;
                counter++;
            }
        }
        float result = amount / counter;
        if (!float.IsNaN( result))
        {
            result = Convert.ToInt32(result);

            plebeianClothes.text = result.ToString() + "%";

        }



        counter = 0;
        amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Plebeian)
            {
                amount += item.demands[0].PercentHappiness;
                counter++;
            }
        }
        result = amount / counter;
        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);

            plebeianBread.text = result.ToString() + "%";

        }

        counter = 0;
        amount = 0;
        foreach (var item in freePeopleList.listPeople)
        {
            if (item.personType == TypePerson.Plebeian)
            {
                amount += item.demands[1].PercentHappiness;
                counter++;
            }
        }

        result = amount / counter;
        if (!float.IsNaN(result))
        {
            result = Convert.ToInt32(result);

            plebeianMeat.text = result.ToString() + "%";

        }
    }
}

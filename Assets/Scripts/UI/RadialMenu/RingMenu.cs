using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RingMenu : MonoBehaviour
{
    public Ring data;
    public RingCakePiece ringCakePiecePrefab;
    public float gapWidthDegree = 1f;   //space between the ringcakepieces
    public Action<string> callback;
    protected RingCakePiece[] pieces;
    public RingMenu parent;
    public int numberIndexParent;
    public List<RingMenu> childs=new List<RingMenu>();
    public string path;
    public bool itemSelected;
    public int activeElement;
    public UIManager manager;
    public GameManager gameManager;
    // Start is called before the first frame update
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        var stepLength = 360f / data.elements.Length;    //to make it proportional like cheese slices
        var iconDist = Vector3.Distance(ringCakePiecePrefab.icon.transform.position, ringCakePiecePrefab.cakePiece.transform.position); //calculates the distance between the icon and the cakepiece in the prefab
        //Positions it
        pieces = new RingCakePiece[data.elements.Length];   //fills the new ringcakePiece array with all the elements from the ring (previously filled in the class Ring)
        for (int i = 0; i < data.elements.Length; i++)
        {
            pieces[i] = Instantiate(ringCakePiecePrefab, transform); //isntantiate the prefab and sets the current trasnform as the parent
            //set root element
            pieces[i].transform.localPosition = Vector3.zero;   //sets the position and the roptaion to 0 for the rect transform in the gameobject parent
            pieces[i].transform.localRotation = Quaternion.identity;
            //set cake piece
            pieces[i].cakePiece.fillAmount = 1f / data.elements.Length - gapWidthDegree / 360; //calulates how much  image is filled
            pieces[i].cakePiece.transform.localPosition = Vector3.zero;
            pieces[i].cakePiece.transform.localRotation = Quaternion.Euler(0, 0, -stepLength / 2f + gapWidthDegree / 2f + i * stepLength); //sets the rotation
            pieces[i].cakePiece.color = new Color(1f, 1f, 1f, 0.5f);

            //set icon
            pieces[i].icon.transform.localPosition = pieces[i].cakePiece.transform.localPosition + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;   //sets each icon in the opposite position
            pieces[i].icon.sprite = data.elements[i].Icon;  //sets the icon for each image
            //set desciprtion
            pieces[i].description.transform.localPosition = pieces[i].cakePiece.transform.localPosition + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * 80;   //sets each icon in the opposite position

            pieces[i].descriptionText.text= CreateDescriptionElementRadialMenu(data.elements[i]);  //sets the description for each element
        }
    }

    // Update is called once per frame
    private void Update()
    {

        HighlightsTheCorrectCakePieceAndShowsDescription();

        if (Input.GetMouseButtonDown(0))
        {
            gameManager.audioManager.ClickInInspector();
            if (data.elements[activeElement].Name != "NextRing") //if the activeelemnt is not "NextRing" (nextRing is for the cakepieces who has another ring inside))
            {
                itemSelected = true;
            }
            else
            {
                itemSelected = false;
            }
            if (data.elements[activeElement].NextRing != null)  //if the cakepiece has a subring
            {
                if (!childs.Exists(j => j.numberIndexParent == activeElement))
                {
                    var newSubRing = Instantiate(gameObject, transform.parent).GetComponent<RingMenu>();        //it instantiates the same values of this menu ring
                    newSubRing.parent = this;   //sets the parent
                    newSubRing.numberIndexParent = activeElement;
                    this.childs.Add( newSubRing);
                    for (var j = 0; j < newSubRing.transform.childCount; j++)
                        Destroy(newSubRing.transform.GetChild(j).gameObject);   //destroys all the cakepieces of the previous menuring
                    newSubRing.data = data.elements[activeElement].NextRing;    //the data of the new ring is the nextring of the previous one
                    newSubRing.callback = callback;
                }
                else
                {
                    RingMenu ring=childs.Find(j => j.numberIndexParent == activeElement);
                    ring.gameObject.SetActive(true);
                    ring.transform.position = ring.parent.transform.position;
                }
            }
            else
            {
                callback?.Invoke(path);
            }
            gameObject.SetActive(false);

        }
    }
    public string CreateDescriptionElementRadialMenu(RingMenuElement dataElements)
    {
        string dataDescriptionAndItemsAmountNeeded = dataElements.DescriptionBuildingMenu + " (";
        int counter = 0;
        foreach (var item in dataElements.ResourcesNeededForBuilding)
        {
            counter++;

            if (counter == (dataElements.ResourcesNeededForBuilding.Length))
            {
                dataDescriptionAndItemsAmountNeeded += item.TypeItemNeeded.ToString() + " : " + item.amountItem.ToString();

            }
            else
            {
                dataDescriptionAndItemsAmountNeeded += item.TypeItemNeeded.ToString() + " : " + item.amountItem.ToString() + " , ";

            }
        }
        dataDescriptionAndItemsAmountNeeded += ")";
        return dataDescriptionAndItemsAmountNeeded;
    }
    public void HighlightsTheCorrectCakePieceAndShowsDescription()
    {
        var stepLength = 360f / data.elements.Length;
        var mouseAngle = NormalizeAngle(Vector3.SignedAngle(Vector3.up, Input.mousePosition - transform.position, Vector3.forward) + stepLength / 2f); //it returns which angle is having the mouse

        activeElement = (int)(mouseAngle / stepLength); //returns the element
        if (activeElement + 1 < data.elements.Length)   //sums 1 to the element  for getting the correct highlighted cakepiece
        {
            activeElement += 1;
        }
        else
        {
            activeElement = 0;
        }
        for (int i = 0; i < data.elements.Length; i++)  //highlightes the correct cakepiece
        {
            if (i == activeElement)
            {
                pieces[i].cakePiece.color = new Color(1f, 1f, 1f, 0.75f);   //highlightes one if its the correct one

            }
            else
            {
                pieces[i].cakePiece.color = new Color(1f, 1f, 1f, 0.5f);    //the rest cakepieces turn off their highlight

            }
        }
        activeElement = (int)(mouseAngle / stepLength); //change the active elemnt to the correct (else the activeelement is not correct)
        for (int i = 0; i < data.elements.Length; i++)  //changes the description
        {
            if (i == activeElement)
            {
                string cut = pieces[i].descriptionText.text;
                if (cut.EndsWith(" ()"))
                {
                    cut.TrimEnd(')');
                    cut.TrimEnd('(');
                    manager.UpdateDescriptionRadialBuildMenu(cut);

                }
                else
                {
                    manager.UpdateDescriptionRadialBuildMenu(pieces[i].descriptionText.text);

                }

                Debug.Log(pieces[i].descriptionText.text);
            }

        }
    }
    public void CleanInformationMenu()
    {
        manager.UpdateDescriptionRadialBuildMenu("");

    }
    public string ReturnsElement()
    {
        if (itemSelected == true && data.elements[activeElement].NextRing != null)
        {
            return (childs[activeElement].data.elements[childs[activeElement].activeElement].Name);
        }
        else
        {
            return (data.elements[activeElement].Name);
        }
    }

    public string ReturnsElementDescriptionRingMenu()
    {
        if (itemSelected == true && data.elements[activeElement].NextRing != null)
        {
            return (childs[activeElement].data.elements[childs[activeElement].activeElement].DescriptionBuildingMenu);
        }
        else
        {
            return (data.elements[activeElement].DescriptionBuildingMenu);
        }
    }
    public string ReturnsElementDescription()
    {
        if (itemSelected == true && data.elements[activeElement].NextRing != null)
        {
            return (childs[activeElement].data.elements[childs[activeElement].activeElement].Description);
        }
        else
        {
            return (data.elements[activeElement].Description);
        }
    }
    public ItemNeeded[] ReturnsItemsNeededElement()
    {
        if (itemSelected == true && data.elements[activeElement].NextRing != null)
        {
            return (childs[activeElement].data.elements[childs[activeElement].activeElement].ResourcesNeededForBuilding);
        }
        else
        {
            return (data.elements[activeElement].ResourcesNeededForBuilding);
        }
    }

    public GameObject ReturnsGameObjectElement()
    {
        if (itemSelected == true && data.elements[activeElement].NextRing != null)
        {
            return (childs[activeElement].data.elements[childs[activeElement].activeElement].PlaceHolderObjectCorrect);
        }
        else
        {
            return (data.elements[activeElement].PlaceHolderObjectCorrect);
        }
    }
    public GameObject ReturnsGameObjectIncorrectElement()
    {
        if (itemSelected == true && data.elements[activeElement].NextRing != null)
        {
            return (childs[activeElement].data.elements[childs[activeElement].activeElement].PlaceHolderObjectIncorrect);
        }
        else
        {
            return (data.elements[activeElement].PlaceHolderObjectIncorrect);
        }
    }

    private float NormalizeAngle(float a) => (a + 360f) % 360f; //if its bigger than 360 or less than 0, it delivers the correct angle value
}
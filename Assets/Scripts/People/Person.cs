using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

public enum TypePerson
{
    None,
    Slave,
    Plebeian,
    Freedman,
    Patrician
}
public class Person : MonoBehaviour
{
    public string Name;
    public string SurnameFamily;
    public string Description;
    public Sprite SpritePerson;
    public TypePerson personType = TypePerson.None;
    public bool isMale;
    public HouseBuilding roofNeeded;
    public bool hasEmploy;
    public JobBuilding job;
    public Person family;
    [SerializeField]
    Animator animator;
    //tasks
    //demands
    public Demand[] demands;
    [SerializeField] 
    People listPeople;
    [SerializeField]
    private TypePeople peopleLivingState;
    [SerializeField]
    List<Vector3> actualPathFollowing;
    [SerializeField]
    List<Vector3> reversePathFollowing;
    [SerializeField]
    public Vector3 target=new Vector3();
    public Vector3 actualTarget;
    public bool isWalking =false;
    public bool isEndPathReached = false;
    int counterPath = 0;
    [SerializeField]
    float movementSpeed = 1;
    bool isRotating=false;
    [SerializeField]
    float timeForRotating=2f;
    [SerializeField]
    float timePreviousToRotation = 2f;
    private void Awake()
    {
        animator=GetComponent<Animator>();
    }
    public void SetHouse(HouseBuilding building)
    {
        roofNeeded= building;
    }
    private void Update()
    {
        FindTarget();
        if (isWalking)
        {
            Move();

        }
    }
    void FindTarget()
    {

        if (isEndPathReached)
        {
            Debug.Log("sdf");
            if (target == job.buildingEnterPosition)
            {
                target = roofNeeded.buildingEnterPosition;
                List<Vector3> pathreverse = reversePathFollowing;
                reversePathFollowing = actualPathFollowing;
                actualPathFollowing = pathreverse;
                actualTarget = actualPathFollowing[0];
            }
            else
            {
                target = job.buildingEnterPosition;
                List<Vector3> pathActual = actualPathFollowing;
                actualPathFollowing = reversePathFollowing;
                reversePathFollowing = pathActual;
                actualTarget = actualPathFollowing[0];

            }

            isEndPathReached = false;
        }
        
    }
    void Move()
    {
        if (isWalking)
        {
            if (transform.position != target)
            {
                if (transform.position != actualTarget)
                {
                    Debug.Log("Is walking!");
                    float speedFinal = movementSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, actualTarget, speedFinal);
                }
                else
                {
                    if (counterPath==actualPathFollowing.Count)
                    {
                        Debug.Log("reached");
                        isEndPathReached = true;
                        counterPath = 0;
                    }
                    else
                    {
                        CheckIfItsCorner();
                        actualTarget = actualPathFollowing[counterPath++];

                    }
                }
            }
            else
            {
                isEndPathReached = true;
                counterPath = 0;

            }

        }
    }
    void CheckIfItsCorner()
    {
        if (!isRotating)
        {
            Debug.Log(counterPath);
            if (actualPathFollowing[counterPath+1] == new Vector3(actualTarget.x - 1, 0, actualTarget.z+1))
            {
                isRotating = true;
                Debug.Log("Actual path:" + actualPathFollowing[counterPath + 1] + " and acrtual taarget path:" + actualTarget);

                if (actualPathFollowing[counterPath].x > actualPathFollowing[counterPath + 1].x)
                {
                    //transform.Rotate(new Vector3(0, -90, 0));
                    Debug.Log("Rotating Left");
                    StartCoroutine(RotateForDuration(-90));
                }
                else 
                {
                    Debug.Log("Rotating Left 2");

                    StartCoroutine(RotateForDuration(90));


                }

            }
            else if (actualPathFollowing[counterPath+1] == new Vector3(actualTarget.x - 1, 0, actualTarget.z+1))
            {
                //transform.Rotate(new Vector3(0, -90, 0));
                Debug.Log("Rotating Right");
                StartCoroutine(RotateForDuration(90));

            }
            //if (actualPathFollowing[counterPath + 1] == new Vector3(actualTarget.x + 1, 0, actualTarget.z - 1))
            //{
            //    //transform.Rotate(new Vector3(0, -90, 0));
            //    Debug.Log("Rotating");
            //    StartCoroutine(RotateForDuration(90));

            //}
            //if (actualPathFollowing[counterPath + 1] == new Vector3(actualTarget.x - 1, 0, actualTarget.z + 1))
            //{
            //    //transform.Rotate(new Vector3(0, -90, 0));
            //    Debug.Log("Rotating");
            //    StartCoroutine(RotateForDuration(-90));

            //}
        }

    }
    IEnumerator RotateForDuration(float rotationAngle)
    {
        isRotating = true;
        float elapsedTime = 0.0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(Vector3.up * rotationAngle); // Gira 360 grados
        while (elapsedTime < timePreviousToRotation)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0.0f;
        while (elapsedTime < timeForRotating)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / timeForRotating);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // Asegura que termine en la rotación final exacta
        isRotating = false;
    }
    public void AddPersonToTheList()
    {
        People[] list = GameObject.FindObjectsOfType<People>();
        foreach (var item in list)
        {
            if (item.typeList == peopleLivingState)
            {
                listPeople = item;
            }
        }
        this.transform.parent = listPeople.GetComponentInParent<Transform>();
        listPeople.AddPeopleToList(this);
        listPeople.AddPeopleToListUnemployed(this);

    }
    public void SetEmploy(bool set,JobBuilding jobNew)
    {
        job = jobNew;
        hasEmploy = set;

        List<Vector3> listPath= GridSearchIA.AStarSearchNormalGridForPeople(job.grid, new Vector3Int((int)job.buildingEnterPosition.x, 0, (int)job.buildingEnterPosition.z),
            new Vector3Int((int)roofNeeded.buildingEnterPosition.x, 0, (int)roofNeeded.buildingEnterPosition.z));


        actualPathFollowing = listPath;

        reversePathFollowing = GridSearchIA.AStarSearchNormalGridForPeople(job.grid, new Vector3Int((int)roofNeeded.buildingEnterPosition.x, 0, (int)roofNeeded.buildingEnterPosition.z),
            new Vector3Int((int)job.buildingEnterPosition.x, 0, (int)job.buildingEnterPosition.z));
        //transform.position = actualPathFollowing[0];
        actualTarget = actualPathFollowing[0];
        target= job.buildingEnterPosition;
        counterPath = 0;
        SetOffsetStartLiving();
    }
    void SetOffsetStartLiving()
    {
        StartCoroutine(StartLiving());
    }
    IEnumerator StartLiving()
    {
        float elapsedTime = 0.0f;
        float randomTime = UnityEngine.Random.Range(0f, 2f);
        while (elapsedTime < randomTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isWalking = true;
        animator.SetTrigger("StartWalking");

    }
    public Vector3 ReturnPositionRoof()
    {
        return roofNeeded.transform.position;
    }
    public void SatisfyDemands(GameManager gameManager)
    {
        foreach (var demand in demands)
        {
            ItemNeeded[] itemsNeeded=new ItemNeeded[1];
            itemsNeeded[0] = new ItemNeeded();
            itemsNeeded[0].TypeItemNeeded = demand.TypeItemNeeded;
            itemsNeeded[0].amountItem = demand.AmountItemDemanded;
            if (gameManager.CheckIfCanBeSatisfiedDemand(itemsNeeded[0]))
            {
                if (demand.canBeSatisfied)
                {
                   
                    gameManager.ReduceStats(itemsNeeded);
                    demand.SatisfyDemand();
                }
                else
                {
                    itemsNeeded[0].amountItem = (int)(itemsNeeded[0].amountItem / 2);  //it only consumes half since it does not have any building supplying it
                    gameManager.ReduceStats(itemsNeeded);
                    demand.SatisfyDemandIfThePersonDoesntHaveABuildingNear();
                }

            }
            else
            {
                demand.UnSatisfyDemand();

            }

        }
    }
}

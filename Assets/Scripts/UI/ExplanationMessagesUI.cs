using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExplanationMessagesUI : MonoBehaviour
{
    Vector3 defaultPosition= Vector3.zero;
    bool isWorking = false;
    [SerializeField]
    TextMeshProUGUI textMesh;
     float heightItem = 2;
    [SerializeField]
    GameManager gameManager;
    private void Start()
    {
        defaultPosition=transform.position;
    }
    public void ReturnToDefaultPosition()
    {
        this.gameObject.transform.position=defaultPosition;
    }
    public void ChangePosition(Vector3 transformPosition,string description)
    {
        isWorking= true;
        transform.position=new Vector3(transformPosition.x, transformPosition.y+ heightItem, transformPosition.z);
        textMesh.text = description;
    }
    private void Update()
    {
        //var rotation = gameManager.cameraRenderer.transform.rotation;

        //transform.LookAt(gameManager.cameraRenderer.transform.position+ rotation * Vector3.forward, rotation * Vector3.up);
    }
}

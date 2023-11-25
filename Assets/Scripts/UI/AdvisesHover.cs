using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdvisesHover : MonoBehaviour
{
    [SerializeField]
    GameObject explanationMessagesGO;

    [SerializeField]
    string explanationAdviseMessage;
    ExplanationMessagesUI explanationMessagesUI;
    private void Start()
    {
        explanationMessagesUI=explanationMessagesGO.GetComponent<ExplanationMessagesUI>();
    }
    private void OnMouseOver()
    {
        explanationMessagesUI.ChangePosition(transform.position,explanationAdviseMessage);
    }
    private void OnMouseExit()
    {
        explanationMessagesUI.ReturnToDefaultPosition();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMissionsManager : MonoBehaviour
{
    [SerializeField]
    UIPause pauseUI;
    [SerializeField]
    GameObject infoMissionPanel;
    [SerializeField]
    SwitchInspectorAndLoadItems switchInspector;
    [SerializeField]
    GameObject closeIcon;
    [SerializeField]
    Animator animatorPrincipalMission;
    [SerializeField]
    Animator animatorCloseArrow;
    bool check = false;
    public void LoadInfoMissionPanel()
    {
        switchInspector.HideInspectors();
        if (!check)
        {
            //infoMissionPanel.SetActive(true);
            animatorPrincipalMission.SetTrigger("Appear");
            animatorCloseArrow.SetTrigger("Appear");
            pauseUI.PauseForMenuStats();
            //closeIcon.SetActive(true);
            check= true;

        }
        else
        {
            OnClose();
        }

    }
    public void OnClose()
    {
        animatorPrincipalMission.SetTrigger("Disappear");
        animatorCloseArrow.SetTrigger("Disappear");

       // closeIcon.SetActive(false);

       // infoMissionPanel.SetActive(false);
        pauseUI.StopPauseForMenuStats();
        check= false;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

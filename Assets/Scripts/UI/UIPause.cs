using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPause : MonoBehaviour
{
    Canvas canvasPause;
    [SerializeField]
    AudioManager audioManager;
    [SerializeField]
    CameraController cameraController;
    [SerializeField]
    float timeForResetPauseMenu;
    bool isPause = false;
    // Start is called before the first frame update
    void Start()
    {
        canvasPause = GetComponent<Canvas>();
        canvasPause.enabled = false;
    }
    public void PauseForMenuStats()
    {
        Time.timeScale = 0;
    }
    public void StopPauseForMenuStats()
    {
        Time.timeScale = 1;
    }
    public void PauseByClickingItem()
    {
        audioManager.PauseMusicBG();
        canvasPause.enabled = true;
        Time.timeScale = 0;
        isPause = true;
    }
    public void PauseGeneral()
    {
        Debug.Log("afjasjk");
        if (isPause)
        {
            StopPause();
            audioManager.ContinueMusicBG();

        }
        else
        {
            PauseByClickingItem();
            audioManager.PauseMusicBG();
        }
    }
    public void StopPause()
    {
        audioManager.ContinueMusicBG();

        StartCoroutine(DisableInput());
        cameraController.newPosition = Vector3.zero;
        canvasPause.enabled = false;

        Time.timeScale = 1;
    }
    IEnumerator DisableInput()
    {
        cameraController.DisableInputCam();
        yield return new WaitForSeconds(timeForResetPauseMenu);
        cameraController.EnableInputCam();
        isPause = false;

        StopCoroutine(DisableInput());

    }
}

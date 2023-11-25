using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Action<Vector3Int> OnMouseClick, OnMouseHold;
    public Action OnMouseUp;
    public WorldGenerator grid;
    public bool isHolding = false;
    public bool isClickedUp = false;
    public bool isClickedDown = false;

    public bool isClickedDownRight = false;
    public bool isClickedUpRight = false;
    public bool isPausedGame = false;

    public bool buildDescriptionMenuChecker=false;
    public Vector3 mouseposition;
    public GameManager gameManager;
    public UIManager uiManager;
    public KeyCode keyCodeEscape;
    [SerializeField]
    LayerMask layerTerrain;
    ControllerMode lastControllerMode= ControllerMode.None;

    // Start is called before the first frame update
    public Vector3Int GetPosition()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo,1000,layerTerrain))
        {
           Vector3Int actualPosition = grid.GetNearestPointOnGrid(hitInfo.point); //gets the actualPosition of the mouse in the grid with entire numbers as for example vector3(1,0,2)
           // Vector3Int actualPosition=new Vector3Int((int)hitInfo.point.x,0, (int)hitInfo.point.z);
            return actualPosition;
        }
        else
        {
            return new Vector3Int(999, 999, 999);
        }
    }

    public Vector3? GetPositionFloat()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            Vector3 actualPosition = hitInfo.point; //gets the actualPosition of the mouse in the grid with entire numbers as for example vector3(1,0,2)
            return actualPosition;
        }
        else
        {
            return null;
        }
    }

    public string GetPositionForCoordenates()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            string actualPosition = (hitInfo.textureCoord.x) + "," + (hitInfo.textureCoord.y); //gets the actualPosition of the mouse in the grid with entire numbers as for example vector3(1,0,2)
            return actualPosition;
        }
        else
        {
            return null;
        }
    }

    // Update is called once per frame
    private void LateUpdate()   //lateupdate for inputs of
    {
        if (!gameManager.cam.isRotating)
        {
            CheckClickDownEvent();
            CheckClickUpEvent();
            CheckClickHoldEvent();
            CheckRightBottonClickedEvent();
            CheckRightBottonUpEvent();
            CheckEscapePause();
        }
    }

    public void CheckEscapePause()
    {
        if (Input.GetKeyDown(keyCodeEscape))
        {
            if (lastControllerMode==ControllerMode.None)
            {
                lastControllerMode = gameManager.state;
                gameManager.ChangeState(ControllerMode.Menu);
                uiManager.ControlPauseMenuActive();
            }
            else
            {
                gameManager.ChangeState(lastControllerMode);
                uiManager.ControlPauseMenuDesactive();
                lastControllerMode = ControllerMode.None;


            }

        }
    }

    public void CheckClickHoldEvent()
    {
        if (Input.GetMouseButton(0) && GetPosition() != null)
        {
            if (isClickedDown == true)
            {
                isHolding = true;
                isClickedUp = false;
            }
        }
    }
    public void CheckRightBottonUpEvent()
    {
        if ((Input.GetMouseButtonUp(1) && GetPosition() != null))
        {
            isClickedUpRight= true;
            isClickedDownRight = false;

        }
    }
    public void CheckRightBottonClickedEvent()
    {
        if ((Input.GetMouseButtonDown(1) && GetPosition() != null))
        {
            gameManager.placeHolderMode = false;
            isClickedDownRight = true;
            isClickedUpRight = false;
            mouseposition = Input.mousePosition;

            //if (!isClickedUpRight )
            //{
            //    uiManager.EnableAndDisableMenuBuilder();
            //    Debug.Log("disabled2");
            //    buildDescriptionMenuChecker = false;

            //}

            if (gameManager.state == ControllerMode.Build)
            {
                gameManager.ChangeState(ControllerMode.Play);
                gameManager.FinishBuildMode();
                uiManager.mainMenuInstance.CleanInformationMenu();
                gameManager.EnableCamera();
                uiManager.DisableMenuBuilder();
                gameManager.audioManager.CloseInspector();


                isClickedDownRight = false;
            }
            else if (gameManager.state == ControllerMode.Play && isClickedDownRight)
            {
                gameManager.DisableCamera();
                uiManager.EnableMenuBuilder();
                gameManager.audioManager.OpenInspector();
                if (uiManager.mainMenuInstance != null)
                {
                    gameManager.ChangeState(ControllerMode.BuildMenu);
                    uiManager.mainMenuInstance.CleanInformationMenu();
                    //TODO, make that the first time that enters the buildmode and creates de ring, to clean the information also
                }
            }
        }
        else if (Input.GetMouseButtonUp(1) && GetPosition() != null)
        {
            isClickedDownRight = false;
            //if (!isClickedUpRight && !buildDescriptionMenuChecker&&!gameManager.placeHolderMode)
            //{
            //    uiManager.EnableAndDisableMenuBuilder();
            //    buildDescriptionMenuChecker = true;


            //}
            gameManager.EnableCamera();

            uiManager.DisableMenuBuilder();
            gameManager.audioManager.CloseInspector();


            if (gameManager.state == ControllerMode.BuildMenu)
            {
                if (uiManager.mainMenuInstance != null)
                {
                    RingMenu ring = uiManager.mainMenuInstance.childs.Find(j => j.numberIndexParent == uiManager.mainMenuInstance.activeElement);

                    if (uiManager.mainMenuInstance.gameObject.activeInHierarchy)
                    {
                        if (!uiManager.mainMenuInstance.itemSelected)
                        {
                            uiManager.mainMenuInstance.CleanInformationMenu();
                            gameManager.ChangeState(ControllerMode.Play);
                            gameManager.DisableCamera();
                        }
                    }
                    else if (ring)
                    {
                        if (!ring.itemSelected)
                        {
                            uiManager.mainMenuInstance.CleanInformationMenu();
                            gameManager.ChangeState(ControllerMode.Play);
                            gameManager.DisableCamera();
                        }
                    }
                }
            }
        }
    }

    public void CheckClickUpEvent()
    {
        if (Input.GetMouseButtonUp(0) && GetPosition() != null)
        {
            if (isHolding == true)
            {
                isHolding = false;
                isClickedDown = false;
                isClickedUp = true;
            }
        }
    }

    public void CheckClickDownEvent()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            isHolding = false;
            isClickedDown = true;
            isClickedUp = false;
            if (gameManager.state == ControllerMode.Build)
            {
               gameManager.placeHolderMode = false;
            }
        }
    }
}
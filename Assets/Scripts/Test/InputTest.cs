using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputTest : MonoBehaviour
{
    [SerializeField]
    GridIA grid;
    public Vector3 GetPosition()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            Vector3 actualPosition = grid.GetNearestPointOnGrid(hitInfo.point); //gets the actualPosition of the mouse in the grid with entire numbers as for example vector3(1,0,2)
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
            Vector3 actualPosition = grid.GetNearestPointOnGrid(hitInfo.point); //gets the actualPosition of the mouse in the grid with entire numbers as for example vector3(1,0,2)
            return actualPosition;
        }
        else
        {
            return null;
        }
    }
    private void Update()
    {
        CheckClickDownEvent();
    }
    public void CheckClickDownEvent()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Debug.Log(GetPosition());
        }
    }

}

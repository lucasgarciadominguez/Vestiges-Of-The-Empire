using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class CameraController : MonoBehaviour
{
    #region class variables

    public Transform actualCameraTransform;
    public float movementSpeed;
    public float movementTime;
    public float rotationSpeed;
    public float zoomSpeed;
    public float inputX;
    public float inputY;

    public Vector3 newPosition;
    public TMP_Text textX;
    public TMP_Text textY;
    public float newRotationY;
    public float newRotationX;
    public int verticalRotationMin;
    public int verticalRotationMax;
    public Vector3 newZoom;
    public LayerMask layerMask;
    public Vector2 screenLimits;
    public bool isRotating = false;

    public KeyCode keyCodeRotate;
    public KeyCode keyCodeUp;
    public KeyCode keyCodeDown;
    public KeyCode keyCodeLeft;
    public KeyCode keyCodeRight;
    bool canOperateInput = true;
    #endregion class variables

    private void Start()
    {
        newPosition = transform.position;
        newZoom = actualCameraTransform.localPosition;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        HandleMouseInput();
        HandleMovementInput();
        HandleMovementInputWASD();
    }
    public void EnableInputCam()
    {
        canOperateInput= true;
    }
    public void DisableInputCam()
    {
        canOperateInput = false;


    }
    public Ray PosicionPuntero()    //getmouseposition
    {
        return actualCameraTransform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
    }

    private void HandleMouseInput()
    {
        if (canOperateInput)
        {
            inputX = Input.mousePosition.x;
            inputY = Input.mousePosition.y;
        }
        else
        {
            newPosition = new Vector3(transform.position.x, -6f, transform.position.z);
            inputX = 0;
            inputY = 0;
        }

    }

    private void HandleMovementInputWASD()
    {
        if (Input.GetKey(keyCodeLeft))   //de izquierda a derecha, se modifica el vector de nueva posicion
        {
            newPosition += (transform.right * -movementSpeed);
        }
        if (Input.GetKey(keyCodeRight))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(keyCodeDown))  //de arriba a abajo, se modifica el vector de nueva posicion
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(keyCodeUp))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        newPosition.x = Mathf.Clamp(newPosition.x, 0, screenLimits.x);
        newPosition.z = Mathf.Clamp(newPosition.z, 15, screenLimits.y);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);  //se modifca el posicion del eje de la camara
    }

    private void HandleMovementInput()
    {
        Vector3 position = Input.mousePosition;
        textX.text = position.x.ToString();
        textY.text = position.y.ToString();
        if (Input.GetMouseButton(1) && Input.GetKey(keyCodeRotate))    //si se presiona alt+click derecho, se rota
        {
            HandleRotation();
            isRotating = true;
        }
        else
        {
            isRotating = false;
        }
        if (!isRotating)
        {
            if ((inputX < (Screen.width - 20) && inputY < (Screen.height - 20)) && inputY > 20 && inputX > 20) //comprueba que el cursor esta en las dimensiones preestablecidas, definiendo la zona en donde no se mueve la camara
            {
                #region zoom

                if (Input.GetAxis("Mouse ScrollWheel") != 0) // segun el giro de la rueda se amplia o se reduce, reduciendo o ampliando el fov
                {
                    Debug.Log("Zooomuig");
                    Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * 10, 10, 60);
                }

                #endregion zoom

                transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            }
            else
            {
                #region cameraMovement
                if (canOperateInput)
                {
                    //sino, la camara se mueve en ciertas direcciones
                    if (!isRotating)
                    {
                        if ((inputX < Screen.width / 2))    //de izquierda a derecha, se modifica el vector de nueva posicion
                        {
                            newPosition += (transform.right * -movementSpeed);
                        }
                        else
                        {
                            newPosition += (transform.right * movementSpeed);
                        }
                        if ((inputY < Screen.height / 2)) //de arriba a abajo, se modifica el vector de nueva posicion
                        {
                            newPosition += (transform.forward * -movementSpeed);
                        }
                        else
                        {
                            newPosition += (transform.forward * movementSpeed);
                        }
                        newPosition.x = Mathf.Clamp(newPosition.x, 0, screenLimits.x);
                        newPosition.z = Mathf.Clamp(newPosition.z, 15, screenLimits.y);
                    }

                    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);  //se modifca el posicion del eje de la camara
                }
                #endregion cameraMovement
            }
        }
    }

    private void HandleRotation()
    {
        #region horizontalRotation

        if (Input.mousePosition.x != inputX)    //de izquierda a derecha en eje horizontal, si el nuevo frame contiene una nueva posicicion en el eje horizontal
        {
            newRotationY = (Input.mousePosition.x - inputX) * rotationSpeed * Time.deltaTime;  //para rotar horizontal tenemos que variar el valor en y y multiplicarlo por un valor y el time.deltatime
            this.transform.Rotate(0, newRotationY, 0);
        }

        #endregion horizontalRotation

        #region verticalRotation

        if (Input.mousePosition.y != inputY)    //de izquierda a derecha en eje horizontal, si el nuevo frame contiene una nueva posicicion en el eje vertical
        {
            GameObject mainCamera = this.gameObject.transform.Find("Main Camera").gameObject;
            newRotationX = (Input.mousePosition.y - inputY) * rotationSpeed * Time.deltaTime;  //para rotar horizontal tenemos que variar el valor en y y multiplicarlo por un valor y el time.deltatime
            var desiredRotationX = mainCamera.transform.eulerAngles.x + newRotationX;
            if (desiredRotationX >= verticalRotationMin && desiredRotationX <= verticalRotationMax)
            {
                mainCamera.transform.Rotate(newRotationX, 0, 0);
            }
        }

        #endregion verticalRotation
    }
}
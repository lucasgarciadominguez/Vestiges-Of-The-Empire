using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    
    public GameObject RayCastGround(Ray ray,LayerMask layerRequired)
    {

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit,200, layerRequired))
        {
            return hit.transform.gameObject;
        }
        return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInteractables : MonoBehaviour
{
    public LayerMask layer;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(pos);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow, 1);
            
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000,  layer))
            {
                hit.transform.gameObject.GetComponent<WallMover>().SetWallFromRaycast(hit);
            }
        }
    }

    
}

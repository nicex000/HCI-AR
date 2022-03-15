using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMover : MonoBehaviour
{
    [SerializeField] private GameObject wall;


    public void SetWallFromRaycast(RaycastHit hit)
    {
        Vector3 newPos = transform.InverseTransformPoint(wall.transform.position);
        newPos.y = transform.InverseTransformPoint(hit.point).y;
        wall.transform.position = transform.TransformPoint(newPos);
    }
}

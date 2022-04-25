using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMover : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private bool affectedByGravity;
    [SerializeField] private float timeBeforeGravity;

    private Vector3 startPos;
    private Rigidbody wallRigidbody;
    private float gravityTimer;

    private void Start()
    {
        startPos = wall.transform.position;
        if (affectedByGravity)
            wallRigidbody = wall.GetComponent<Rigidbody>();
        gravityTimer = -1f;
    }

    private void Update()
    {
        if (affectedByGravity)
        {
            if (wallRigidbody != null && wallRigidbody.useGravity)
            {
                if (wall.transform.position.y <= startPos.y)
                {
                    ToggleGravity(false);
                }
            }

            if (wallRigidbody != null && !wallRigidbody.useGravity)
            {
                if (gravityTimer > -1)
                {
                    if (gravityTimer >= timeBeforeGravity)
                    {
                        ToggleGravity(true);
                        gravityTimer = -1f;
                    }
                    else
                    {
                        gravityTimer += Time.deltaTime;
                    }
                }
            }
        }
    }
    

    private void ToggleGravity(bool enable)
    {
        if (enable)
        {
            wallRigidbody.useGravity = true;
            wallRigidbody.constraints = RigidbodyConstraints.FreezeAll
                                        & ~RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            wallRigidbody.useGravity = false;
            wallRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }


    public void SetWallFromRaycast(RaycastHit hit)
    {
        Vector3 newPos = transform.InverseTransformPoint(wall.transform.position);
        newPos.y = transform.InverseTransformPoint(hit.point).y;
        wall.transform.position = transform.TransformPoint(newPos);
        if (affectedByGravity)
        {
            gravityTimer = 0f;
        }
    }

}

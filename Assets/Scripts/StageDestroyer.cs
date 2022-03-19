using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageDestroyer : MonoBehaviour
{
    [SerializeField] private float drag;
    private bool alreadyTriggered = false;
    public void EnableDestruction()
    {
        if (alreadyTriggered) return;
        alreadyTriggered = true;
        var objects = FindObjectsOfType<Collider>();
        float lowestY = float.MaxValue;
        foreach (var collider in objects)
        {
            lowestY = Mathf.Min(lowestY, collider.gameObject.transform.position.y);
            Rigidbody body = collider.attachedRigidbody;
            if (body == null)
            {
                body = collider.gameObject.AddComponent<Rigidbody>();
            }

            body.constraints = RigidbodyConstraints.None;
            body.useGravity = true;
            body.drag = drag;
            Vector3 x = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            x *= 100 * drag;
            body.AddTorque(x);
        }

        var killColl = gameObject.AddComponent<BoxCollider>();
        killColl.center = Vector3.up * lowestY;
        killColl.size = new Vector3(100f, 0.01f, 100f);

        var disableScripts = FindObjectsOfType<WallMover>();
        foreach (var script in disableScripts)
        {
            script.enabled = false;
        }

    }

    void OnCollisionEnter(Collision other)
    {
        Destroy(other.collider.gameObject);
    }
    void OnCollisionStay(Collision other)
    {
        Destroy(other.collider.gameObject);
    }
    void OnCollisionExit(Collision other)
    {
        Destroy(other.collider.gameObject);
    }
}

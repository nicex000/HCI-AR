using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMover : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private Vector3 finalPosOffset;

    private Vector3 initialPos;
    private float animationTimer;
    private float endTime;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        animationTimer = -1f;
        endTime = curve[curve.length - 1].time;
    }

    // Update is called once per frame
    private void Update()
    {
        if (animationTimer >= 0f)
        {
            animationTimer += Time.deltaTime;

            transform.position = Vector3.Lerp(initialPos, initialPos + finalPosOffset, curve.Evaluate(animationTimer));

            if (animationTimer > endTime)
            {
                animationTimer = -1f;
            }
        }
    }

    public void TriggerMove()
    {
        animationTimer = 0f;
        initialPos = transform.position;
    }
}

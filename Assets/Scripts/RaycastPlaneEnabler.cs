using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;

public class RaycastPlaneEnabler : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private PlaceOnPlane placeScript;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "Disable placement";
        
    }

    public void ToggleRaycast()
    {
        if (placeScript.enabled)
        {
            placeScript.enabled = false;
            text.text = "Enable placement";
        }
        else
        {
            placeScript.enabled = true;
            text.text = "Disable placement";
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedObject : MonoBehaviour
{

    //  For this class to work rendering mode of the     !!! 
    //  material of this gameObject must be set to fade   !!!

    public bool isVisible = true;

    //Set visibility change velocity (lesser amounts = slower transition / higher amounts = faster transition)
    private float time = 0.015f;
    private float alphaVal;
    private Renderer rend;
    private Color color;

    void Start()
    {
        rend = GetComponent<Renderer>();
        color = rend.material.color;
        alphaVal = color.a;
    }

    void Update()
    {
        if (!isVisible && alphaVal > 0)
        {
            alphaVal -= time;
            color.a = alphaVal;
            rend.material.color = color;

        }
        else if (isVisible && alphaVal < 1)
        {
            alphaVal += time;
            color.a = alphaVal;
            rend.material.color = color;

        }
        else if (alphaVal < 0)
        {
            alphaVal = 0;
            Destroy(gameObject);
        }
        else if (alphaVal > 1) alphaVal = 1;
    }

    public void ChangeObjectVisibility()
    {
        isVisible = !isVisible;
    }
    
    public void SetVisibility(bool state)
    {
        isVisible = state;
    }
}

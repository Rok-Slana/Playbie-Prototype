using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPanel : MonoBehaviour
{
    public bool panelState = true;

    //Set visibility change velocity (lesser amounts = slower transition / higher amounts = faster transition)
    private float time = 0f;
    private float timeMiltipier = 5.5f;
    [SerializeField]
    public float AlphaValPublic { get; private set; }

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
        if (!panelState && alphaVal > 0)
        {
            alphaVal = Mathf.Lerp(1, 0, time);
            time += timeMiltipier * Time.deltaTime;
            color.a = alphaVal;
            rend.material.color = color;
        }
        else if (panelState && alphaVal < 1)
        {
            alphaVal = Mathf.Lerp(0, 1, time);
            time += timeMiltipier * Time.deltaTime;
            color.a = alphaVal;
            rend.material.color = color;
        }
        else if (alphaVal == 0)
        {
            alphaVal = 0;
            AlphaValPublic = alphaVal;
            time = 0f;
        }
        else if (alphaVal == 1)
        {
            alphaVal = 1;
            time = 0f;
        }
}

        public void PanelSwitch(bool state)
    {
        panelState = state;
    }
}

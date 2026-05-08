using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomToggleScript : MonoBehaviour, IPointerClickHandler
{
    private Animator anim;
    private bool isOn = true;
    public Data data;
    public bool amIMethod;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    public void Toggle()
    {
        Debug.Log("toggle activated! before if statements, isOn is " + isOn);
        if (isOn)
        {
            anim.Play("toggleOff", 0, 0f);
            if (amIMethod)
            {
                data.method = PaystubGrabbingMethod.DatabaseFile;
                data.methodText.text = "database file";
            } else
            {
                data.sceneToGoTo = WhatSceneToGoTo.TestPaystubViewer;
                data.sceneText.text = "Test Paystub Viewer";
            }
            isOn = false;
        }
        else
        {
            anim.Play("toggleOn", 0, 0f);
            if (amIMethod)
            {
                data.method = PaystubGrabbingMethod.JSON;
                data.methodText.text = "JSON";
            } else
            {
                data.sceneToGoTo = WhatSceneToGoTo.PaystubBrowser;
                data.sceneText.text = "Paystub Browser";
            }
            isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

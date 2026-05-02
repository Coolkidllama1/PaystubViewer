using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum PaystubGrabbingMethod
{
    DatabaseFile,
    JSON
}

public enum WhatSceneToGoTo
{
    TestPaystubViewer,
    PaystubBrowser
}

public class Data : MonoBehaviour
{
    public Slider textSize; //6-140, default 36
    public TMP_InputField ipInputField;
    public TMP_InputField portInputField;
    public PaystubGrabbingMethod method;
    public WhatSceneToGoTo sceneToGoTo;
    public Button goBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

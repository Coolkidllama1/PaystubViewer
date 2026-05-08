using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public TMP_Text textSizeTxt;
    public TMP_InputField ipInputField;
    public TMP_InputField portInputField;
    public PaystubGrabbingMethod method;
    public TMP_Text methodText;
    public WhatSceneToGoTo sceneToGoTo;
    public TMP_Text sceneText;
    public Button goBtn;
    public SOPaystub soPaystub;
    public GlobalSettings globalSettings;
    [Space]
    public List<TMP_Text> textsToChangeSizeOf;
    private Coroutine checkCoroutine;


    private IEnumerator CheckServer()
    {
        goBtn.interactable = false;
        string ip = ipInputField.text;
        string port = portInputField.text;
        if (string.IsNullOrEmpty(ip))
        {
            ip = "10.0.0.55";
        }
        if (string.IsNullOrEmpty(port))
        {
            port = "3000";
        }
        yield return new WaitForSeconds(0.5f);

        string URL = $"https://{ip}:{port}/paystub";
        using var request = UnityWebRequest.Get(URL);
        request.certificateHandler = new BypassCertificate();
        request.timeout = 5;

        yield return request.SendWebRequest();

        bool reachable = request.result != UnityWebRequest.Result.ConnectionError && request.result != UnityWebRequest.Result.DataProcessingError;
        goBtn.interactable = reachable;
    }

    private void RestartCheck()
    {
        if (checkCoroutine != null) StopCoroutine(checkCoroutine);
        checkCoroutine = StartCoroutine(CheckServer());
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ipInputField.onValueChanged.AddListener(_ => RestartCheck());
        portInputField.onValueChanged.AddListener(_ => RestartCheck());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoClick()
    {
        if (ipInputField.text != "" && portInputField.text != "")
        {
            globalSettings.serverIP = ipInputField.text;
            globalSettings.serverPort = portInputField.text;
        }
        else
        {
            globalSettings.serverIP = "10.0.0.55";
            globalSettings.serverPort = "3000";
        }
        //todo: pull all paystubs and save them to the soPaystub scriptable object, then load the scene
        string URL = $"https://{globalSettings.serverIP}:{globalSettings.serverPort}/paystub";
        if (method == PaystubGrabbingMethod.DatabaseFile)
        {
            URL += "/db";
        }
        else if (method == PaystubGrabbingMethod.JSON)
        {
            URL += "/json";
        }
        globalSettings.method = method;

        StartCoroutine(FetchAndGo(URL));
    }

    private IEnumerator FetchAndGo(string url)
    {
        using var request = UnityWebRequest.Get(url);
        request.certificateHandler = new BypassCertificate();
        request.timeout = 5;
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string wrapped = "{\"paystubs\":" + request.downloadHandler.text + "}";
            var result = JsonUtility.FromJson<PaystubList>(wrapped);
            soPaystub.paystubs.Clear();
            soPaystub.paystubs.AddRange(result.paystubs);
        } else
        {
            Debug.LogError($"Failed to fetch paystubs: {request.error}");
            yield break;
        }

        if (sceneToGoTo == WhatSceneToGoTo.TestPaystubViewer)
            UnityEngine.SceneManagement.SceneManager.LoadScene("Paystub Test View");
        else if (sceneToGoTo == WhatSceneToGoTo.PaystubBrowser)
            UnityEngine.SceneManagement.SceneManager.LoadScene("Paystub Browser");

    }

    public void ChangeTextSizes()
    {
        globalSettings.textSize = (int)textSize.value;
        if (textsToChangeSizeOf != null)
        {
            foreach (TMP_Text text in textsToChangeSizeOf)
            {
                text.fontSize = textSize.value;
            }
        }
        textSizeTxt.text = $"Text Size: {(int)textSize.value}";
    }

    
}

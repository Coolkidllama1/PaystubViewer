using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldDuo
{
    public TMP_InputField to;
    public TMP_InputField from;
    public InputFieldDuo(TMP_InputField to, TMP_InputField from)
    {
        this.to = to;
        this.from = from;
    }
}


public class Filters : MonoBehaviour
{
    public GlobalSettings settings;
    public SOPaystub soPaystub;
    public List<TMP_Text> txtxToChangeSizeTo = new List<TMP_Text>();
    public List<GameObject> LoadedPaystubNodes = new List<GameObject>();
    private List<GameObject> tmp = new List<GameObject>();   
    private List<GameObject> denied = new List<GameObject>();   
    public InputFieldDuo payDate;
    public InputFieldDuo payPeriodRange;
    public InputFieldDuo payRange;
    public TMP_InputField wage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (TMP_Text t in txtxToChangeSizeTo)
        {
            t.fontSize = settings.textSize;
        }
    }

    public bool FilterCheck(Paystub ps)
    {
        
    }

    public void AddNode(GameObject go, Paystub ps) // only the node spawner should be using this
    {
        if (ps == null)
        {
            denied.Add(go);
            go.SetActive(false);
            return;
        } 
        if (!FilterCheck(ps))
        {
            denied.Add(go);
            go.SetActive(false);
            return;
        }
        LoadedPaystubNodes.Add(go);
    }

    public void ReImport() //meant for the apply filters btn
    {
        tmp.AddRange(LoadedPaystubNodes);
        foreach (GameObject go in denied)
        {
            go.SetActive(true);
        }
        denied.Clear();
        LoadedPaystubNodes.Clear();
        foreach (GameObject go in tmp)
        {
            PaystubBrowserNode nodeScript = go.GetComponent<PaystubBrowserNode>();
            if (nodeScript != null)
            {
                AddNode(go);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

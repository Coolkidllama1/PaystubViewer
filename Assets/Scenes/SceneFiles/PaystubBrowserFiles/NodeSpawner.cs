using UnityEngine;

public class NodeSpawner : MonoBehaviour
{
    public GlobalSettings settings;
    public SOPaystub soPaystub;
    public GameObject nodePrefab;
    public GameObject contentObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //the filter class will do the actual spawning.
        foreach (Paystub p in soPaystub.paystubs)
        {
            GameObject node = Instantiate(nodePrefab, contentObject.transform);
            PaystubBrowserNode nodeScript = node.GetComponent<PaystubBrowserNode>();
            string hours = (p.earningsDetail != null && p.earningsDetail.Count > 0) ? p.earningsDetail[0].hours : "N/A";
            nodeScript.ParseData(p.adviceNumber, hours, p.payDate, p.netPayCurrent, settings, soPaystub, p);
            GetComponent<Filters>().AddNode(node, p);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PaystubBrowserNode : MonoBehaviour
{                                 //this is how it comes in
    public TMP_Text adviceNumber; //6535994
    public TMP_Text hoursWorked;  //64.73
    public TMP_Text payDate;      //2026-04-15 00:00:00
    public TMP_Text payRecieved;  //$856.91
    public Button viewDetails;
    public Button copyAdviceNumber;
    public GlobalSettings settings;
    public SOPaystub soPaystub;
    public Paystub fullPaystub;

    public void GoToFullView()
    {
        if (soPaystub == null || settings == null)
        {
            Debug.LogError("PaystubBrowserNode: Missing data for full view. ParseData must be ran first");
            return;
        }
        StartCoroutine(FetchDetailAndGo());
        
    }

    private string GetURLMethod()
    {
        if (settings.method == PaystubGrabbingMethod.DatabaseFile)
        {
            return "/db";
        }
        else
        {
            return "/json";
        }
    }

    private IEnumerator FetchDetailAndGo()
    {
        var paystub = soPaystub.paystubs.Find(p => p.adviceNumber == adviceNumber.text);
        if (paystub.lineItems == null || paystub.lineItems.Count == 0)
        {
            string url = $"https://{settings.serverIP}:{settings.serverPort}/paystub{GetURLMethod()}/{adviceNumber.text}";
            using var request = UnityWebRequest.Get(url);
            request.certificateHandler = new BypassCertificate();
            request.timeout = 5;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var detailed = JsonUtility.FromJson<Paystub>(request.downloadHandler.text);
                detailed.earningsDetail = new List<EarningsDetail>();
                detailed.deductionsDetail = new List<DeductionsDetail>();
                detailed.distributions = new List<Distributions>();

                foreach (var item in detailed.lineItems)
                {
                    if (item.category == "earning")
                    {
                        detailed.earningsDetail.Add(new EarningsDetail
                        {
                            description = item.description,
                            rate = item.rate,
                            hours = item.hours,
                            current = item.current,
                            ytd = item.ytd
                        });
                    } else if (item.category == "deduction")
                    {
                        detailed.deductionsDetail.Add(new DeductionsDetail
                        {
                            description = item.description,
                            current = item.current,
                            ytd = item.ytd
                        });
                    }
                    else if (item.category == "distribution")
                    {
                        detailed.distributions.Add(new Distributions
                        {
                            description = item.description,
                            current = item.current
                        });
                    }
                }
                soPaystub.tmp = detailed;
            } else
            {
                Debug.LogError($"Failed to fetch paystub details: {request.error}");
                yield break;
            }
        }
        else
        {
            soPaystub.tmp = paystub;
        }
        SceneManager.LoadScene("Paystub Test View");
    }



    public void ParseData(string adviceNumber, string hoursWorked, string payDate, string payRecieved, GlobalSettings settings, SOPaystub sopaystub, Paystub paystub)
    {
        this.adviceNumber.text = adviceNumber;
        this.hoursWorked.text = hoursWorked;
        this.payDate.text = payDate;
        this.payRecieved.text = payRecieved;
        this.settings = settings;
        this.soPaystub = sopaystub;
        this.fullPaystub = paystub;
    }

    public void CopyAdviceNumber()
    {
        GUIUtility.systemCopyBuffer = adviceNumber.text;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

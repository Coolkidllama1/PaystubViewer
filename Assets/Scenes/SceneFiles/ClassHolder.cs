using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

/// <summary>
/// from ClassHolder. just the wrapper for grabbing from the server.
/// </summary>
[System.Serializable]
public class PaystubList
{
    public List<Paystub> paystubs;
}

public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData) => true;
}
[System.Serializable]
public class LineItem
{
    public int id = -1;
    public int paystubId = -1;
    public string category;
    public string description;
    public string rate;
    public string hours;
    public string current;
    public string ytd;
    
     public bool IsEmpty
     {
         get
         {
            return string.IsNullOrEmpty(category) && string.IsNullOrEmpty(description) && string.IsNullOrEmpty(rate) && string.IsNullOrEmpty(hours) && string.IsNullOrEmpty(current) && string.IsNullOrEmpty(ytd) && id == -1 && paystubId == -1;
         }
     }
}

[System.Serializable]
public class Distributions
{
    public string description;
    public string current;
    public bool IsEmpty
     {
         get
         {
             return string.IsNullOrEmpty(description) && string.IsNullOrEmpty(current);
         }
    }
}
[System.Serializable]
public class DeductionsDetail
{
    public string description;
    public string current;
    public string ytd;
    public bool IsEmpty
    {
        get
        {
            return string.IsNullOrEmpty(description) && string.IsNullOrEmpty(current) && string.IsNullOrEmpty(ytd);
        }
    }
}
[System.Serializable]
public class EarningsDetail //on a paystub JSON i see Reg Hours-1112, Paid Tips-1112, Paid Tips YTD, and Reg Hours YTD
{
    public string description;
    public string rate;
    public string hours;
    public string current;
    public string ytd;

    public bool IsEmpty
    {
        get
        {
            return string.IsNullOrEmpty(description) && string.IsNullOrEmpty(rate) && string.IsNullOrEmpty(hours) && string.IsNullOrEmpty(current) && string.IsNullOrEmpty(ytd);
        }
    }
}

[System.Serializable]
public class Paystub
{
    public string adviceNumber;
    public int id;
    public string payDate;
    public string payPeriodStart;
    public string payPeriodEnd;
    public string grossPayCurrent;
    public string grossPayYTD;
    public string netPayCurrent;
    public string netPayYTD;
    public string taxDeductionsCurrent;
    public string taxDeductionsYTD;
    public string otherDeductionsCurrent;
    public string otherDeductionsYTD;
    public List<EarningsDetail> earningsDetail;
    public List<DeductionsDetail> deductionsDetail;
    public List<Distributions> distributions;

    public List<LineItem> lineItems;

    public bool IsEmpty
    {
        get
        {
            return string.IsNullOrEmpty(adviceNumber) && string.IsNullOrEmpty(payDate) && string.IsNullOrEmpty(payPeriodStart) && string.IsNullOrEmpty(payPeriodEnd) &&
                string.IsNullOrEmpty(grossPayCurrent) && string.IsNullOrEmpty(grossPayYTD) && string.IsNullOrEmpty(netPayCurrent) && string.IsNullOrEmpty(netPayYTD) &&
                string.IsNullOrEmpty(taxDeductionsCurrent) && string.IsNullOrEmpty(taxDeductionsYTD) && string.IsNullOrEmpty(otherDeductionsCurrent) && string.IsNullOrEmpty(otherDeductionsYTD) &&
                (earningsDetail == null || earningsDetail.Count == 0) && (deductionsDetail == null || deductionsDetail.Count == 0) && (distributions == null || distributions.Count == 0) && (lineItems == null || lineItems.Count == 0);
        }
    }
}

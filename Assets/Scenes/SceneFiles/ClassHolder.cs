using UnityEngine;
using System.Collections.Generic;

public class Distributions
{
    string description;
    string current;
}
[System.Serializable]
public class DeductionsDetail
{
    string description;
    string current;
    string ytd;
}
[System.Serializable]
public class EarningsDetail //on a paystub JSON i see Reg Hours-1112, Paid Tips-1112, Paid Tips YTD, and Reg Hours YTD
{
    string description;
    string rate;
    string hours;
    string current;
    string ytd;
}

[System.Serializable]
public class Paystub
{
    string adviceNumber;
    string payDate;
    string payPeriodStart;
    string payPeriodEnd;
    string grossPayCurrent;
    string grossPayYTD;
    string netPayCurrent;
    string netPayYTD;
    string taxDeductionsCurrent;
    string taxDeductionsYTD;
    string otherDeductionsCurrent;
    string otherDeductionsYTD;
    List<EarningsDetail> earningsDetail;
    List<DeductionsDetail> deductionsDetail;
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaystubTestViewScript : MonoBehaviour
{
    /*
     * heres the game plan for this script:
     * main data txt:
     * advice number: 6535994
pay date: 4/15/2026
pay period: 03/25/2026 - 04/07/2026
gross pay: $856.91
net pay: $767.59
gross pay YTD: $5,878.70
net pay YTD: $5,310.23
Tax Deductions: $89.32
Tax Deductions YTD: $568.47
other decuctions: $0.00
other deductions YTD: $0.00
     * 
     * earnings txt:
     * wage: $12.5
hours worked: 64.73
money earned from that: $809.13
digital tips recieved: $47.78
cash tips declared: $0.00
     * 
     * deductions txt:
     * deductions:
OASDI/EE: $50.17
MED/EE: $11.73
OASDItipEE: $2.96
Federal Witholding: $23.77
Med/EE/tip: $0.69
GA Witholding: $0.00
     * 
     * extra data txt:
     * heres the math:

856.91 - 767.59 = 89.32 / 856.91 = 0.104234984 
                                                           (about 10%)
use the exact number you got for more accuracy though. ill represent that big one with "ans)

856.91 - (856.91 * ans) = 767.59
856.91 - (856.91 * 0.1) = 771.219
     * 
     * 
     */
    public Paystub paystub;
    public TMP_Text mainData;
    public TMP_Text earnings;
    public TMP_Text deductions;
    public TMP_Text extraData;
    public TextAsset paystubJSON;
    public SOPaystub soPaystub;
    public GlobalSettings globalSettings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (soPaystub.paystubs.Count != 0)
        {
            if (soPaystub.tmp.IsEmpty)
            {
                paystub = soPaystub.paystubs[Random.Range(0, soPaystub.paystubs.Count)];
            } else
            {
                paystub = soPaystub.tmp;
            }
        }
        if (paystub.IsEmpty)
        {
            Debug.LogWarning("Paystub is empty and the paystub scriptable object has nothing. applying sample data via the json file in the unity project");
            paystub = JsonUtility.FromJson<Paystub>(paystubJSON.text);
        }
        SetDataToTexts(globalSettings.textSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float ParseCurrency(string value)
    {
        return float.Parse(value.Replace("$", "").Replace(",", ""));
    }

    private string FindEarning(string keyword)
    {
        var match = paystub.earningsDetail.Find(e => e.description.Contains(keyword));
        return match != null ? match.current : "$0.00";
    }

    private string FindDeduction(string keyword)
    {
        var match = paystub.deductionsDetail.Find(d => d.description.Contains(keyword));
        return match != null ? match.current : "$0.00";
    }

    public void SetDataToTexts(int textSize = 36)
    {
        SetMainDataText(textSize);
        SetEarningsText(textSize);
        SetDeductionsText(textSize);
        SetExtraDataText(textSize);
    }

    public void SetMainDataText(int textSize = 36)
    {
        mainData.text = $"Advice Number: {paystub.adviceNumber}\n" +
            $"Pay Date: {paystub.payDate}\n" +
            $"Pay Period: {paystub.payPeriodStart} - {paystub.payPeriodEnd}\n" +
            $"Gross Pay: {paystub.grossPayCurrent}\n" +
            $"Net Pay: {paystub.netPayCurrent}\n" +
            $"Gross Pay YTD: {paystub.grossPayYTD}\n" +
            $"Net Pay YTD: {paystub.netPayYTD}\n" +
            $"Tax Deductions: {paystub.taxDeductionsCurrent}\n" +
            $"Tax Deductions YTD: {paystub.taxDeductionsYTD}\n" +
            $"Other Deductions: {paystub.otherDeductionsCurrent}\n" +
            $"Other Deductions YTD: {paystub.otherDeductionsYTD}";
        mainData.fontSize = textSize;
    }

    public void SetEarningsText(int textSize = 36)
    {
        var regHours = paystub.earningsDetail.Find(e => e.description.Contains("Reg Hours") && !e.description.Contains("YTD"));
        string wage = regHours != null ? regHours.rate : "$0.00";
        string hours = regHours != null ? regHours.hours : "0";
        string moneyEarned = regHours != null ? regHours.current : "$0.00";
        earnings.text = $"Wage: {wage}\n" +
            $"Hours Worked: {hours}\n" +
            $"Money Earned from that: {moneyEarned}\n" +
            $"Digital Tips Received: {FindEarning("Paid Tips-")}\n" +
            $"Cash Tips Declared: {FindEarning("Cash Tips")}";
        earnings.fontSize = textSize;
    }

    public void SetDeductionsText(int textSize = 36)
    {
        deductions.text = $"Deductions:\nOASDI/EE: {FindDeduction("OASDI/EE")}\n" +
            $"MED/EE: {FindDeduction("MED/EE")}\n" +
            $"OASDItipEE: {FindDeduction("OASDItipEE")}\n" +
            $"Federal Withholding: {FindDeduction("Fed Withhold")}\n" +
            $"Med/EE/tip: {FindDeduction("Med/EE/tip")}\n" +
            $"GA Withholding: {FindDeduction("GA Withhold")}";
        deductions.fontSize = textSize;
    }

    public void SetExtraDataText(int textSize = 36)
    {
        float grossPay = ParseCurrency(paystub.grossPayCurrent);
        float netPay = ParseCurrency(paystub.netPayCurrent);
        float taxDeductions = ParseCurrency(paystub.taxDeductionsCurrent);
        float ans = taxDeductions / grossPay;
        float ansPercent = Mathf.Round(ans * 1000f) / 10f;
        float estimatedNet = grossPay - (grossPay * 0.1f);

        extraData.text = $"Here's the math:\n\n" +
            $"{grossPay} - {netPay} = {taxDeductions} / {grossPay} = {ans:F9}\n" +
            $"                                                           (about {ansPercent}%)\n" +
            $"Use the exact number you got for more accuracy though. I'll represent that big one with \"ans\"\n\n" +
            $"{grossPay} - ({grossPay} * ans) = {netPay}\n" +
            $"{grossPay} - ({grossPay} * 0.1) = {estimatedNet}";
        extraData.fontSize = textSize;
    }
    public void goToBrowser()
    {
        SceneManager.LoadScene("Paystub Browser");
    }
}

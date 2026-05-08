using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "Scriptable Objects/GlobalSettings")]
public class GlobalSettings : ScriptableObject
{
    public string serverIP;
    public string serverPort;
    public int textSize = 36;
    public PaystubGrabbingMethod method;
}

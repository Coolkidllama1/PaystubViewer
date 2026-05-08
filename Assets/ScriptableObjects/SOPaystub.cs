using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SOPaystub", menuName = "Scriptable Objects/SOPaystub")]
public class SOPaystub : ScriptableObject
{
    public List<Paystub> paystubs = new List<Paystub>();
    public Paystub tmp = new Paystub();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPartial : MonoBehaviour
{
    public Unit _Unit;
    protected Unit unit => _Unit ?? (_Unit = GetComponent<UnitPartialManager>().unit);


}

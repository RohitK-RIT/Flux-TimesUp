using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilityMeterHandler : MonoBehaviour
{
    public static StabilityMeterHandler Instance;
    public int stabilityLevel = 100;

    void Awake() { Instance = this; }

    public void DecreaseTSM(int amount) {
        //stabilityLevel -= amount;
        Debug.Log("boss is affecting TSM");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainUI : MonoBehaviour
{
    public const float MAX_BATT_POWER = 100f;
    public Slider batteryPowerBar;
    public PlayerSurvival playerSurvival;

    private void Start() {
        batteryPowerBar.value = 0;
    }

    // Update is called once per frame
    void Update() {
        batteryPowerBar.value = playerSurvival.GetCurrentBatteryPower / MAX_BATT_POWER;
    }
}

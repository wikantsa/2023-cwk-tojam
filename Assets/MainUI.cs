using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MainUI : MonoBehaviour
{
    public Slider batteryPowerBar;
    public PlayerSurvival playerSurvival;
    
    public RectTransform itemOne;
    public Image lemonCross;

    public RectTransform itemTwo;
    public Image missileCross;
    
    public RectTransform itemThree;
    public Image laserCross;
    
    public const float MAX_BATT_POWER = 100f;

    public TextMeshProUGUI _timerText;
    public int secondsElapsed { get; set; } = 0;
   

    private void Start() {
        batteryPowerBar.value = 0;
        StartCoroutine("GameTimeTracker");
    }

    // Update is called once per frame
    void Update() {
        batteryPowerBar.value = playerSurvival.GetCurrentBatteryPower / MAX_BATT_POWER;

        Debug.Log($"Bullets: {playerSurvival.GetItemPowerLevel(Power.Bullets)}");

        if (playerSurvival.GetItemPowerLevel(Power.Bullets) == 0)
            lemonCross.enabled = true;

        Debug.Log($"Missiles: {playerSurvival.GetItemPowerLevel(Power.Missiles)}");

        if (playerSurvival.GetItemPowerLevel(Power.Missiles) == 0)
            missileCross.enabled = true;

        Debug.Log($"Lasers: {playerSurvival.GetItemPowerLevel(Power.Laser)}");

        if (playerSurvival.GetItemPowerLevel(Power.Laser) == 0)
            laserCross.enabled = true;
    }

    private IEnumerator GameTimeTracker()
    {
        ++secondsElapsed;
        _timerText.text = $"{secondsElapsed} s";
        yield return new WaitForSeconds(1f);
    }


}

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
    public TextMeshProUGUI lemonRemains;

    public RectTransform itemTwo;
    public Image missileCross;
    public TextMeshProUGUI missileRemains;
    
    public RectTransform itemThree;
    public Image laserCross;
    public TextMeshProUGUI laserRemains;

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

        lemonRemains.text = $"{playerSurvival.GetItemPowerLevel(Power.Bullets)}";

        if (playerSurvival.GetItemPowerLevel(Power.Bullets) == 0)
            lemonCross.enabled = true;
        lemonRemains.text = $"{playerSurvival.GetItemPowerLevel(Power.Bullets)}";

        if (playerSurvival.GetItemPowerLevel(Power.Missiles) == 0)
            missileCross.enabled = true;
        missileRemains.text = $"{playerSurvival.GetItemPowerLevel(Power.Missiles)}";

        if (playerSurvival.GetItemPowerLevel(Power.Laser) == 0)
            laserCross.enabled = true;
        laserRemains.text = $"{playerSurvival.GetItemPowerLevel(Power.Laser)}";

    }

    private IEnumerator GameTimeTracker()
    {
        while (true)
        {
            ++secondsElapsed;
            _timerText.text = $"{secondsElapsed} s";
            yield return new WaitForSeconds(1);
        }
    }


}

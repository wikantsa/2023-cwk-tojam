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

    public RectTransform currentlySelectedWeapon;

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

    public List<CanvasGroup> canvasGroups;
    public CanvasGroup fadeCG;
    public CanvasGroup gameOverCG;
    public TextMeshProUGUI scoreText;

    private Coroutine timerRoutine;

    public static MainUI Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start() {
        batteryPowerBar.value = 0;
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

        // set selected indicator
        switch (playerSurvival.CurrentlySelectedWeapon)
        {
            case 0:
                currentlySelectedWeapon.position = new Vector3(
                    itemOne.position.x - 20,
                    itemOne.position.y,
                    itemOne.position.z
                );
                break;
            case 1:
                currentlySelectedWeapon.position = new Vector3(
                    itemTwo.position.x - 20,
                    itemTwo.position.y,
                    itemTwo.position.z
                );
                break;
            case 2:
                currentlySelectedWeapon.position = new Vector3(
                    itemThree.position.x - 20,
                    itemThree.position.y,
                    itemThree.position.z
                );
                break;
        }
    }

    public void StartTimer()
    {
        if (timerRoutine == null)
            timerRoutine = StartCoroutine(GameTimeTracker());
    }

    public void StopTimer()
    {
        StopCoroutine(timerRoutine);
    }

    private IEnumerator GameTimeTracker()
    {
        while (true)
        {
            ++secondsElapsed;
            _timerText.text = $"{secondsElapsed}s";
            yield return new WaitForSeconds(1);
        }
    }

    public void FadeCanvas(float value, float time)
    {
        foreach (var canvas in canvasGroups)
        {
            DOTween.To(() => canvas.alpha, x => canvas.alpha = x, value, time);
        }
    }

    public void UpdateFadeCG(float value, float time)
    {
        DOTween.To(() => fadeCG.alpha, x => fadeCG.alpha = x, value, time);
    }

    public void GameOverSequence()
    {
        scoreText.text = _timerText.text;
        DOTween.To(() => gameOverCG.alpha, x => gameOverCG.alpha = x, 1f, 1f);
    }
}

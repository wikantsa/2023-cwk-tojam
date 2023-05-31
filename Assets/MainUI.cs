using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Analytics;
using System.Runtime.InteropServices.WindowsRuntime;

public class MainUI : MonoBehaviour
{
    public Slider batteryPowerBar;
    public PlayerSurvival playerSurvival;

    public RectTransform currentlySelectedWeapon;
    public Image catFaceStatus;

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

    public TextMeshProUGUI timeMinutesValue;
    public TextMeshProUGUI timeMillisecondsValue;
    public int secondsElapsed { get; set; } = 0;

    public float reactionTime = 0.375f;

    public List<CanvasGroup> canvasGroups;
    public CanvasGroup fadeCG;
    public CanvasGroup gameOverCG;
    public TextMeshProUGUI scoreText;

    private Coroutine timerRoutine;
    private Coroutine milliShaker;

    public enum FaceNames
    {
        Healthy = 0,
        GotHit = 1,
        GettingLow = 2,
        NearingDeath = 3,
        GotHealed = 4
    };

    //Since dictionaries don't serialise natively, going to use
    // list-indices with text here instead
    // 0: base status
    // 1: on-hit -> use hurt face
    // 2: when health/energy is low, use tired face
    // 3: when very low, use terrifed face
    // 4: when recovering, use happy face
    public List<Sprite> felineFeelingFaces;

    public static MainUI Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start() {
        batteryPowerBar.value = 0;
        catFaceStatus.sprite = felineFeelingFaces[(int)FaceNames.Healthy];

    }
    private float Timer;

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
                currentlySelectedWeapon.position = itemOne.position; 
                break;
            case 1:
                currentlySelectedWeapon.position = itemTwo.position;
                break;
            case 2:
                currentlySelectedWeapon.position = itemThree.position;
                break;
        }
    }

    public void StartTimer()
    {
        if(milliShaker == null)
            milliShaker = StartCoroutine(ShakeAllTheMillis());

        if (timerRoutine == null)
            timerRoutine = StartCoroutine(GameTimeTracker());
    }

    public void StopTimer() {
        StopCoroutine(timerRoutine);
        StopCoroutine(milliShaker);
        StopAllCoroutines();
    }

    private IEnumerator ShakeAllTheMillis()
    {
        while (true)
        {
            timeMillisecondsValue.transform.DOPunchScale(new Vector3(0.8f, 0.6f, 0.6f), 0.3f, 3, 1);
            timeMillisecondsValue.transform.DOShakeRotation(1f, 70, 9, 0, true);
            yield return new WaitForSeconds(1f);
        }
    }

    public void UpdateFaceState(string reactionType = null) {
        if (reactionType == "heal")
            StartCoroutine(HitReaction());
        else if (reactionType == "damage")
            StartCoroutine(HealReaction());
        else if (reactionType == "constitution")
            CattyStatusIndicator();
    }

    private IEnumerator HitReaction()
    {
        catFaceStatus.sprite = felineFeelingFaces[(int)FaceNames.GotHit];
        catFaceStatus.transform.DOPunchScale(new Vector3(0.8f, 0.6f, 0.6f), 0.3f, 3, 1);
        catFaceStatus.transform.DOShakeRotation(1f, 70, 9, 0, true);
        yield return new WaitForSeconds(reactionTime);
        yield return new WaitUntil(CattyStatusIndicator);
    }

    private IEnumerator HealReaction()
    {
        catFaceStatus.sprite = felineFeelingFaces[(int)FaceNames.GotHealed];
        catFaceStatus.transform.DOPunchPosition(new Vector3(0f, 0.6f, 1f), 0.3f, 3, 1);
        catFaceStatus.transform.DOShakeRotation(1f, 70, 9, 0, true);
        yield return new WaitForSeconds(reactionTime);
        yield return new WaitUntil(CattyStatusIndicator);
    }

    private bool CattyStatusIndicator()
    {
        if (60 <= playerSurvival.GetCurrentBatteryPower && playerSurvival.GetCurrentBatteryPower < MAX_BATT_POWER)
            catFaceStatus.sprite = felineFeelingFaces[(int)FaceNames.Healthy];

        else if (25 <= playerSurvival.GetCurrentBatteryPower && playerSurvival.GetCurrentBatteryPower < 60)
            catFaceStatus.sprite = felineFeelingFaces[(int)FaceNames.GettingLow];

        else if (0 <= playerSurvival.GetCurrentBatteryPower && playerSurvival.GetCurrentBatteryPower < 25)
            catFaceStatus.sprite = felineFeelingFaces[(int)FaceNames.NearingDeath];
        
        return true;
    }

    private IEnumerator GameTimeTracker()
    {
        while (true)
        {
            Timer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(Timer / 60F);
            int seconds = Mathf.FloorToInt(Timer % 60F);
            int milliseconds = Mathf.FloorToInt((Timer * 1000F) % 1000F);

            timeMinutesValue.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}";
            timeMillisecondsValue.text = $"{milliseconds.ToString("000")}";
            yield return null;
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
        scoreText.text = $"{timeMinutesValue.text}:{timeMillisecondsValue.text}";
        DOTween.To(() => gameOverCG.alpha, x => gameOverCG.alpha = x, 1f, 1f);
    }
}

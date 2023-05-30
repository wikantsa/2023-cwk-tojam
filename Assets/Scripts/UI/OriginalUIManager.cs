//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//using TMPro;
//using DG.Tweening;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class OriginalUIManager : MonoBehaviour
//{
//    public static OriginalUIManager instance = null;
//    private bool initialised = false;
//    public UnityEvent addTargetCount;
//    public UnityEvent<EGameState> gameStateChanged;
//    [SerializeField] private TMP_Text coinValue;
//    private int totalTargets = 0;
//    [SerializeField] private TMP_Text currentScoreValue;
//    [SerializeField] private TMP_Text totalScoreValue;
//    [SerializeField] private TMP_Text scoreText;
//    [SerializeField] private TMP_Text timeMinutesValue;
//    [SerializeField] private TMP_Text timeMillisecondsValue;
//    [SerializeField] private Image coinIcon;

//    [SerializeField] private TextMeshProUGUI GameOverText;
//    private float FadeSpeed = 20.0f;
//    private int RolloverCharacterSpread = 10;

//    [SerializeField] private TMP_Text retryText;
//    [SerializeField] private TMP_Text exitText;
//    [SerializeField] private Image gameOverBg;

//    private void Awake(){
//        if(!initialised)
//            instance = this;
//        else
//            Destroy(this);
//    }
//    void Start()
//    {
//        if (gameStateChanged == null)
//            gameStateChanged = new UnityEvent<EGameState>();

//        if(addTargetCount == null)
//            addTargetCount = new UnityEvent();

//        gameStateChanged.AddListener(StateChanged);
//        addTargetCount.AddListener(IncrementTargetScore);
//    }

//    private void IncrementTargetScore(){
//        coinValue.text = (++totalTargets).ToString("000");
//    }

//    private float Timer;
//    private bool game_on = false;
//    private bool rotationFlipper = true;
//    private void Update(){
//        if(game_on){
//            Timer += Time.deltaTime;

//            int minutes = Mathf.FloorToInt(Timer / 60F);
//            int seconds = Mathf.FloorToInt(Timer % 60F);
//            int milliseconds = Mathf.FloorToInt((Timer * 1000F) % 1000F);

//            timeMinutesValue.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}";
//            timeMillisecondsValue.text = $"{milliseconds.ToString("000")}";
//        }
//        totalScoreValue.text = PointsManager.instance.PointsTotal.ToString();
//        currentScoreValue.text = PointsManager.instance.PointsCurrent.ToString();
//    }

//    private bool doOnce = false;
//    private void StateChanged(EGameState _gamestate){
//        switch(_gamestate){
//            case EGameState.Starting:
//                Debug.Log("Triggering starting state UI.");

//                currentScoreValue.text = "0";
//                totalScoreValue.text = "0";

//                coinValue.text = "000";
//                timeMinutesValue.text = "00:00 ";
//                timeMillisecondsValue.text = "000";

//                StartCoroutine("PlayCountdownUISequence");
//                break;

//            case EGameState.Game:
//                game_on = true;
//                StartCoroutine("ShakeAllTheMillis");
//                StartCoroutine("RotateThatCoin");
//                StartCoroutine("MakeThatScorePOP");
//                break;

//            case EGameState.GameOver:
//                game_on = false;
//                Debug.Log("Triggering ending state UI.");
//                StopCoroutine("ShakeAllTheMillis");
//                StopCoroutine("RotateThatCoin");
//                StopCoroutine("MakeThatScorePOP");
//                break;

//            case EGameState.End:
//                if(!doOnce) {
//                    StartCoroutine("FadeInGameOver");
//                    var goList = new List<GameObject>() {
//                        coinIcon.gameObject,
//                        timeMillisecondsValue.gameObject,
//                        coinValue.gameObject,
//                        timeMinutesValue.gameObject
//                    };

//                    goList.ForEach(go => {
//                        go.transform.DOMoveY(-99f, 20f, true);
//                        go.transform.DOShakeRotation(999f,25,5,5,true);
//                        if(go.GetComponent<TextMeshProUGUI>() != null)
//                            go.GetComponent<TextMeshProUGUI>().DOFade(0f, 5f);
//                        else
//                            go.GetComponent<Image>().DOFade(0f, 5f);
//                    });
//                    doOnce = true;
//                    gameOverBg.DOFade(1f, 6f);
//                    StartCoroutine(IGameOverRoutine());
//                }
//                break;

//            default:
//                break;
//        }
//    }

//    private IEnumerator IGameOverRoutine(){
//        while(true){
//            if(Input.GetKeyDown(KeyCode.Return))
//                RetryGame();
//            else if(Input.GetKeyDown(KeyCode.Escape))
//                ExitGame();
//            yield return null;
//        }
//    }
//    private void RetryGame(){
//        DOTween.KillAll();
//        SceneManager.LoadScene("main");
//    }

//    private void ExitGame(){
//        DOTween.KillAll();
//        SceneManager.LoadScene("start");
//    }
//    private IEnumerator ShakeAllTheMillis(){
//        while(true){
//            timeMillisecondsValue.transform.DOPunchScale(new Vector3(0.8f,0.6f,0.6f),0.3f, 3,1);
//            timeMillisecondsValue.transform.DOShakeRotation(1f,70,9,0,true);
//            yield return new WaitForSeconds(1f);
//        }
//    }

//    private IEnumerator RotateThatCoin(){
//        while(true){
//            if(rotationFlipper)
//                coinIcon.transform.DORotate(new Vector3(0,180f,0), 3f, RotateMode.Fast);
//            else
//                coinIcon.transform.DORotate(new Vector3(0,360f,0), 3f, RotateMode.Fast);
//            rotationFlipper = !rotationFlipper;
//            yield return new WaitForSeconds(3f);
//        }
//    }
//    private IEnumerator MakeThatScorePOP(){
//        while(true){
//            totalScoreValue.transform.DOPunchScale(new Vector2(0.2f,0.1f), 1/60f,10,1);
//            yield return new WaitForSeconds(1/60f);
//        }
//    }

//    private IEnumerator PlayCountdownUISequence()
//    {
//        CountdownUIController ui = gameObject.GetComponentInChildren<CountdownUIController>();
//        ui.ToggleCanvas(true);
//        yield return ui.DoCountdown();
//        GameStateManager.Instance.ChangeState(EGameState.Game);
//        yield return new WaitForSeconds(2);
//        ui.ToggleCanvas(false);
//    }

//    public void PlayOnceMoreWithFeelingUISequence()
//    {
//        StartCoroutine("OnceMoreWithFeelingUISequence");
//    }

//    private IEnumerator OnceMoreWithFeelingUISequence()
//    {
//        CountdownUIController ui = gameObject.GetComponentInChildren<CountdownUIController>();
//        ui.ToggleCanvas(true);
//        yield return ui.DoOnceMoreWithFeelingSequence();
//        yield return new WaitForSeconds(2);
//        ui.ToggleCanvas(false);
//    }

//    IEnumerator FadeInGameOver() {
//        // Set the whole text transparent
//        GameOverText.color = new Color
//            (
//                GameOverText.color.r,
//                GameOverText.color.g,
//                GameOverText.color.b,
//                0
//            );
//        GameOverText.ForceMeshUpdate();

//        TMP_TextInfo textInfo = GameOverText.textInfo;
//        Color32[] newVertexColors;

//        int currentCharacter = 0;
//        int startingCharacterRange = currentCharacter;
//        bool isRangeMax = false;

//        while (!isRangeMax)
//        {
//            int characterCount = textInfo.characterCount;

//            // Spread should not exceed the number of characters.
//            byte fadeSteps = (byte)Mathf.Max(1, 255 / RolloverCharacterSpread);

//            for (int i = startingCharacterRange; i < currentCharacter + 1; i++)
//            {
//                // Skip characters that are not visible (like white spaces)
//                if (!textInfo.characterInfo[i].isVisible) continue;

//                // Get the index of the material used by the current character.
//                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

//                // Get the vertex colors of the mesh used by this text element (character or sprite).
//                newVertexColors = textInfo.meshInfo[materialIndex].colors32;

//                // Get the index of the first vertex used by this text element.
//                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

//                // Get the current character's alpha value.
//                byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a + fadeSteps, 0, 255);

//                // Set new alpha values.
//                newVertexColors[vertexIndex + 0].a = alpha;
//                newVertexColors[vertexIndex + 1].a = alpha;
//                newVertexColors[vertexIndex + 2].a = alpha;
//                newVertexColors[vertexIndex + 3].a = alpha;


//                if (alpha == 255)
//                {
//                    startingCharacterRange += 1;

//                    if (startingCharacterRange == characterCount)
//                    {
//                        // Update mesh vertex data one last time.
//                        GameOverText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

//                        yield return new WaitForSeconds(1.0f);

//                        // Reset the text object back to original state.
//                        // GameOverText.ForceMeshUpdate();

//                        yield return new WaitForSeconds(1.0f);

//                        // Reset our counters.
//                        currentCharacter = 0;
//                        startingCharacterRange = 0;
//                        isRangeMax = true; // Would end the coroutine.

//                        // adjust game over text
//                        DOTween.Sequence().Append(
//                            scoreText.transform.DOMoveY(scoreText.transform.position.y - 50f, 2f, true)
//                        ).Join(
//                            scoreText.transform.DOScale(new Vector3(2f,2f,2f), 2f)
//                        ).Play();

//                        DOTween.Sequence().Append(
//                            totalScoreValue.transform.DOMoveY(totalScoreValue.transform.position.y - 120f, 2f, true)
//                        ).Join(
//                            totalScoreValue.transform.DOScale(new Vector3(2f,2f,2f), 2f)
//                        ).Play();

//                        DOTween.Sequence().Append(
//                            currentScoreValue.transform.DOMoveY(currentScoreValue.transform.position.y - 120f, 2f, true)
//                        ).Join(
//                            currentScoreValue.transform.DOScale(new Vector3(2f,2f,2f), 2f)
//                        ).Play();
//                        exitText.DOFade(1f, 3f);
//                        retryText.DOFade(1f, 3f);
//                    }
//                }
//            }
//            // Upload the changed vertex colors to the Mesh.
//            GameOverText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

//            if (currentCharacter + 1 < characterCount) currentCharacter += 1;

//            yield return new WaitForSeconds(0.4f - FadeSpeed * 0.01f);
//        }
//    }
//}


//// reference: https://forum.unity.com/threads/have-words-fade-in-one-by-one.525175/
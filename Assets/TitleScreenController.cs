using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleScreenController : MonoBehaviour
{
    public RectTransform _cursor = null;
    public RectTransform _start = null;
    public RectTransform _how = null;
    public RectTransform questionIcon = null;
    public RectTransform _exit = null;
    public List<RectTransform> cursorPositions = null;

    public GameObject robertTheRobot;
    public int TargetSceneIndex = 4;

    private float m_input = 0f;
    private const float DELAY = 0.2f;
    private int cursorPointer { get; set; } = 0;

    // Start is called before the first frame update
    void Start() {
        // set default cursor position
        if (_cursor) {
            _cursor.anchoredPosition = cursorPositions[cursorPointer].anchoredPosition;
            StartCoroutine("HandleTitleInputs");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (_how.gameObject.activeSelf)
            {
                CloseHowTo();
                return;
            }
            else {
                switch (cursorPointer)
                {
                    case 0:
                        LoadScene(TargetSceneIndex);
                        break;
                    case 1:
                        HowToPlay();
                        break;
                    case 2:
                        ExitGame();
                        break;
                }
            }
        }
    }

    private IEnumerator HandleTitleInputs()
    {
        while (true)
        {
            yield return new WaitForSeconds(DELAY);

            // when the player taps or holds down, move cursor down to next option
            // when the player taps or holds up, move cursor up to the next option
            // when player hits the confirmation button, exit or start game
            if (_how.gameObject.activeSelf == false)
            {
                m_input = Input.GetAxis("Vertical");

                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || m_input < 0f)
                    cursorPointer = cursorPointer < cursorPositions.Count - 1 ? ++cursorPointer : 0;

                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || m_input > 0f)
                    cursorPointer = cursorPointer > 0 ? --cursorPointer : cursorPositions.Count - 1;

                _cursor.position = cursorPositions[cursorPointer].position;
            }
        }
    }

    private Vector3 robertBaseRotation = new Vector3(0f, 240f, 0f);
    private Vector3 robertBasePosition = new Vector3(7f, -5f, 0.04f);
    private Vector3 robertsWatchingRotation = new Vector3(-18.2f, 286.02f, 1f);
    private Vector3 robertsWatchingPosition= new Vector3(6f, -6f, -0.6f);

    public void HowToPlay() {
        _how.gameObject.SetActive(true);
        questionIcon.gameObject.SetActive(true);

        questionIcon.DOShakeAnchorPos(99f, 15, 5, 45, false, true, ShakeRandomnessMode.Harmonic);

        robertTheRobot.transform.DOMove(robertsWatchingPosition, 0.3f, false);
        robertTheRobot.transform.DORotate(robertsWatchingRotation, 0.3f, RotateMode.Fast);
    }

    public void CloseHowTo() {
        _how.gameObject.SetActive(false);
        questionIcon.gameObject.SetActive(false);

        robertTheRobot.transform.DORotate(robertBaseRotation, 0.3f, RotateMode.Fast);
        robertTheRobot.transform.DOMove(robertBasePosition, 0.3f, false);
    }

    public void LoadScene(int index) {
        SceneManager.LoadScene(TargetSceneIndex);
    }

    public void ExitGame() {
        Application.Quit();
    }
}

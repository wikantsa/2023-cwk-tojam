using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public RectTransform _cursor;
    public RectTransform _start;
    public RectTransform _exit;
    public List<RectTransform> cursorPositions;

    public int TargetSceneIndex = 4;

    //public GameObject robertsArm;
    // add particle effect or something here that is fired
    // when start or exit are selected

    private float m_input = 0f;
    private const float DELAY = 0.2f;
    private static UIManager instance = null;

    private int cursorPointer { get; set; } = 0;
    
    void Awake()
    {
        // the scots object!!!
        if (UIManager.instance == null) {
            instance = this;
        }
        else {
            Destroy(this);
        }
        UIManager.DontDestroyOnLoad(this);

        // set default cursor position
        _cursor.anchoredPosition = cursorPositions[cursorPointer].anchoredPosition;

        StartCoroutine("HandleTitleInputs");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            if (cursorPointer == 0)
                LoadScene(TargetSceneIndex);
            else
                ExitGame();
    }

    private IEnumerator HandleTitleInputs()
    {
        while (true)
        {
            yield return new WaitForSeconds(DELAY);

            // when the player taps or holds down, move cursor down to next option
            // when the player taps or holds up, move cursor up to the next option
            // when player hits the confirmation button, exit or start game
            m_input = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || m_input < 0f)
                cursorPointer = cursorPointer < cursorPositions.Count - 1 ? ++cursorPointer : 0;

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || m_input > 0f)
                cursorPointer = cursorPointer > 0 ? --cursorPointer : cursorPositions.Count - 1;

            _cursor.position = cursorPositions[cursorPointer].position;
        }
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadSceneAsync(TargetSceneIndex);
    }

    public void ExitGame() {
        Application.Quit();
    }
}

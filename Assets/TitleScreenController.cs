using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleScreenController : MonoBehaviour
{
    public RectTransform _cursor = null;
    public RectTransform _start = null;
    public RectTransform _exit = null;
    public List<RectTransform> cursorPositions = null;

    public int TargetSceneIndex = 4;

    private float m_input = 0f;
    private const float DELAY = 0.2f;
    private int cursorPointer { get; set; } = 0;

    // Start is called before the first frame update
    void Start() {
        // set default cursor position
        if (_cursor)
        {
            _cursor.anchoredPosition = cursorPositions[cursorPointer].anchoredPosition;
            StartCoroutine("HandleTitleInputs");
        }
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

    public void ExitGame()
    {
        Application.Quit();
    }
}

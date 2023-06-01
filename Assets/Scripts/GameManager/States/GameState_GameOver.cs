using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameState_GameOver : GameState
{

    public GameState_GameOver(GameStateManager p_gameStateManager)
        : base(p_gameStateManager)
    {

    }

    protected override void EnterState(EGameState prevState)
    {
        MainUI.Instance.GameOverSequence();
    }

    protected override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
        }
    }

    protected override void LeaveState(EGameState nextState)
    {

    }

}


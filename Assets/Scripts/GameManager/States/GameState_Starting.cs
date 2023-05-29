using UnityEngine;
using System.Collections;

public class GameState_Starting : GameState
{

    public GameState_Starting(GameStateManager p_gameStateManager)
        : base(p_gameStateManager)
    {

    }

    protected override void EnterState(EGameState prevState)
    {
        if (!m_gameStateManager.SkipIntro)
            m_gameStateManager.IntroPlayable.Play();
    }

    protected override void UpdateState()
    {
        if (m_gameStateManager.IntroPlayable.state != UnityEngine.Playables.PlayState.Playing)
        {
            m_gameStateManager.ChangeState(EGameState.Game);
        }
        else if (m_gameStateManager.SkipIntro)
        {
            MainUI.Instance.fadeCG.alpha = 0;
            Camera.main.GetComponent<IsoCamera>().enabled = true;
            m_gameStateManager.ChangeState(EGameState.Game);
        }
    }

    protected override void LeaveState(EGameState nextState)
    {

    }
}

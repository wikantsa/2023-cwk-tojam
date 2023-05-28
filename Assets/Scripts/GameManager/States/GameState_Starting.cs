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
        MusicManager.Instance.PlayTrack(MusicTrack.FunkyBattle);
    }

    protected override void UpdateState()
    {
        m_gameStateManager.ChangeState(EGameState.Game);
    }

    protected override void LeaveState(EGameState nextState)
    {

    }
}

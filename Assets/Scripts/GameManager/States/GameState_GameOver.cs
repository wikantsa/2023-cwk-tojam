using UnityEngine;
using System.Collections;

public class GameState_GameOver : GameState
{

    public GameState_GameOver(GameStateManager p_gameStateManager)
        : base(p_gameStateManager)
    {

    }

    protected override void EnterState(EGameState prevState)
    {
        //PlayerStateManager.Instance.ChangeState(EPlayerState.Dead);
    }

    protected override void UpdateState()
    {

    }

    protected override void LeaveState(EGameState nextState)
    {

    }

}


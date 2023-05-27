using UnityEngine;
using System.Collections;

public class GameState_Game : GameState
{

    public GameState_Game(GameStateManager p_gameStateManager)
        : base(p_gameStateManager)
    {

    }

    protected override void EnterState(EGameState prevState)
    {
        //PlayerStateManager.Instance.ChangeState(EPlayerState.Alive);
    }

    protected override void UpdateState()
    {

    }

    protected override void LeaveState(EGameState nextState)
    {
        //PlayerStateManager.Instance.ChangeState(EPlayerState.Idle);
    }
}


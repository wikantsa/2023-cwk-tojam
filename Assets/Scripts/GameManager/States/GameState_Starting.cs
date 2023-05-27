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
        //PlayerStateManager.Instance.ChangeState(EPlayerState.Idle);
    }

    protected override void UpdateState()
    {
        
    }

    protected override void LeaveState(EGameState nextState)
    {

    }
}

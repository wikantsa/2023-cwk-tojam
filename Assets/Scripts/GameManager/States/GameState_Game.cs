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
        PlayerStateManager.Instance.ChangeState(EPlayerState.Alive);
        BotManager.Instance.NumberofEnemies = 25;
    }

    protected override void UpdateState()
    {

    }

    protected override void LeaveState(EGameState nextState)
    {
        BotManager.Instance.NumberofEnemies = 0;
        PlayerStateManager.Instance.ChangeState(EPlayerState.Idle);
    }
}


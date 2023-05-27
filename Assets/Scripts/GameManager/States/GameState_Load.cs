using UnityEngine;
using System.Collections;

public class GameState_Load : GameState
{
    public GameState_Load(GameStateManager p_gameStateManager)
        : base(p_gameStateManager)
    {

    }

    protected override void EnterState(EGameState prevState)
    {

    }

    protected override void UpdateState()
    {
        //m_gameStateManager.ChangeState(EGameState.Starting);
    }

    protected override void LeaveState(EGameState nextState)
    {

    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GameState
{
    protected GameStateManager m_gameStateManager;

    public GameState()
    {

    }

    public GameState(GameStateManager p_gameStateManager)
    {
        m_gameStateManager = p_gameStateManager;
    }

    public void Enter(EGameState p_prevState)
    {
        EnterState(p_prevState);
    }

    public void Leave(EGameState p_nextState)
    {
        LeaveState(p_nextState);
    }

    public void Update()
    {
        UpdateState();
    }

    //State control functions
    protected virtual void EnterState(EGameState prevState)
    {

    }

    protected virtual void LeaveState(EGameState nextState)
    {

    }

    protected virtual void UpdateState()
    {

    }
}

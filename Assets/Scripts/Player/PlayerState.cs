using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PlayerState
{
    protected PlayerStateManager m_playerStateManager;

    public PlayerState()
    {
    }

    public PlayerState(PlayerStateManager p_playerStateManager)
    {
        m_playerStateManager = p_playerStateManager;
    }

    public void Enter(EPlayerState p_prevState)
    {
        EnterState(p_prevState);
    }

    public void Leave(EPlayerState p_nextState)
    {
        LeaveState(p_nextState);
    }

    public void Update()
    {
        UpdateState();
    }

    //State control functions
    protected virtual void EnterState(EPlayerState prevState)
    {

    }

    protected virtual void LeaveState(EPlayerState nextState)
    {

    }

    protected virtual void UpdateState()
    {

    }
}

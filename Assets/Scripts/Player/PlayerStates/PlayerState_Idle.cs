using UnityEngine;
using System.Collections;

public class PlayerState_Idle : PlayerState
{

    public PlayerState_Idle(PlayerStateManager p_playerStateManager)
        : base(p_playerStateManager)
    {

    }

    protected override void EnterState(EPlayerState prevState)
    {
        m_playerStateManager.gameObject.GetComponent<PlayerController>().enabled = false;
    }

    protected override void UpdateState()
    {

    }

    protected override void LeaveState(EPlayerState nextState)
    {

    }

}

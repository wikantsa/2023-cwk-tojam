using UnityEngine;
using System.Collections;

public class PlayerState_Alive : PlayerState
{

    public PlayerState_Alive(PlayerStateManager p_playerStateManager)
        : base(p_playerStateManager)
    {

    }

    protected override void EnterState(EPlayerState prevState)
    {
        m_playerStateManager.gameObject.GetComponent<PlayerController>().enabled = true;
    }

    protected override void UpdateState()
    {

    }

    protected override void LeaveState(EPlayerState nextState)
    {

    }
}

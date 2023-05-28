using UnityEngine;
using System.Collections;

public class PlayerState_Dead : PlayerState
{
    public PlayerState_Dead(PlayerStateManager p_playerStateManager)
        : base(p_playerStateManager)
    {

    }

    protected override void EnterState(EPlayerState prevState)
    {
        m_playerStateManager.gameObject.GetComponent<PlayerController>().enabled = false;
        GameObject.Destroy(m_playerStateManager.gameObject);
    }

    protected override void UpdateState()
    {
        GameStateManager.Instance.ChangeState(EGameState.End);
    }

    protected override void LeaveState(EPlayerState nextState)
    {

    }
}

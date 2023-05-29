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
        GameStateManager.Instance.ChangeState(EGameState.GameOver);
        m_playerStateManager.gameObject.SetActive(false);
        ExplosionManager.Instance.SpawnExplosion(m_playerStateManager.transform.position, 3);
    }

    protected override void UpdateState()
    {

    }

    protected override void LeaveState(EPlayerState nextState)
    {

    }
}

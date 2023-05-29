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
        BotManager.Instance.IsPaused = false;
        MainUI.Instance.FadeCanvas(1f, 1f);
        Camera.main.GetComponent<IsoCamera>().enabled = true;
        MusicManager.Instance.PlayTrack(MusicTrack.FunkyBattle);
        MainUI.Instance.StartTimer();
    }

    protected override void UpdateState()
    {

    }

    protected override void LeaveState(EGameState nextState)
    {
        BotManager.Instance.IsPaused = true;
        PlayerStateManager.Instance.ChangeState(EPlayerState.Idle);
        MainUI.Instance.FadeCanvas(0f, 1f);
        MainUI.Instance.StopTimer();
        MusicManager.Instance.StopTrack();
    }
}


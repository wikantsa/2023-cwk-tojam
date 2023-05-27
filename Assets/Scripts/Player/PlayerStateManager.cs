using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerState
{
    Null = -1,
    Idle = 0,
    Alive = 1,
    Dead = 2
}

public class PlayerStateManager : MonoBehaviour
{
    private static PlayerStateManager instance = null;

    private Dictionary<EPlayerState, PlayerState> m_playerStateDictionary;

    private PlayerState m_currentState;

    private EPlayerState m_currentStateIndex;
    private EPlayerState m_nextStateIndex;

    private bool m_initialized = false;

    public static PlayerStateManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        //Highlander exception
        if (PlayerStateManager.Instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    public EPlayerState CurrentStateIndex
    {
        get { return m_currentStateIndex; }
    }

    public PlayerState CurrentState
    {
        get { return m_currentState; }
    }

    void Start()
    {
        m_currentState = null;

        m_currentStateIndex = EPlayerState.Null;
        m_nextStateIndex = EPlayerState.Null;

        InitializeStates();

        m_initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_initialized)
        {
            Debug.Log("Player State Manager failed to initialize");
            return;
        }

        if (m_currentState != null)
            m_currentState.Update();

        if (m_nextStateIndex != EPlayerState.Null)
        {
            m_currentState.Leave(m_nextStateIndex);
            m_currentState = m_playerStateDictionary[m_nextStateIndex];
            m_currentState.Enter(m_currentStateIndex);

            m_currentStateIndex = m_nextStateIndex;
            m_nextStateIndex = EPlayerState.Null;
        }
    }

    public void InitializeStates()
    {
        m_currentStateIndex = EPlayerState.Idle;

        m_playerStateDictionary = new Dictionary<EPlayerState, PlayerState>();

        m_playerStateDictionary.Add(EPlayerState.Idle, new PlayerState_Idle(this));
        m_playerStateDictionary.Add(EPlayerState.Alive, new PlayerState_Alive(this));
        m_playerStateDictionary.Add(EPlayerState.Dead, new PlayerState_Dead(this));

        m_currentState = m_playerStateDictionary[(EPlayerState)m_currentStateIndex];

        //set initial state to load
        m_currentState.Enter(m_currentStateIndex);
    }

    public void ChangeState(EPlayerState p_nextState)
    {
        if (!m_playerStateDictionary.ContainsKey(p_nextState))
            return;

        m_nextStateIndex = p_nextState;
    }
}

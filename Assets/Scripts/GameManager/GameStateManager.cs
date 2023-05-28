using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum EGameState
{
    Null = -1,
    Load = 0,
    Starting = 1,
    Game = 2,
    End = 3,
    GameOver = 4
}

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager instance = null;
   
    private Dictionary<EGameState, GameState> m_gameStateDictionary;    //maps the dance states to the dance state enum for reference

    private GameState m_currentState;                                      //dance state object the state machine is currently in

    private EGameState m_currentStateIndex;                               //enum reference to current dance state
    private EGameState m_nextStateIndex;                                  //enum reference to upcoming dance state

    private bool m_initialized = false;

    public EGameState CurrentStateIndex
    {
        get { return m_currentStateIndex; }
    }

    public GameState CurrentState
    {
        get { return m_currentState; }
    }

    // returns the single GameStateManager instance
    public static GameStateManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        //Highlander exception
        if (GameStateManager.Instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    void Start()
    {
        m_currentState = null;

        m_currentStateIndex = EGameState.Null;
        m_nextStateIndex = EGameState.Null;

        InitializeStates();

        m_initialized = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (!m_initialized)
        {
            //Debug.Log("Dance State Manager failed to initialize");
            return;
        }

        if (m_currentState != null)
            m_currentState.Update();

        if (m_nextStateIndex != EGameState.Null)
        {
            m_currentState.Leave(m_nextStateIndex);
            m_currentState = m_gameStateDictionary[m_nextStateIndex];
            m_currentState.Enter(m_currentStateIndex);

            m_currentStateIndex = m_nextStateIndex;
            m_nextStateIndex = EGameState.Null;
        }
    }

    public void InitializeStates()
    {
        m_currentStateIndex = EGameState.Load;

        m_gameStateDictionary = new Dictionary<EGameState, GameState>();

        m_gameStateDictionary.Add(EGameState.Load, new GameState_Load(this));
        m_gameStateDictionary.Add(EGameState.Starting, new GameState_Starting(this));
        m_gameStateDictionary.Add(EGameState.Game, new GameState_Game(this));
        m_gameStateDictionary.Add(EGameState.End, new GameState_End(this));
        m_gameStateDictionary.Add(EGameState.GameOver, new GameState_GameOver(this));

        m_currentState = m_gameStateDictionary[(EGameState)m_currentStateIndex];

        //set initial state to load
        m_currentState.Enter(m_currentStateIndex);
    }

    public void ChangeState(EGameState p_nextState)
    {
        if (!m_gameStateDictionary.ContainsKey(p_nextState))
            return;

        m_nextStateIndex = p_nextState;
        //UIManager.instance.gameStateChanged.Invoke(p_nextState);
    }
}

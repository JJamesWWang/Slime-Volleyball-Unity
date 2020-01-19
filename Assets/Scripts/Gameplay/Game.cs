using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Side
{
    UNSET,
    LEFT,
    RIGHT
}

public abstract class Game : MonoBehaviour
{
    [SerializeField] protected Volleyball VolleyballPrefab = default;
    [SerializeField] protected PlayerController Player1Prefab = default;
    [SerializeField] protected PlayerController Player2Prefab = default;
    [SerializeField] protected PlayerController Player3Prefab = default;
    [SerializeField] protected PlayerController Player4Prefab = default;

    [HideInInspector] public bool IsPaused { get; protected set; }
    [HideInInspector] public bool IsOver { get; protected set; }

    protected int leftScore;
    protected int rightScore;
    protected PlayerController player1;
    protected PlayerController player2;
    protected PlayerController player3;
    protected PlayerController player4;
    protected List<PlayerController> players = new List<PlayerController>();
    protected List<Volleyball> volleyballs = new List<Volleyball>();

    public static Game Instance { get; protected set; }

    // Constants
    public abstract float CENTER { get; }
    public abstract float DROP_HEIGHT { get; }
    public abstract float DROP_DELAY { get; }
    public abstract float GROUND { get; }
    public abstract float CEILING { get; }
    public abstract float LEFT_WALLX { get; }
    public abstract float RIGHT_WALLX { get; }
    public abstract float NET_TOP { get; }
    public abstract float NET_LEFT { get; }
    public abstract float NET_RIGHT { get; }
    public virtual Vector2 PLAYER1_DEFAULT_POSITION {
        get { return new Vector2(-18, 0); } }
    public virtual Vector2 PLAYER2_DEFAULT_POSITION {
        get { return new Vector2(17, 0); } }
    public virtual Vector2 PLAYER3_DEFAULT_POSITION {
        get { return new Vector2(-9, 0); } }
    public virtual Vector2 PLAYER4_DEFAULT_POSITION {
        get { return new Vector2(8, 0); } }
    public float GAME_SPEED {
        get { return PlayerPrefs.GetFloat("Game Speed"); } }
    public int POINTS_TO_WIN {
        get { return PlayerPrefs.GetInt("Points To Win"); } }
    public int VOLLEYBALLS {
        get { return PlayerPrefs.GetInt("Volleyballs"); } }

    protected virtual void AddListeners()
    {
        Messenger.AddListener(GameEvent.GAME_STARTED, StartGame);
        Messenger.AddListener<Side>(GameEvent.POINT_STARTED, StartPoint);
        Messenger.AddListener<Side>(GameEvent.POINT_ENDED, EndPoint);
        Messenger.AddListener<Side>(GameEvent.GAME_ENDED, EndGame);
        Messenger.AddListener(GameEvent.GAME_PAUSED, Pause);
        Messenger.AddListener(GameEvent.GAME_UNPAUSED, Unpause);
        Messenger.AddListener(GameEvent.GAME_RESET, ResetGame);
    }

    protected virtual void Setup()
    {
        Init();
        InstantiatePlayers();
    }

    protected virtual void Init()
    {
        leftScore = 0;
        rightScore = 0;
        IsOver = false;
        Messenger.Broadcast(GameEvent.SCORE_UPDATED, 0, 0);
    }

    protected virtual void InstantiatePlayers()
    {
        player1 = Instantiate(Player1Prefab);
        players.Add(player1);

        player2 = Instantiate(Player2Prefab);
        players.Add(player2);

        if (PlayerPrefs.GetInt("Player 3") == 1)
        {
            player3 = Instantiate(Player3Prefab);
            players.Add(player3);
        }
        if (PlayerPrefs.GetInt("Player 4") == 1)
        {
            player4 = Instantiate(Player4Prefab);
            players.Add(player4);
        }
    }

    protected abstract void UseDefaults();
    protected abstract void StartGame();
    protected abstract void StartPoint(Side side);
    protected abstract void EndPoint(Side side);
    protected virtual void ResetPlayers()
    {
        foreach (PlayerController player in players)
        {
            player.Reset();

            if (player.gameObject.name.Contains("Player 1"))
                player.transform.position = PLAYER1_DEFAULT_POSITION;
            else if (player.gameObject.name.Contains("Player 2"))
                player.transform.position = PLAYER2_DEFAULT_POSITION;
            else if (player.gameObject.name.Contains("Player 3"))
                player.transform.position = PLAYER3_DEFAULT_POSITION;
            else if (player.gameObject.name.Contains("Player 4"))
                player.transform.position = PLAYER4_DEFAULT_POSITION;
            else
                Debug.Log("Unknown player detected: " + player.gameObject.name);
        }
    }

    protected virtual void EndGame(Side side)
    {
        IsOver = true;
    }

    protected virtual void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0;
    }

    protected virtual void Unpause()
    {
        IsPaused = false;
        Time.timeScale = GAME_SPEED;
    }

    protected virtual void ResetGame()
    {
        Setup();
    }

}

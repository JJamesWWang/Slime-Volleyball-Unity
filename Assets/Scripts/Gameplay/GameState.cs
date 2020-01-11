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

public abstract class GameState : MonoBehaviour
{
    [SerializeField] protected GameObject VolleyballPrefab = default;
    [SerializeField] protected PlayerController Player1Prefab = default;
    [SerializeField] protected PlayerController Player2Prefab = default;
    [SerializeField] protected PlayerController Player3Prefab = default;
    [SerializeField] protected PlayerController Player4Prefab = default;

    [SerializeField] protected MainUI ui = default;
    protected int leftScore;
    protected int rightScore;
    [HideInInspector] public bool IsPaused { get; protected set; }
    [HideInInspector] public bool IsOver { get; protected set; }

    protected PlayerController player1;
    protected PlayerController player2;
    protected PlayerController player3;
    protected PlayerController player4;
    protected List<PlayerController> players = new List<PlayerController>();
    protected List<GameObject> volleyballs = new List<GameObject>();

    public static GameState Instance { get; protected set; }

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
    public virtual float GAME_SPEED {
        get { return PlayerPrefs.GetFloat("Game Speed"); } }
    public virtual int POINTS_TO_WIN {
        get { return PlayerPrefs.GetInt("Points To Win"); } }
    public virtual int VOLLEYBALLS {
        get { return PlayerPrefs.GetInt("Volleyballs"); } }

    // Variables
    public abstract void Setup();
    public abstract void EndPoint(Side side);
    public abstract void ResetPlayers();
    public abstract void SpawnBall(bool leftWon);

    protected virtual void Awake()
    {
        leftScore = 0;
        rightScore = 0;
        IsPaused = false;
        IsOver = false;
        Unpause();
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IsOver)
        {
            IsPaused = !IsPaused;
            if (IsPaused)
            {
                Pause();
                ui.Pause();
            }
            else
            {
                Unpause();
                ui.Unpause();
            }
        }
    }

    public virtual void Pause()
    {
        Time.timeScale = 0;
    }

    public virtual void Unpause()
    {
        Time.timeScale = GAME_SPEED;
    }

    public virtual void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGame : Game
{
    [SerializeField] protected GameObject Map = default;

    public override float CENTER { get { return -0.5f; } }
    public override float DROP_HEIGHT { get { return 15f; } }
    public override float DROP_DELAY { get { return 0.1f; } }
    public override float GROUND { get { return 0f; } }
    public override float CEILING { get { return 22f; } }
    public override float LEFT_WALLX { get { return -21f; } }
    public override float RIGHT_WALLX { get { return 20f; } }
    public override float NET_TOP { get { return 3f; } }
    public override float NET_LEFT { get { return -1f; } }
    public override float NET_RIGHT { get { return 0f; } }

    protected override void Awake()
    {
        base.Awake();
        if (PlayerPrefs.GetString("Game Mode").Equals("Normal"))
        {
            Instance = this;
            Map.SetActive(true);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("Game Mode").Equals("Normal"))
        {
            AddListeners();
            Setup();
        }
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        Messenger.AddListener<Volleyball, ScoreArea>(
            GameEvent.SCORE_AREA_CONTACT, (v, a) => {
                Messenger.Broadcast(GameEvent.POINT_ENDED, a.side);
            });
    }

    protected override void Setup()
    {
        base.Setup();
        Messenger.Broadcast(GameEvent.GAME_STARTED);
    }

    protected override void UseDefaults()
    {
        PlayerPrefs.SetFloat("Game Speed", 1f);
        PlayerPrefs.SetInt("Points To Win", 11);
        PlayerPrefs.SetInt("Volleyballs", 1);
        PlayerPrefs.SetInt("Spikes", 1);
        PlayerPrefs.SetInt("Use Defaults", 1);
        PlayerPrefs.SetInt("Ball Collision", 0);
        PlayerPrefs.SetInt("Horizontal Speed", 10);
        PlayerPrefs.SetInt("Vertical Speed", 8);
        PlayerPrefs.SetFloat("Jump Time", 1f);
        PlayerPrefs.SetFloat("Hover Time", 0.15f);
        PlayerPrefs.SetInt("Player 3", 0);
        PlayerPrefs.SetInt("Player 4", 0);
    }

    protected override void StartGame()
    {
        Messenger.Broadcast<Side>(GameEvent.POINT_STARTED, Side.LEFT);
    }

    protected override void StartPoint(Side side)
    {
        ResetPlayers();
        StartCoroutine(SpawnBall(side, VOLLEYBALLS));
    }

    private IEnumerator SpawnBall(Side side, int numVolleyballs)
    {
        yield return DisableUserInput(0.5f);
        while (numVolleyballs > 0)
        {
            float volleyballX = Game.Instance.CENTER;
            if (side == Side.RIGHT)
                volleyballX = PLAYER1_DEFAULT_POSITION.x;
            else if (side == Side.LEFT)
                volleyballX = PLAYER2_DEFAULT_POSITION.x;

            Volleyball volleyball = Instantiate(VolleyballPrefab,
                new Vector2(volleyballX, DROP_HEIGHT), Quaternion.identity)
                .GetComponent<Volleyball>();

            volleyballs.Add(volleyball);
            Messenger.Broadcast(GameEvent.VOLLEYBALL_DROPPED, volleyball, side);
            yield return new WaitForSecondsRealtime(DROP_DELAY);
            numVolleyballs -= 1;

            if (side == Side.LEFT)
                side = Side.RIGHT;
            else if (side == Side.RIGHT)
                side = Side.LEFT;
        }
    }

    private IEnumerator DisableUserInput(float seconds)
    {
        foreach (PlayerController player in players)
            player.AllowMovement(false);
        yield return new WaitForSecondsRealtime(seconds);
        foreach (PlayerController player in players)
            player.AllowMovement(true);
    }

    protected override void EndPoint(Side side)
    {
        foreach (Volleyball volleyball in volleyballs)
            Destroy(volleyball.gameObject);
        volleyballs.Clear();

        if (side == Side.LEFT)
            rightScore += 1;
        else if (side == Side.RIGHT)
            leftScore += 1;
        Messenger.Broadcast(GameEvent.SCORE_UPDATED, leftScore, rightScore);

        if (leftScore == POINTS_TO_WIN || rightScore == POINTS_TO_WIN)
        {
            Messenger.Broadcast(GameEvent.GAME_PAUSED);
            Messenger.Broadcast(GameEvent.GAME_ENDED, side);
        }
        else
            Messenger.Broadcast(GameEvent.POINT_STARTED, side);

    }

    protected override void EndGame(Side side)
    {
        base.EndGame(side);
    }

    protected override void ResetGame()
    {
        base.ResetGame();
        foreach (Volleyball volleyball in volleyballs)
            Destroy(volleyball.gameObject);
        volleyballs.Clear();
        Messenger.Broadcast(GameEvent.GAME_STARTED);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGame : Game
{
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
        }
    }

    protected override void Start()
    {
        base.Start();
        Setup();
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        Messenger.AddListener<Volleyball, ScoreArea>(
            GameEvent.SCORE_AREA_CONTACT, OnScoreAreaContact);
    }

    protected override void Setup()
    {
        base.Setup();
        Messenger.Broadcast(GameEvent.GAME_STARTED);
    }

    protected override void StartGame()
    {
        Messenger.Broadcast<Side>(GameEvent.POINT_STARTED, Side.LEFT);
    }

    protected override void StartPoint(Side side)
    {
        ResetPlayers();
        StartCoroutine(SpawnBall(side == Side.RIGHT, VOLLEYBALLS));
    }

    private IEnumerator SpawnBall(bool left, int numVolleyballs)
    {
        foreach (PlayerController player in players)
            player.AllowMovement(false);
        yield return new WaitForSecondsRealtime(0.5f);
        foreach (PlayerController player in players)
            player.AllowMovement(true);

        while (numVolleyballs > 0)
        {
            float volleyballX;
            if (left)
                volleyballX = PLAYER1_DEFAULT_POSITION.x;
            else
                volleyballX = PLAYER2_DEFAULT_POSITION.x;

            GameObject volleyball = Instantiate(VolleyballPrefab,
                new Vector2(volleyballX, DROP_HEIGHT), Quaternion.identity);

            volleyballs.Add(volleyball);
            yield return new WaitForSecondsRealtime(DROP_DELAY);
            numVolleyballs -= 1;
            left = !left;
        }
    }

    private void OnScoreAreaContact(Volleyball volleyball, ScoreArea area)
    {
        Messenger.Broadcast(GameEvent.POINT_ENDED, area.side);
    }

    protected override void EndPoint(Side side)
    {
        foreach (GameObject volleyball in volleyballs)
            Destroy(volleyball);
        volleyballs.Clear();

        if (side == Side.LEFT)
            rightScore += 1;
        else if (side == Side.RIGHT)
            leftScore += 1;
        Messenger.Broadcast(GameEvent.SCORE_UPDATED, leftScore, rightScore);

        if (leftScore == POINTS_TO_WIN || rightScore == POINTS_TO_WIN)
            Messenger.Broadcast(GameEvent.GAME_ENDED, side);
        else
            Messenger.Broadcast(GameEvent.POINT_STARTED, side);

    }

    protected override void EndGame(Side side)
    {
        base.EndGame(side);
        Messenger.Broadcast(GameEvent.GAME_PAUSED);
    }

    protected override void ResetGame()
    {
        base.ResetGame();
        foreach (GameObject volleyball in volleyballs)
            Destroy(volleyball);
        volleyballs.Clear();
        Messenger.Broadcast(GameEvent.GAME_STARTED);
    }
}

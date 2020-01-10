using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGame : GameState
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
            Setup();
        }
    }

    public override void Setup()
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

        SpawnBall(true);
    }

    public override void EndPoint(Side side)
    {
        foreach (GameObject volleyball in volleyballs)
            Destroy(volleyball);

        volleyballs.Clear();

        if (side == Side.LEFT)
            rightScore += 1;
        else if (side == Side.RIGHT)
            leftScore += 1;

        ui.UpdateScore(leftScore, rightScore);
        if (leftScore == POINTS_TO_WIN || rightScore == POINTS_TO_WIN)
            HandleWin();
        else
            StartCoroutine(StartNextPoint(side));
    }

    private void HandleWin()
    {
        Pause();
        IsOver = true;
        ui.ShowWinner(leftScore == POINTS_TO_WIN);
    }

    IEnumerator StartNextPoint(Side side)
    {
        ui.DisplayScore(true);
        yield return new WaitForSeconds(1);
        ui.DisplayScore(false);
        ResetPlayers();
        SpawnBall(side == Side.RIGHT);
    }

    public override void ResetPlayers()
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

    public override void SpawnBall(bool left)
    {
        StartCoroutine(SpawnBall(left, VOLLEYBALLS));
    }

    private IEnumerator SpawnBall(bool left, int numVolleyballs)
    {
        while (numVolleyballs > 0)
        {
            float volleyballX;
            if (left)
                volleyballX = Player1Prefab.transform.position.x;
            else
                volleyballX = Player2Prefab.transform.position.x;

            GameObject volleyball = Instantiate(VolleyballPrefab,
                new Vector2(volleyballX, DROP_HEIGHT), Quaternion.identity);

            volleyballs.Add(volleyball);
            yield return new WaitForSecondsRealtime(DROP_DELAY);
            numVolleyballs -= 1;
            left = !left;
        }
    }
}

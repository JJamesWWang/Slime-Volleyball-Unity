using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] Text leftScoreText = default;
    [SerializeField] Text rightScoreText = default;
    [SerializeField] Text winnerText = default;
    [SerializeField] GameObject pauseText = default;
    [SerializeField] GameObject pausePanel = default;

    private void Awake()
    {
        DisplayScore(false);
        winnerText.gameObject.SetActive(false);
        pauseText.gameObject.SetActive(false);
        pausePanel.SetActive(false);
    }

    private void Start()
    {
        AddListeners();
    }

    void AddListeners()
    {
        Messenger.AddListener(GameEvent.GAME_PAUSED, Pause);
        Messenger.AddListener(GameEvent.GAME_UNPAUSED, Unpause);
        Messenger.AddListener<int, int>(GameEvent.SCORE_UPDATED, UpdateScore);
        Messenger.AddListener<Side>(GameEvent.POINT_STARTED, (s) => {
            StartCoroutine(FlashScore());
        });
        Messenger.AddListener<Side>(GameEvent.GAME_ENDED, ShowWinner);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Game.Instance.IsOver)
        {
            if (!Game.Instance.IsPaused)
                Messenger.Broadcast(GameEvent.GAME_PAUSED);
            else
                Messenger.Broadcast(GameEvent.GAME_UNPAUSED);
        }
    }
    public void OnRestart()
    {
        Messenger.Broadcast(GameEvent.GAME_RESET);
        Messenger.Broadcast(GameEvent.GAME_UNPAUSED);
    }

    void UpdateScore(int leftScore, int rightScore)
    {
        leftScoreText.text = "" + leftScore;
        rightScoreText.text = "" + rightScore;
    }

    IEnumerator FlashScore()
    {
        DisplayScore(true);
        yield return new WaitForSecondsRealtime(1);
        if (!Game.Instance.IsPaused)
            DisplayScore(false);
    }

    void DisplayScore(bool show)
    {
        leftScoreText.gameObject.SetActive(show);
        rightScoreText.gameObject.SetActive(show);
    }

    public void Pause()
    {
        pauseText.SetActive(true);
        pausePanel.SetActive(true);
        DisplayScore(true);
    }

    public void Unpause()
    {
        pauseText.SetActive(false);
        pausePanel.SetActive(false);
        DisplayScore(false);
        winnerText.gameObject.SetActive(false);
    }

    void ShowWinner(Side side)
    {
        DisplayScore(true);
        pausePanel.SetActive(true);
        if (side == Side.RIGHT)
            winnerText.text = "Left Team Wins!";
        else if (side == Side.LEFT)
            winnerText.text = "Right Team Wins!";
        else
            winnerText.text = "Victory!";

        pauseText.SetActive(false);
        winnerText.gameObject.SetActive(true);
    }

    public void OnMainMenu()
    {
        Messenger.Broadcast(GameEvent.GAME_UNPAUSED);
        SceneManager.LoadScene("Menu");
    }

}

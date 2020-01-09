using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] Text leftScoreText;
    [SerializeField] Text rightScoreText;
    [SerializeField] Text winnerText;
    [SerializeField] GameObject pauseText;
    [SerializeField] GameObject pausePanel;

    private void Awake()
    {
        DisplayScore(false);
        winnerText.gameObject.SetActive(false);
        pauseText.gameObject.SetActive(false);
        pausePanel.SetActive(false);
    }

    public void OnRestart()
    {
        GameState.Instance.ResetGame();
    }

    public void UpdateScore(int leftScore, int rightScore)
    {
        leftScoreText.text = "" + leftScore;
        rightScoreText.text = "" + rightScore;
    }

    public void DisplayScore(bool show)
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
    }

    public void ShowWinner(bool leftWon)
    {
        DisplayScore(true);
        pausePanel.SetActive(true);
        if (leftWon)
        {
            winnerText.text = "Left Team Wins!";
        } else
        {
            winnerText.text = "Right Team Wins!";
        }
        pauseText.SetActive(false);
        winnerText.gameObject.SetActive(true);

    }

    public void OnMainMenu()
    {
        GameState.Instance.Unpause();
        SceneManager.LoadScene("Menu");
    }

}

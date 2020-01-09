using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] Text gameModeText;
    //string[] gameModes = { "Normal", "Wall Protect", "Drop Shot",
    //    "Rally (4P)" };
    string[] gameModes = { "Normal" };
    int gameModeIndex = 0;

    private void Start()
    {
        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);

        PlayerPrefs.SetString("Game Mode",
            PlayerPrefs.GetString("Game Mode", "Normal"));

        string gameMode = PlayerPrefs.GetString("Game Mode");
        for (int i = 0; i < gameModes.Length; ++i)
        {
            if (gameModes[i].Equals(gameMode))
            {
                gameModeIndex = i;
            }
        }
        gameModeText.text = gameModes[gameModeIndex];
    }
    int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnGameModeLeft()
    {
        gameModeIndex = Mod(gameModeIndex - 1, gameModes.Length);
        gameModeText.text = gameModes[gameModeIndex];
        PlayerPrefs.SetString("Game Mode", gameModes[gameModeIndex]);
    }
    public void OnGameModeRight()
    {
        gameModeIndex = Mod(gameModeIndex + 1, gameModes.Length);
        gameModeText.text = gameModes[gameModeIndex];
        PlayerPrefs.SetString("Game Mode", gameModes[gameModeIndex]);
    }

    public void OnSettings()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}

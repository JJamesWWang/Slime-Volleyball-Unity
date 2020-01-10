using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] GameObject menuPanel = default;
    [SerializeField] GameObject settingsPanel = default;

    public float defaultGameSpeed;
    public int defaultPointsToWin;
    public int defaultVolleyballs;
    public int defaultHorizontalSpeed;
    public int defaultVerticalSpeed;
    public float defaultJumpTime;
    public float defaultHoverTime;

    [SerializeField] Text gameSpeedValue = default;
    [SerializeField] Text pointsToWinValue = default;
    [SerializeField] Text volleyballsValue = default;
    [SerializeField] Text horizontalSpeedValue = default;
    [SerializeField] Text verticalSpeedValue = default;
    [SerializeField] Text jumpTimeValue = default;
    [SerializeField] Text hoverTimeValue = default;

    [SerializeField] Slider gameSpeedSlider = default;
    [SerializeField] Slider pointsToWinSlider = default;
    [SerializeField] Slider volleyballsSlider = default;
    [SerializeField] Toggle spikesToggle = default;
    [SerializeField] Slider horizontalSpeedSlider = default;
    [SerializeField] Slider verticalSpeedSlider = default;
    [SerializeField] Slider jumpTimeSlider = default;
    [SerializeField] Slider hoverTimeSlider = default;
    [SerializeField] Toggle player3Toggle = default;
    [SerializeField] Toggle player4Toggle = default;

    private void Awake()
    {
        SetDefaultSettings();
        DisplaySavedSettings();
    }

    private void SetDefaultSettings()
    {
        PlayerPrefs.SetFloat("Game Speed",
            PlayerPrefs.GetFloat("Game Speed", defaultGameSpeed));
        PlayerPrefs.SetInt("Points To Win",
            PlayerPrefs.GetInt("Points To Win", defaultPointsToWin));
        PlayerPrefs.SetInt("Volleyballs",
            PlayerPrefs.GetInt("Volleyballs", defaultVolleyballs));
        PlayerPrefs.SetInt("Spikes",
            PlayerPrefs.GetInt("Spikes", 1));
        PlayerPrefs.SetInt("Horizontal Speed",
            PlayerPrefs.GetInt("Horizontal Speed", defaultHorizontalSpeed));
        PlayerPrefs.SetInt("Vertical Speed",
            PlayerPrefs.GetInt("Vertical Speed", defaultVerticalSpeed));
        PlayerPrefs.SetFloat("Jump Time",
            PlayerPrefs.GetFloat("Jump Time", defaultJumpTime));
        PlayerPrefs.SetFloat("Hover Time",
            PlayerPrefs.GetFloat("Hover Time", defaultHoverTime));
        PlayerPrefs.SetInt("Player 3",
            PlayerPrefs.GetInt("Player 3", 0));
        PlayerPrefs.SetInt("Player 4",
            PlayerPrefs.GetInt("Player 4", 0));
    }

    private void DisplaySavedSettings()
    {
        gameSpeedSlider.value = PlayerPrefs.GetFloat("Game Speed");
        pointsToWinSlider.value = PlayerPrefs.GetInt("Points To Win");
        volleyballsSlider.value = PlayerPrefs.GetInt("Volleyballs");
        spikesToggle.isOn = PlayerPrefs.GetInt("Spikes") == 1;
        horizontalSpeedSlider.value = PlayerPrefs.GetInt("Horizontal Speed");
        verticalSpeedSlider.value = PlayerPrefs.GetInt("Vertical Speed");
        jumpTimeSlider.value = PlayerPrefs.GetFloat("Jump Time");
        hoverTimeSlider.value = PlayerPrefs.GetFloat("Hover Time");
        player3Toggle.isOn = PlayerPrefs.GetInt("Player 3") == 1;
        player4Toggle.isOn = PlayerPrefs.GetInt("Player 4") == 1;
    }

    public void OnGameSpeedChanged(float value)
    {
        value = Mathf.Round(value * 4) / 4f;
        gameSpeedValue.text = "" + value;
    }

    public void OnPointsToWinChanged(float valuef)
    {
        int value = Mathf.RoundToInt(valuef);
        pointsToWinValue.text = "" + value;
    }

    public void OnVolleyballsChanged(float valuef)
    {
        int value = Mathf.RoundToInt(valuef);
        volleyballsValue.text = "" + value;
    }

    public void OnHorizontalSpeedChanged(float valuef)
    {
        int value = Mathf.RoundToInt(valuef);
        horizontalSpeedValue.text = "" + value;
    }

    public void OnVerticalSpeedChanged(float valuef)
    {
        int value = Mathf.RoundToInt(valuef);
        verticalSpeedValue.text = "" + value;
    }

    public void OnJumpTimeChanged(float value)
    {
        value = Mathf.Round(value * 20) / 20f;
        jumpTimeValue.text = "" + value;
    }

    public void OnHoverTimeChanged(float value)
    {
        value = Mathf.Round(value * 20) / 20f;
        hoverTimeValue.text = "" + value;
    }

    public void OnDefault()
    {
        gameSpeedSlider.value = defaultGameSpeed;
        pointsToWinSlider.value = defaultPointsToWin;
        volleyballsSlider.value = defaultVolleyballs;
        spikesToggle.isOn = true;
        horizontalSpeedSlider.value = defaultHorizontalSpeed;
        verticalSpeedSlider.value = defaultVerticalSpeed;
        jumpTimeSlider.value = defaultJumpTime;
        hoverTimeSlider.value = defaultHoverTime;
        player3Toggle.isOn = false;
        player4Toggle.isOn = false;
    }
    public void OnCancel()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OnConfirm()
    {
        PlayerPrefs.SetFloat("Game Speed",
            float.Parse(gameSpeedValue.text));
        PlayerPrefs.SetInt("Spikes", spikesToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("Points To Win",
            int.Parse(pointsToWinValue.text));
        PlayerPrefs.SetInt("Volleyballs",
            int.Parse(volleyballsValue.text));
        PlayerPrefs.SetInt("Horizontal Speed",
            int.Parse(horizontalSpeedValue.text));
        PlayerPrefs.SetInt("Vertical Speed",
            int.Parse(verticalSpeedValue.text));
        PlayerPrefs.SetFloat("Jump Time",
            float.Parse(jumpTimeValue.text));
        PlayerPrefs.SetFloat("Hover Time",
            float.Parse(hoverTimeValue.text));
        PlayerPrefs.SetInt("Player 3", player3Toggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("Player 4", player4Toggle.isOn ? 1 : 0);

        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

}

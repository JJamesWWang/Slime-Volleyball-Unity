using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsUI : MonoBehaviour
{
    [SerializeField] GameObject instructionsPanel = default;
    [SerializeField] Sprite[] tutorialImages = default;
    [SerializeField] Image currentImage = default;
    [SerializeField] Text numbersText = default;
    int imageIndex = 0;
    bool instructionsShown;

    private void Start()
    {
        Messenger.AddListener(GameEvent.GAME_STARTED, OnGameStart);
    }

    private void OnGameStart()
    {
        if (!instructionsShown)
        {
            Messenger.Broadcast(GameEvent.GAME_PAUSED);
            gameObject.SetActive(true);
            instructionsShown = true;
        }
    }

    public void OnLeftButton()
    {
        imageIndex = Mathf.Max(imageIndex - 1, 0);
        currentImage.sprite = tutorialImages[imageIndex];
        numbersText.text = (imageIndex + 1) + " / " + tutorialImages.Length;
    }

    public void OnRightButton()
    {
        imageIndex = Mathf.Min(imageIndex + 1, tutorialImages.Length - 1);
        currentImage.sprite = tutorialImages[imageIndex];
        numbersText.text = (imageIndex + 1) + " / " + tutorialImages.Length;
    }

    public void OnClose()
    {
        instructionsPanel.SetActive(false);
        Messenger.Broadcast(GameEvent.GAME_UNPAUSED);
    }
}

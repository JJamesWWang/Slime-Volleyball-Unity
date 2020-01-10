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

    private void Start()
    {
        if (GameState.Instance != null && !GameState.Instance.IsOver)
        {
            GameState.Instance.Pause();
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
        imageIndex = Mathf.Min(imageIndex + 1, tutorialImages.Length);
        currentImage.sprite = tutorialImages[imageIndex];
        numbersText.text = (imageIndex + 1) + " / " + tutorialImages.Length;
    }

    public void OnClose()
    {
        instructionsPanel.SetActive(false);
        if (GameState.Instance != null && !GameState.Instance.IsOver)
        {
            GameState.Instance.Unpause();
        }
    }
}

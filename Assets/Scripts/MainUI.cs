using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    private int score = 300;
    [SerializeField] private int deltaScore = 10;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image processBar;

    private void Awake()
    {
        scoreText.text = score.ToString();
        processBar.fillAmount = 0f;
    }

    private void OnEnable()
    {
        BlockManager.UpdateBlockStatus += UpdateProcessBar;
    }

    private void OnDisable()
    {
        BlockManager.UpdateBlockStatus -= UpdateProcessBar;
    }

    public void UpdateScore()
    {
        score += deltaScore;
        scoreText.text = score.ToString();
    }

    public void ResetProcessBar()
    {
        processBar.fillAmount = 0f;
    }

    public void UpdateProcessBar(float progress)
    {
        Debug.Log("Updating process bar to: " + progress);
        processBar.fillAmount = progress;
    }
}

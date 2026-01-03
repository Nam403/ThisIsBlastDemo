using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    private bool isActive = false;
    public void RetryLevel()
    {
        GameManager.Instance.RetryLevel();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Show()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}

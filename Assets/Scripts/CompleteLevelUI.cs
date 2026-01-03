using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteLevelUI : MonoBehaviour
{
    private bool isActive = false;
    public void NextLevel()
    {
        GameManager.Instance.LoadNextLevel();
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

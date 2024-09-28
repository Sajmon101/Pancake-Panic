using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject LosePanel;
    [SerializeField] private OrderGenerator OrderGenerator;

    private void OnEnable()
    {
        OrderGenerator.OnGameWin += OnGameWin;
        OrderGenerator.OnGameLose += OnGameLose;
    }

    private void OnGameWin(object sender, System.EventArgs e)
    {
        WinPanel.SetActive(true);
        Player.Instance.DisablePlayerMovement();
    }

    private void OnGameLose(object sender, System.EventArgs e)
    {
        LosePanel.SetActive(true);
        Player.Instance.DisablePlayerMovement();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}

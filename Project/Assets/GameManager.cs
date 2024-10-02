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
    [SerializeField] AudioClip backgroundMusicClip;
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip loseClip;
    private AudioSource backgroundMusic;
    private AudioSource winSound;
    private AudioSource loseSound;

    private void Awake()
    {
        backgroundMusic = gameObject.AddComponent<AudioSource>();
        backgroundMusic.clip = backgroundMusicClip;
        backgroundMusic.loop = true;

        winSound = gameObject.AddComponent<AudioSource>();
        winSound.clip = winClip;

        loseSound = gameObject.AddComponent<AudioSource>();
        loseSound.clip = loseClip;
    }

    private void Start()
    {
        backgroundMusic.Play();
    }

    private void OnEnable()
    {
        OrderGenerator.OnGameWin += OnGameWin;
        OrderGenerator.OnGameLose += OnGameLose;
    }

    private void OnGameWin(object sender, System.EventArgs e)
    {
        WinPanel.SetActive(true);
        backgroundMusic.Stop();
        winSound.Play();
        Player.Instance.DisablePlayerMovement();
    }

    private void OnGameLose(object sender, System.EventArgs e)
    {
        LosePanel.SetActive(true);
        backgroundMusic.Stop();
        loseSound.Play();
        Player.Instance.DisablePlayerMovement();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}

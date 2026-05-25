using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectFiles.Scripts.Game_Manager
{
    public class PauseMenu : MonoBehaviour
    {
        public static PauseMenu instance;
        public static bool isPaused = false;
        
        [SerializeField] private GameObject pauseMenuUI;
        
        private void Awake()
        {
            instance = this;
            isPaused = false;
            pauseMenuUI.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
        }

        public void TogglePause()
        {
            if (isPaused) Resume();
            else Pause();
        }

        public void Pause()
        {
            isPaused = true;
            pauseMenuUI.SetActive(true);
        }

        public void Resume()
        {
            isPaused = false;
            pauseMenuUI.SetActive(false);
        }

        public void RestartLevel()
        {
            isPaused = false;
            pauseMenuUI.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void GoToSelector()
        {
            isPaused = false;
            pauseMenuUI.SetActive(false);
            SceneManager.LoadScene("LevelSelector");
        }

        public void MainMenu()
        {
            isPaused = false;
            pauseMenuUI.SetActive(false);
            SceneManager.LoadScene("MainMenu");
        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
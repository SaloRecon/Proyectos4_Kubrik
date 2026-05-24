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
        [SerializeField] private AudioLowPassFilter lowPassFilter;
        
        [Header("Low Pass Settings")]
        [SerializeField] private float normalCutoff = 22000;
        [SerializeField] private float pausedCutoff = 12000;
        [SerializeField] private float filterDuration = 0.4f;

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
            
            //efecto de low pass activado
            DOTween.To(() => lowPassFilter.cutoffFrequency,
                x => lowPassFilter.cutoffFrequency = x, 
                pausedCutoff, filterDuration).SetEase(Ease.OutQuad);
        }

        public void Resume()
        {
            isPaused = false;
            pauseMenuUI.SetActive(false);
            //efecto de low pass desactivado
            DOTween.To(() => lowPassFilter.cutoffFrequency,
                x => lowPassFilter.cutoffFrequency = x, 
                normalCutoff, filterDuration).SetEase(Ease.OutQuad)
                .OnComplete(() => { pauseMenuUI.SetActive(false); });
        }

        public void RestartLevel()
        {
            isPaused = false;
            pauseMenuUI.SetActive(false);
            DOTween.To(() => lowPassFilter.cutoffFrequency,
                x => lowPassFilter.cutoffFrequency = x,
                normalCutoff, filterDuration);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void GoToSelector()
        {
            isPaused = false;
            pauseMenuUI.SetActive(false);
            DOTween.To(() => lowPassFilter.cutoffFrequency,
                x => lowPassFilter.cutoffFrequency = x,
                normalCutoff, filterDuration);
            SceneManager.LoadScene("LevelSelector");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;
    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeDuration;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    //al ser un singleton que se mantiene vivo, no se puede depender de Start porque ese solo se activa una vez
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fadePanel.gameObject.SetActive(true);
        fadePanel.color = Color.black;
        fadePanel.DOFade(0f, fadeDuration).SetEase(Ease.OutQuad);
    }

    //transición y difuminado
    public void FadeOutAndLoad(string sceneToLoad)
    {
        fadePanel.gameObject.SetActive(true);
        fadePanel.DOFade(1f, fadeDuration).SetEase(Ease.InQuad)
            .OnComplete(() => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad));
    }
}

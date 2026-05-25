using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ProjectFiles.Scripts.Game_Manager;

public class TutorialScreen : MonoBehaviour
{
    [SerializeField] private Image tutorialScreen;
    void Start()
    {
        tutorialScreen.gameObject.SetActive(true); //solo por las dudas, para que antes del nivel tenga tutorial
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !PauseMenu.isPaused) FadeOut();
    }

    private void FadeOut()
    {
        tutorialScreen.DOFade(0, 1).SetEase(Ease.OutQuad)
            .OnComplete((() => tutorialScreen.gameObject.SetActive(false)));
    }
}

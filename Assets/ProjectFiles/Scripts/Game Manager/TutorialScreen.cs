using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ProjectFiles.Scripts.Game_Manager;

public class TutorialScreen : MonoBehaviour
{
    [SerializeField] private Image tutorialScreen;
    void Start()
    {
        PauseMenu.isPaused = true;
        tutorialScreen.gameObject.SetActive(true); //solo por las dudas, para que antes del nivel tenga tutorial
    }
    public void FadeOut()
    {
        PauseMenu.isPaused = false;
        tutorialScreen.DOFade(0, 1).SetEase(Ease.OutQuad)
            .OnComplete((() => tutorialScreen.gameObject.SetActive(false)));
    }
}

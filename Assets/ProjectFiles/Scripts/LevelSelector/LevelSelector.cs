using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{ 
    public string[] sceneNames;
    private string targetScene;
   public GameObject[] levels;
   private int currentLevelIndex = 0;
   private int currentTitleIndex = 0;
   private bool loading = false;
   
   [SerializeField] private GameObject[] titlesUI;

   private void Update()
   {
       if (loading) return;
       
       //hover
       Ray rayH = Camera.main.ScreenPointToRay(Input.mousePosition);
       if (Physics.Raycast(rayH, out RaycastHit hitH, 1000f))
       {
           GameObject current = levels[currentLevelIndex];
           bool hoveringCurrent = hitH.transform.IsChildOf(current.transform) || hitH.transform.gameObject == current;
           if (hoveringCurrent && !IsSelectAnimating()) HoverEnter(current);
           else HoverExit(current);
       }
       else HoverExit(levels[currentLevelIndex]);
       
       if (Input.GetMouseButtonDown(0))
       {
           //lanzo un raycast desde la posición del mouse para detectar los GO de los cubos
           RaycastHit hitC;
           Ray rayC = Camera.main.ScreenPointToRay(Input.mousePosition);
           if (Physics.Raycast(rayC, out hitC, 1000f))
           {
               //básicamente, si detecta un collider, carga la escena correspondiente a ese GO (bueno, el método Select)
               if (hitC.transform.IsChildOf(levels[currentLevelIndex].transform) ||
                   hitC.transform.gameObject == levels[currentLevelIndex])
               {
                   SelectLevel();
               }
           }
       }
   }

  public void NextLevel()
  {
      //si está cargando no hago nada
      if (loading) return;
      HoverExit(levels[currentLevelIndex]); //reseteo escala
      //activo y desactivo según el índice de GO
      levels[currentLevelIndex].SetActive(false);
      titlesUI[currentTitleIndex].SetActive(false);
      currentLevelIndex = (currentLevelIndex + 1) % levels.Length;
      currentTitleIndex = (currentTitleIndex + 1) % titlesUI.Length;
      levels[currentLevelIndex].SetActive(true);
      titlesUI[currentTitleIndex].SetActive(true);
      //DOTween de animación de bounce para UX
      BounceEffect();
      //esta línea para que los botones no permanezcan visualmente seleccionados
      EventSystem.current.SetSelectedGameObject(null);
  }

  public void PreviousLevel()
  {
      if (loading) return;
      HoverExit(levels[currentLevelIndex]); //reseteo escala
      levels[currentLevelIndex].SetActive(false);
      titlesUI[currentTitleIndex].SetActive(false);
      currentLevelIndex = (currentLevelIndex - 1 + levels.Length) % levels.Length;
      currentTitleIndex = (currentTitleIndex - 1 + titlesUI.Length) % titlesUI.Length;
      levels[currentLevelIndex].SetActive(true);
      titlesUI[currentTitleIndex].SetActive(true);
      BounceEffect();
      EventSystem.current.SetSelectedGameObject(null);
  }

  private void SelectLevel()
  {
      //al hacer click en el GO, reproduzco animación y cargo la escene correspondiente a ese GO (lambda)
      loading = true;
      isSelectAnimating = true;
      
      GameObject current = levels[currentLevelIndex];
      targetScene = sceneNames[currentLevelIndex];
      
      current.transform.DOKill();
      //paso 1, animación de achicar
      current.transform.DOScale(Vector3.one * 0.9f, 0.15f)
          .SetEase(Ease.InQuad)
          .OnComplete(() =>
          {
              SceneTransition.instance.FadeOutAndLoad(targetScene);
          });
  }

  private void BounceEffect()
  {
      isHovering = false;
      isSelectAnimating = false;
      //según el GO activado reproduzco una animación, pero antes detengo cualquiera para no solapar
      GameObject currentCube = levels[currentLevelIndex];
      currentCube.transform.DOKill();
      currentCube.transform.localScale = Vector3.one;
      currentCube.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 3, 0.8f);
  }
  

  private bool isHovering = false;
  private bool isSelectAnimating =  false;
  private bool IsSelectAnimating()=>isSelectAnimating;

  private void HoverEnter(GameObject cube)
  {
      if (isHovering || isSelectAnimating) return;
      isHovering = true;
      cube.transform.DOKill();
      cube.transform.DOScale(Vector3.one * 1.08f, 0.2f).SetEase(Ease.OutQuad);
  }

  private void HoverExit(GameObject cube)
  {
      if (!isHovering) return;
      isHovering = false;
      if (isSelectAnimating) return; //no interfiere con la animación de navegación
      cube.transform.DOKill();
      cube.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad);
  }
}

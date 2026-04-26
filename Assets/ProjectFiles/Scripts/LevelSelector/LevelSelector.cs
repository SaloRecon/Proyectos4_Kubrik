using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public string[] sceneNames;
   public GameObject[] levels;
   private int currentLevelIndex = 0;
   private bool loading = false;
   
  public void NextLevel()
  {
      if (loading) return;
      levels[currentLevelIndex].SetActive(false);
      currentLevelIndex = (currentLevelIndex + 1) % levels.Length;
      levels[currentLevelIndex].SetActive(true);
      BounceEffect();
  }

  public void PreviousLevel()
  {
      if (loading) return;
      levels[currentLevelIndex].SetActive(false);
      currentLevelIndex = (currentLevelIndex - 1 + levels.Length) % levels.Length;
      levels[currentLevelIndex].SetActive(true);
      BounceEffect();
  }
  
  public void StartLevel()
  {
      if (loading) return;
      loading = true;
      if (currentLevelIndex < sceneNames.Length)
          SceneManager.LoadScene(sceneNames[currentLevelIndex]);
  }

  private void BounceEffect()
  {
      GameObject currentCube = levels[currentLevelIndex];
      currentCube.transform.DOKill();
      currentCube.transform.localScale = Vector3.one;
      
      currentCube.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 3, 0.8f);
  }
  
}

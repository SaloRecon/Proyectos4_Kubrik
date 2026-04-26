using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public string[] sceneNames;
   public GameObject[] levels;
   private int currentLevelIndex = 0;
   private bool loading;
   
  public void NextLevel()
  {
      if (loading) return;
      levels[currentLevelIndex].SetActive(false);
      currentLevelIndex = (currentLevelIndex + 1) % levels.Length;
      levels[currentLevelIndex].SetActive(true);
  }
  
  public void PreviousLevel()
  {
      if (loading) return;
      levels[currentLevelIndex].SetActive(false);
      currentLevelIndex = (currentLevelIndex - 1 + levels.Length) % levels.Length;
      levels[currentLevelIndex].SetActive(true);
  }
  
  public void StartLevel()
  {
      if (loading) return;
      loading = true;
      if (currentLevelIndex < sceneNames.Length)
          SceneManager.LoadScene(sceneNames[currentLevelIndex]);
  }
   
}

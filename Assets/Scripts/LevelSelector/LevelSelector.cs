using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
   public GameObject[] levels;
   private int currentLevelIndex = 0;
   
  public void NextLevel()
  {
      levels[currentLevelIndex].SetActive(false);
      currentLevelIndex = (currentLevelIndex + 1) % levels.Length;
      levels[currentLevelIndex].SetActive(true);
  }
  
  public void PreviousLevel()
  {
      levels[currentLevelIndex].SetActive(false);
      currentLevelIndex = (currentLevelIndex - 1 + levels.Length) % levels.Length;
      levels[currentLevelIndex].SetActive(true);
  }
  
  public void StartLevel()
  {
     SceneManager.LoadScene(currentLevelIndex);
  }
   
}

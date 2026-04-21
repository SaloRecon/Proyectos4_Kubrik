using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSysem : MonoBehaviour
{
    
    public void play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Saliendo...");
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{

    [SerializeField] private AudioClip buttonSFX;
    
    public void play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SC_SFXManager.Instance.PlaySoundFXClip(buttonSFX, transform, 1f);
        
    }

    
    public void Quit()
    {
        SC_SFXManager.Instance.PlaySoundFXClip(buttonSFX, transform, 1f);
        Application.Quit();
        Debug.Log("Saliendo...");
    }
}

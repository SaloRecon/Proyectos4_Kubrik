using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    [Header("Audio Source")]
    public AudioSource musicSource;

    [Header("Menu Music")]
    public AudioClip menuMusic;

    [Header("Level Music")]
    public AudioClip cubo1x1Music;
    public AudioClip cubo2x2Music;
    public AudioClip cubo3x3Music;
    public AudioClip cubo4x4Music;

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;
    public float targetVolume = 1f;

    private string currentGroup = "";
    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        UpdateMusic(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusic(scene.name);
    }

    void UpdateMusic(string sceneName)
    {
        AudioClip newClip = null;
        string newGroup = "";

        //si el nombre de la escena corresponde a una de menú
        if (sceneName == "MainMenu" ||
            sceneName == "LevelSelector" ||
            sceneName == "TutorialScreen")
        {
            newClip = menuMusic;
            newGroup = "Menu";
        }

        //si el nombre de la escena corresponde a una de niveles
        else if (sceneName == "Cubo1x1")
        {
            newClip = cubo1x1Music;
            newGroup = "Cubo1x1";
        }
        else if (sceneName == "Cubo2x2")
        {
            newClip = cubo2x2Music;
            newGroup = "Cubo2x2";
        }
        else if (sceneName == "Cubo3x3")
        {
            newClip = cubo3x3Music;
            newGroup = "Cubo3x3";
        }
        else if (sceneName == "Cubo4x4")
        {
            newClip = cubo4x4Music;
            newGroup = "Cubo4x4";
        }

        // Si seguimos en el mismo grupo, no hacer nada
        if (currentGroup == newGroup)
            return;

        currentGroup = newGroup;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeMusic(newClip));
    }

    IEnumerator FadeMusic(AudioClip newClip)
    {
        // Fade Out
        while (musicSource.volume > 0)
        {
            musicSource.volume -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();

        // Cambiar canción
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade In
        while (musicSource.volume < targetVolume)
        {
            musicSource.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.volume = targetVolume;
    }
}
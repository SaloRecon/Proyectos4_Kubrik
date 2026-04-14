using UnityEngine.SceneManagement;

//public class GameManager : MonoBehaviour
//{
   // public static GameManager Instance;

   // private GameObject player;

   // private void Awake()
   // {
        // Singleton
        //if (Instance == null)
        //{
          //  Instance = this;
          //  DontDestroyOnLoad(gameObject);
       // }
        //{
          //  Destroy(gameObject);
        //}
    //}

    //private void Start()
    //{
        //player = GameObject.FindGameObjectWithTag("Player");
    //}

   // public void LoadNextScene(string sceneName)
    //{
       // SceneManager.sceneLoaded += OnSceneLoaded;
       // SceneManager.LoadScene(sceneName);
    //}

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
       // SceneManager.sceneLoaded -= OnSceneLoaded;

        // Buscar player si se pierde referencia
        //if (player == null)
           // player = GameObject.FindGameObjectWithTag("Player");

        // Buscar StartPlataform
        //GameObject start = GameObject.Find("StartPlataform");

       /// if (start != null && player != null)
        //{
         //   player.transform.position = start.transform.position;
       // }
      //  else
      //  {
            //Debug.LogWarning("No se encontró StartPlataform o Player");
        //}
    //}
//}

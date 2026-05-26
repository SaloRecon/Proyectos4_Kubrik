using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private int level;
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(level);
    }
}

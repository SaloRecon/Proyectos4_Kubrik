using System.Collections.Generic;
using UnityEngine;

public class CubeState : MonoBehaviour
{
    //lados
   
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    
    
    void Start()
    {
       
    }
    
    void Update()
    {
       

    }

    public void PickUp(List<GameObject> cubeSide)
    {
        foreach (GameObject face in cubeSide)
        {
            if (face != cubeSide[4])
            {
                face.transform.parent.transform.parent = cubeSide[4].transform.parent;
                //empieza la lógica de rotación
                
            }
        }
    }
    
}

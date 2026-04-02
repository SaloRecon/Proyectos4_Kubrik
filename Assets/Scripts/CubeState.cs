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

    // ReSharper disable Unity.PerformanceAnalysis
    public void PickUp(List<GameObject> cubeSide)
    {
        foreach (GameObject face in cubeSide)
        {
            //agrupa todas las piezas de esa cara a la pieza central, a menos que sea la misma
            if (face != cubeSide[4])
            {
                face.transform.parent.transform.parent = cubeSide[4].transform.parent;
            }
        }
        //empieza la lógica de rotación
        cubeSide[4].transform.parent.GetComponent<PivotRotation>().Rotate(cubeSide);
    }

    public void PutDown(List<GameObject> littleCubes, Transform pivot)
    {
        foreach (GameObject littleCube in littleCubes)
        {
            if (littleCube != littleCubes[4])
            {
                littleCube.transform.parent.transform.parent = pivot;
            }
        }
    }
    
}

using System.Collections.Generic;
using UnityEngine;

public class CubeState2x2 : MonoBehaviour
{
    //lados
   
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    
    [SerializeField] private GameObject pivot;
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
            if (face != pivot)
            {
                face.transform.parent.transform.parent = pivot.transform.parent;
            }
        }
        //empieza la lógica de rotación
        pivot.transform.parent.GetComponent<PivotRotation>().Rotate(cubeSide);
    }

    public void PutDown(List<GameObject> littleCubes, Transform pivot_)
    {
        foreach (GameObject littleCube in littleCubes)
        {
            if (littleCube != pivot)
            {
                littleCube.transform.parent.transform.parent = pivot_;
            }
        }
    }
    
}
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

    private int layerMask = 1 << 8; //mascara de capa solamente para las caras
    private CubeState cubeState;
    
    
    void Start()
    {
        cubeState = GetComponent<CubeState>();
    }
    
    void Update()
    {
        List<GameObject> facesHit = new List<GameObject>();
        Vector3 ray = tFront.transform.position;
        
    }
}

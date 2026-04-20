using System.Collections.Generic;
using UnityEngine;

public class SelectFace4x4 : MonoBehaviour
{
    [SerializeField] private CubeState4x4 cubeState4x4; 
    [SerializeField] private ReadCube4x4 readCube4x4;
    int layerMask = 1 << 8;
    
    void Start()
    {
        cubeState4x4 = GetComponent<CubeState4x4>();
        readCube4x4 = GetComponent<ReadCube4x4>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            readCube4x4.ReadState();
            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                GameObject face = hit.collider.gameObject;
                List<List<GameObject>> cubeSides = new List<List<GameObject>>()
                {
                    cubeState4x4.up,
                    cubeState4x4.down,
                    cubeState4x4.left,
                    cubeState4x4.right,
                    cubeState4x4.front,
                    cubeState4x4.back
                };

                foreach (List<GameObject> cubeSide in cubeSides)
                {
                    if (cubeSide.Contains(face))
                    {
                        cubeState4x4.PickUp(cubeSide);
                    }
                }
            }
        }
    }
}

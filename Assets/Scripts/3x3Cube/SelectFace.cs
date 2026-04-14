using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class SelectFace : MonoBehaviour
{
    [SerializeField] private CubeState cubeState;
    [SerializeField] private ReadCube readCube;
    int layerMask = 1 << 8;
    
    void Start()
    {
        cubeState = GetComponent<CubeState>();
        readCube = GetComponent<ReadCube>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //lee el estado actual del cubo
            readCube.ReadState();
            //raycast desde el mouse para ver si choca con una cara
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                GameObject face = hit.collider.gameObject;
                //hace una lista de todos los lados
                List<List<GameObject>> cubeSides = new List<List<GameObject>>()
                {
                    cubeState.up,
                    cubeState.down,
                    cubeState.left,
                    cubeState.right,
                    cubeState.front,
                    cubeState.back
                };
                //si existe la cara tocada
                foreach (List<GameObject> cubeSide in cubeSides)
                {
                    if (cubeSide.Contains(face))
                    {
                        //agarrar
                        cubeState.PickUp(cubeSide);
                    }
                }
            }
        }
    }
}

using System.Collections.Generic;
using ProjectFiles.Scripts.Cubes._2x2Cube;
using ProjectFiles.Scripts.Game_Manager;
using UnityEngine;

public class SelectFace2x2 : MonoBehaviour
{
    [SerializeField] private CubeState2x2 cubeState; 
    [SerializeField] private ReadCube2x2 readCube;
    int layerMask = 1 << 8;
    
    void Start()
    {
        cubeState = GetComponent<CubeState2x2>();
        readCube = GetComponent<ReadCube2x2>();
    }

    void Update()
    {
        //no detecto input hasta luego de auto shufflear
        if (Input.GetMouseButtonDown(0) && AutoShuffle2x2.is2x2ShuffleStarted 
                                        && !AutoShuffle2x2.is2x2ShuffleActive
                                        && !PauseMenu.isPaused)
        {
            readCube.ReadState();
            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                GameObject face = hit.collider.gameObject;
                List<List<GameObject>> cubeSides = new List<List<GameObject>>()
                {
                    cubeState.up,
                    cubeState.down,
                    cubeState.left,
                    cubeState.right,
                    cubeState.front,
                    cubeState.back
                };

                foreach (List<GameObject> cubeSide in cubeSides)
                {
                    if (cubeSide.Contains(face))
                    {
                        cubeState.PickUp(cubeSide);
                    }
                }
            }
        }
    }
}
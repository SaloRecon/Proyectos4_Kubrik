using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using Random = UnityEngine.Random;

namespace ProjectFiles.Scripts.Cubes._3x3Cube
{
    public class AutoShuffle : MonoBehaviour
    {
        public static List<string> moveList = new List<string>(){};
        private readonly List<string> allMoves = new List<string>()
        {
            "U", "D", "L", "R", "F", "B",
            "U2", "D2", "L2", "R2", "F2", "B2",
            "U'", "D'", "L'", "R'", "F'", "B'"
        };
        
        [SerializeField] private CubeState cubeState;
        [SerializeField] private ReadCube readCube;
        [SerializeField] private GameObject shuffleButton;
        
        public static bool started = false;
        public static bool shuffling = false;

        void Start()
        {
            cubeState = GetComponent<CubeState>();
            readCube = GetComponent<ReadCube>();
        }

        private void Update()
        {
            if (moveList.Count > 0 && !CubeState.autoRotating && CubeState.started)
            {
                //mover según el primer índice
                DoMove(moveList[0]);
                //remover el movimiento al primer índice
                moveList.Remove(moveList[0]);
            }
            else if (moveList.Count == 0 && shuffling)
            {
                shuffling = false;
            } 
        }

        public void Shuffle()
        {
            if (shuffling) return;
            shuffleButton.SetActive(false);
            started = true;
            
            List<string> moves = new List<string>();
            //la cantidad mínima y máxima de movimientos que queremos que haga automáticamente
            int shuffleLength = Random.Range(20, 31);
            //por cada unidad en la cantidad de movimientos aleatorios, elige uno dentro de los movimientos posibles
            //y lo agrega a la lista que se llama en el update
            //al pulsar el botón llamamos a este método, así que sucede antes del update
            //por lo que siempre habrá una lista definida
            for (int i = 0; i < shuffleLength; i++)
            {
                int randomMove = Random.Range(0, allMoves.Count);
                moves.Add(allMoves[randomMove]);
            }
            moveList = moves;
            shuffling = true;
        }
        void RotateSide(List<GameObject> side, float angle)
        {
            //rotar automáticamente el lado según ángulo
            PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();
            if (pr != null) pr.StartAutoRotate(side, angle);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        void DoMove(string move)
        {
            readCube.ReadState();
            CubeState.autoRotating = true;
            
            //Cada inicial es de la cara que se rota
            if (move == "U") RotateSide(cubeState.up, -90);
            //U' gira en sentido contrario al reloj
            if  (move == "U'") RotateSide(cubeState.up, 90);
            //U2 es una media vuelta, no es por la banda
            if (move == "U2") RotateSide(cubeState.up, -180);
            
            if (move == "D") RotateSide(cubeState.down, -90);
            if  (move == "D'") RotateSide(cubeState.down, 90);
            if (move == "D2") RotateSide(cubeState.down, -180);
            
            if (move == "L") RotateSide(cubeState.left, -90);
            if  (move == "L'") RotateSide(cubeState.left, 90);
            if (move == "L2") RotateSide(cubeState.left, -180);
            
            if (move == "R") RotateSide(cubeState.right, -90);
            if  (move == "R'") RotateSide(cubeState.right, 90);
            if (move == "R2") RotateSide(cubeState.right, -180);
            
            if (move == "F") RotateSide(cubeState.front, -90);
            if  (move == "F'") RotateSide(cubeState.front, 90);
            if (move == "F2") RotateSide(cubeState.front, -180);
            
            if (move == "B") RotateSide(cubeState.back, -90);
            if  (move == "B'") RotateSide(cubeState.back, 90);
            if (move == "B2") RotateSide(cubeState.back, -180);
        }
    }
}
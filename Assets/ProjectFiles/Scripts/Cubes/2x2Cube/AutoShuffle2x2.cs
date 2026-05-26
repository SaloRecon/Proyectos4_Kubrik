using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;

namespace ProjectFiles.Scripts.Cubes._2x2Cube
{
    public class AutoShuffle2x2 : MonoBehaviour
    {
        [SerializeField] private GameObject playerGO;
        public static bool is2x2ShuffleActive = false; //para cuando se está auto shuffleando
        public static bool is2x2ShuffleStarted = false; //para bloquear el input antes de presionar el botón

        [SerializeField] private GameObject shuffleButton;
        
        public static List<string> moveList = new List<string>(){};
        private readonly List<string> allMoves = new List<string>()
        {
            "U", "D", "L", "R", "F", "B",
            "U2", "D2", "L2", "R2", "F2", "B2",
            "U'", "D'", "L'", "R'", "F'", "B'"
        };
        
        [SerializeField] private CubeState2x2 cubeState2x2;
        [SerializeField] private ReadCube2x2 readCube2x2;

        void Start()
        {
            playerGO.SetActive(false);
            cubeState2x2 = GetComponent<CubeState2x2>();
            readCube2x2 = GetComponent<ReadCube2x2>();
        }

        private void Update()
        {
            if (moveList.Count > 0 && !PivotRotation2x2.is2x2Shuffling)
            {
                //mover según el primer índice
                DoMove(moveList[0]);
                //remover el movimiento al primer índice
                moveList.Remove(moveList[0]);
            }
            else if (moveList.Count == 0 && is2x2ShuffleActive)
            {
                playerGO.SetActive(true);
                is2x2ShuffleActive = false;
            }
        }

        public void Shuffle()
        {
            shuffleButton.SetActive(false);
            is2x2ShuffleStarted = true;
            List<string> moves = new List<string>();
            //la cantidad mínima y máxima de movimientos que queremos que haga automáticamente
            int shuffleLength = Random.Range(10, 21);
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
            is2x2ShuffleActive = true;
        }
        void RotateSide(List<GameObject> side, float angle)
        {
            //rotar automáticamente el lado según ángulo
            GameObject pivot = GetPivotForSide(side);
            if (pivot != null) 
                pivot.GetComponent<PivotRotation2x2>().StartAutoShuffle(side, angle);
        }

        //obtengo el pivote de cada cara
        private GameObject GetPivotForSide(List<GameObject> side)
        {
            if (side == cubeState2x2.up) return cubeState2x2.pivots[0];
            if (side == cubeState2x2.down) return cubeState2x2.pivots[1];
            if (side == cubeState2x2.left) return cubeState2x2.pivots[2];
            if (side == cubeState2x2.right) return cubeState2x2.pivots[3];
            if (side == cubeState2x2.front) return cubeState2x2.pivots[4];
            if (side == cubeState2x2.back) return cubeState2x2.pivots[5];
            return null;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        void DoMove(string move)
        {
            readCube2x2.ReadState();
            PivotRotation2x2.is2x2Shuffling = true;
            
            //Cada inicial es de la cara que se rota
            if (move == "U") RotateSide(cubeState2x2.up, -90);
            //U' gira en sentido contrario al reloj
            if  (move == "U'") RotateSide(cubeState2x2.up, 90);
            //U2 es una media vuelta, no es por la banda
            if (move == "U2") RotateSide(cubeState2x2.up, -180);
            
            if (move == "D") RotateSide(cubeState2x2.down, -90);
            if  (move == "D'") RotateSide(cubeState2x2.down, 90);
            if (move == "D2") RotateSide(cubeState2x2.down, -180);
            
            if (move == "L") RotateSide(cubeState2x2.left, -90);
            if  (move == "L'") RotateSide(cubeState2x2.left, 90);
            if (move == "L2") RotateSide(cubeState2x2.left, -180);
            
            if (move == "R") RotateSide(cubeState2x2.left, -90);
            if  (move == "R'") RotateSide(cubeState2x2.left, 90);
            if (move == "R2") RotateSide(cubeState2x2.left, -180);
            
            if (move == "F") RotateSide(cubeState2x2.front, -90);
            if  (move == "F'") RotateSide(cubeState2x2.front, 90);
            if (move == "F2") RotateSide(cubeState2x2.front, -180);
            
            if (move == "B") RotateSide(cubeState2x2.back, -90);
            if  (move == "B'") RotateSide(cubeState2x2.back, 90);
            if (move == "B2") RotateSide(cubeState2x2.back, -180);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectFiles.Scripts.Cubes._4x4Cube
{
    public class AutoShuffle4x4 : MonoBehaviour
    {
        public static bool shuffling = false; //para cuando se está auto shuffleando
        public static bool started = false; //para bloquear el input antes de presionar el botón

        [SerializeField] private GameObject shuffleButton;
        
        public static List<string> moveList = new List<string>(){};
        private readonly List<string> allMoves = new List<string>()
        {
            "U", "D", "L", "R", "F", "B",
            "U'", "D'", "L'", "R'", "F'", "B'",
            "U2", "D2", "L2", "R2", "F2", "B2",
            "U1", "U2i", "L1", "L2i", "F1", "F2i",
            "U1'", "U2i'", "L1'", "L2i'", "F1'", "F2i'" //i es de cara interna
        };
        
        [SerializeField] private CubeState4x4 cubeState4x4;
        [SerializeField] private ReadCube4x4 readCube4x4;

        void Start()
        {
            cubeState4x4 = GetComponent<CubeState4x4>();
            readCube4x4 = GetComponent<ReadCube4x4>();
        }

        /*private void Update()
        {
            if (moveList.Count > 0 && !PivotRotation4x4.isShuffling)
            {
                PivotRotation4x4.isShuffling = true;
                //mover según el primer índice
                DoMove(moveList[0]);
                //remover el movimiento al primer índice
                moveList.Remove(moveList[0]);
            }
            else if (moveList.Count == 0 && shuffling)
            {
                shuffling = false;
            }
        }*/

        public void Shuffle()
        {
            shuffleButton.SetActive(false);
            started = true;
            List<string> moves = new List<string>();
            //la cantidad mínima y máxima de movimientos que queremos que haga automáticamente
            int shuffleLength = Random.Range(20, 36);
            //por cada unidad en la cantidad de movimientos aleatorios, elige uno dentro de los movimientos posibles
            //y lo agrega a la lista que se llama en el update
            //al pulsar el botón llamamos a este método, así que sucede antes del update
            //por lo que siempre habrá una lista definida
            for (int i = 0; i < shuffleLength; i++)
                moves.Add(allMoves[Random.Range(0, allMoves.Count)]);
            moveList = moves;
            shuffling = true;
            StartCoroutine(RunShuffle());
        }
        
        private IEnumerator RunShuffle()
        {
            foreach (string move in moveList.ToList())
            {
                DoMove(move);
                yield return new WaitForSeconds(0.5f);
            }
            moveList.Clear();
            shuffling = false;
        }
        void RotateSide(List<GameObject> side, float angle)
        {
            //rotar automáticamente el lado según ángulo
            GameObject pivot = GetPivotForSide(side);
            if (pivot != null) 
                pivot.GetComponent<PivotRotation4x4>().StartAutoShuffle(side, angle);
        }

        //obtengo el pivote de cada cara
        private GameObject GetPivotForSide(List<GameObject> side)
        {
            if (side == cubeState4x4.up) return cubeState4x4.pivots[0];
            if (side == cubeState4x4.down) return cubeState4x4.pivots[1];
            if (side == cubeState4x4.left) return cubeState4x4.pivots[2];
            if (side == cubeState4x4.right) return cubeState4x4.pivots[3];
            if (side == cubeState4x4.front) return cubeState4x4.pivots[4];
            if (side == cubeState4x4.back) return cubeState4x4.pivots[5];
            if (side == cubeState4x4.up1) return cubeState4x4.pivots[6];
            if (side == cubeState4x4.up2) return cubeState4x4.pivots[7];
            if (side == cubeState4x4.left1) return cubeState4x4.pivots[8];
            if (side == cubeState4x4.left2) return cubeState4x4.pivots[9];
            if (side == cubeState4x4.front1) return cubeState4x4.pivots[10];
            if (side == cubeState4x4.front2) return cubeState4x4.pivots[11];
            return null;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        void DoMove(string move)
        {
            readCube4x4.ReadState();
            
            //Cada inicial es de la cara que se rota
            if (move == "U") RotateSide(cubeState4x4.up, -90);
            //U' gira en sentido contrario al reloj
            if  (move == "U'") RotateSide(cubeState4x4.up, 90);
            //U2 es una media vuelta, no es por la banda
            if (move == "U2") RotateSide(cubeState4x4.up, -180);
            
            if (move == "D") RotateSide(cubeState4x4.down, -90);
            if  (move == "D'") RotateSide(cubeState4x4.down, 90);
            if (move == "D2") RotateSide(cubeState4x4.down, -180);
            
            if (move == "L") RotateSide(cubeState4x4.left, -90);
            if  (move == "L'") RotateSide(cubeState4x4.left, 90);
            if (move == "L2") RotateSide(cubeState4x4.left, -180);
            
            if (move == "R") RotateSide(cubeState4x4.right, -90);
            if  (move == "R'") RotateSide(cubeState4x4.right, 90);
            if (move == "R2") RotateSide(cubeState4x4.right, -180);
            
            if (move == "F") RotateSide(cubeState4x4.front, -90);
            if  (move == "F'") RotateSide(cubeState4x4.front, 90);
            if (move == "F2") RotateSide(cubeState4x4.front, -180);
            
            if (move == "B") RotateSide(cubeState4x4.back, -90);
            if  (move == "B'") RotateSide(cubeState4x4.back, 90);
            if (move == "B2") RotateSide(cubeState4x4.back, -180);
            
            //caras interiores
            if (move == "U1") RotateSide(cubeState4x4.up1, -90);
            if  (move == "U1'") RotateSide(cubeState4x4.up1, 90);
            if (move == "U2i") RotateSide(cubeState4x4.up2, -90);
            if (move == "U2i'") RotateSide(cubeState4x4.up2, 90);
            if  (move == "L1") RotateSide(cubeState4x4.left1, -90);
            if (move == "L1'") RotateSide(cubeState4x4.left1, 90);
            if (move == "L2i") RotateSide(cubeState4x4.left2, -90);
            if  (move == "L2i'") RotateSide(cubeState4x4.left2, 90);
            if (move == "F1") RotateSide(cubeState4x4.front1, -90);
            if (move == "F1'") RotateSide(cubeState4x4.front1, 90);
            if  (move == "F2i") RotateSide(cubeState4x4.front2, -90);
            if (move == "F2i'") RotateSide(cubeState4x4.front2, 90);
        }
    }
}
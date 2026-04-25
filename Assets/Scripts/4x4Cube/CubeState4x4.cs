using System.Collections.Generic;
using UnityEngine;

public class CubeState4x4 : MonoBehaviour
{
    public List<GameObject> front, back, left, right, up, down;
    public List<GameObject> front1, front2, up1, up2, left1, left2;
    public GameObject[] pivots; 

    public void PickUp(List<GameObject> cubeSide) 
    {
        GameObject currentPivot = GetPivot(cubeSide);
        foreach (GameObject face in cubeSide) 
        {
            //no hay pieza central, se emparenta al pivote
            face.transform.parent.transform.parent = currentPivot.transform;
        }
        currentPivot.GetComponent<PivotRotation4x4>().Rotate(cubeSide);
    }

    public void PutDown(List<GameObject> littleCubes, Transform pivot) 
    {
        Transform piecesParent = this.transform.Find("Pieces");
        foreach (GameObject littleCube in littleCubes) 
        {
            littleCube.transform.parent.transform.parent = piecesParent; //vuelven al cubo principal
        }
    }

    private GameObject GetPivot(List<GameObject> side) 
    {
        //capas exteriores
        if (side == up) return pivots[0];
        if (side == down) return pivots[1];
        if (side == left) return pivots[2];
        if (side == right) return pivots[3];
        if (side == front) return pivots[4];
        if (side == back) return pivots[5];
        //capas interiores
        if (side == up1) return pivots[6];
        if (side == up2) return pivots[7];
        if (side == left1) return pivots[8];
        if (side == left2) return pivots[9];
        if (side == front1) return pivots[10];
        if (side == front2) return pivots[11];

        return null;
    }
}

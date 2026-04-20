using System.Collections.Generic;
using UnityEngine;

public class CubeState4x4 : MonoBehaviour
{
    public List<GameObject> front, back, left, right, up, down;
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
        foreach (GameObject littleCube in littleCubes) 
        {
            littleCube.transform.parent.transform.parent = this.transform; //vuelven al cubo principal
        }
    }

    private GameObject GetPivot(List<GameObject> side) 
    {
        if (side == up) return pivots[0];
        if (side == down) return pivots[1];
        if (side == left) return pivots[2];
        if (side == right) return pivots[3];
        if (side == front) return pivots[4];
        return pivots[5];
    }
}

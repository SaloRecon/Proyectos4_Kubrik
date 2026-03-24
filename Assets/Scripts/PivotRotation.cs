using System.Collections.Generic;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    private List<GameObject> activeSide;
    private Vector3 localForward;
    private Vector3 mouseRef;
    private bool dragging = false;
    [SerializeField] private float sensitivity;
    private Vector3 rotation; 

    [SerializeField] private ReadCube readCube;
    [SerializeField] private CubeState cubeState;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            SpinSide(activeSide);
            if (Input.GetMouseButton(0))
            {
                dragging = false;
            }
        }
    }

    private void SpinSide(List<GameObject> side)
    {
        //reinicia la rotación
        rotation = Vector3.zero;
        
        //actual posición del mouse menos la última
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);

        if (side == cubeState.front)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        //rota
        transform.Rotate(rotation, Space.Self);
        mouseRef = Input.mousePosition;
    }
    
    public void Rotate(List<GameObject> side)
    {
        activeSide = side;
        mouseRef = Input.mousePosition;
        dragging = true;
        //crea un vector sobre el cual rotar
        localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
    }
    
}

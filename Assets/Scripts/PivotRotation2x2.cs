using System.Collections.Generic;
using UnityEngine;

public class PivotRotation2x2 : MonoBehaviour
{
    private List<GameObject> activeSide;
    private Vector3 localForward;
    private Vector3 mouseRef;
    private bool dragging = false;
    private bool autoRotating = false;
    private float speed = 300f;
    private float sensitivity = 0.2f;
    private Vector3 rotation; 
    
    [SerializeField] private GameObject pivot;
    
    private Quaternion targetQuaternion;

    [SerializeField] private ReadCube readCube;
    [SerializeField] private CubeState cubeState;
    void Start()
    {
        readCube =  FindObjectOfType<ReadCube>();
        cubeState =  FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            SpinSide(activeSide);
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                RotateToRightAngle();
            }
        }

        if (autoRotating)
        {
            AutoRotate();
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
        if (side == cubeState.back)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.up)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.down)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.left)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.right)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
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
        localForward = Vector3.zero - pivot.transform.parent.transform.localPosition;
    }

    public void RotateToRightAngle()
    {
        Vector3 vec = transform.localEulerAngles;
        //redondea a los 90 grados más cercanos
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;
        
        targetQuaternion.eulerAngles = vec;
        autoRotating = true;
    }

    private void AutoRotate()
    {
        dragging = false;
        var step = speed *  Time.deltaTime;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);
        
        //cancela la rotación a menos de un grado
        if (Quaternion.Angle(transform.localRotation, targetQuaternion) <= 1)
        {
            transform.localRotation = targetQuaternion;
            cubeState.PutDown(activeSide, transform.parent);
            readCube.ReadState();
            autoRotating = false;
        }
    }
    
}

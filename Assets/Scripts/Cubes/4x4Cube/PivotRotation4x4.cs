using System.Collections.Generic;
using UnityEngine;

public class PivotRotation4x4 : MonoBehaviour
{
    private List<GameObject> activeSide;
    private Vector3 localForward;
    private Vector3 mouseRef;
    private bool dragging = false;
    private bool autoRotating = false;
    private float speed = 300f;
    private float sensitivity = 0.2f;
    private Vector3 rotation; 
        
    private Quaternion targetQuaternion;

    [SerializeField] private ReadCube4x4 readCube4x4;
    [SerializeField] private CubeState4x4 cubeState4x4;
   
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

        if (side == cubeState4x4.front || side == cubeState4x4.front1 || side == cubeState4x4.front2)
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        if (side == cubeState4x4.back)
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        if (side == cubeState4x4.up || side == cubeState4x4.up1 || side == cubeState4x4.up2 )
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        if (side == cubeState4x4.down)
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        if (side == cubeState4x4.left || side == cubeState4x4.left1 || side == cubeState4x4.left2)
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        if (side == cubeState4x4.right)
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        
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
        localForward = transform.forward;
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
            cubeState4x4.PutDown(activeSide, transform.parent);
            readCube4x4.ReadState();
            autoRotating = false;
        }
    }
}

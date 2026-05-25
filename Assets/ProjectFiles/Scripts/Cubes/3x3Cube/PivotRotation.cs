using System.Collections.Generic;
using UnityEngine;

public class PivotRotation : MonoBehaviour
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

    [SerializeField] private ReadCube readCube;
    [SerializeField] private CubeState cubeState;
    [SerializeField] private AudioClip snapSFX;

    void Start()
    {
        //readCube =  FindObjectOfType<ReadCube>();
        //cubeState =  FindObjectOfType<CubeState>();
    }

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
            AutoRotate();
    }

    private void SpinSide(List<GameObject> side)
    {
        //reinicia la rotación
        rotation = Vector3.zero;
        
        //actual posición del mouse menos la última
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);

        if (side == cubeState.front)
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        if (side == cubeState.back)
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        if (side == cubeState.up)
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        if (side == cubeState.down)
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        if (side == cubeState.left)
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        if (side == cubeState.right)
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        //rota
        transform.Rotate(rotation, Space.Self);
        mouseRef = Input.mousePosition;
    }

    public void StartAutoRotate(List<GameObject> side, float angle)
    {
        cubeState.PickUp(side);
        //devuelve el eje para la rotación de todas las caras y es el que se usa en la función de arriba
        Vector3 axis = Vector3.zero;
        if (side == cubeState.front ||  side == cubeState.back) axis = Vector3.right;
        if (side == cubeState.up ||  side == cubeState.down) axis = Vector3.up;
        if (side == cubeState.left ||  side == cubeState.right) axis = Vector3.forward;
        if (side == cubeState.back || side == cubeState.down || side == cubeState.right) angle *= -1f;
        targetQuaternion = Quaternion.AngleAxis(angle, axis) * transform.localRotation;
        activeSide = side;
        autoRotating = true;
    }
    public void Rotate(List<GameObject> side)
    {
        activeSide = side;
        mouseRef = Input.mousePosition;
        dragging = true;
        //crea un vector sobre el cual rotar
        localForward = Vector3.zero - transform.localPosition;
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
            CubeState.is3x3AutoRotating = false;
            autoRotating = false;
            SC_SFXManager.Instance.PlaySoundFXClip(snapSFX, transform, 0.1f);
        }
    }
    
}

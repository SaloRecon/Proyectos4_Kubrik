using System.Collections.Generic;
using UnityEngine;

public class ReadCube : MonoBehaviour
{
    public Transform tUp;
    public Transform tDown;
    public Transform tFront;
    public Transform tLeft;
    public Transform tRight;
    public Transform tBack;
    
    private List<GameObject> frontRays = new List<GameObject>();
    private List<GameObject> backRays = new List<GameObject>();
    private List<GameObject> leftRays = new List<GameObject>();
    private List<GameObject> rightRays = new List<GameObject>();
    private List<GameObject> downRays = new List<GameObject>();
    private List<GameObject> upRays = new List<GameObject>();
    
    private int layerMask = 1 << 8; //mascara de capa solamente para las caras
    [SerializeField] private CubeState cubeState;
    public GameObject emptyGO;
    
    void Start()
    {
        SetRayTransforms();
    }
    
    void Update()
    {
       
    }

    public void ReadState()
    {
        cubeState = FindObjectOfType<CubeState>();
        //settea el estado de cada posición a la lista de lados
        cubeState.up = ReadFace(upRays, tUp);
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.right = ReadFace(rightRays, tRight);
    }
    
    private void SetRayTransforms()
    {
        upRays = BuildRays(tUp, new Vector3(90, 90, 0));
        downRays = BuildRays(tDown, new Vector3(270, 90, 0));
        leftRays = BuildRays(tLeft, new Vector3(0, 180, 0));
        rightRays = BuildRays(tRight, new Vector3(0, 0, 0));
        frontRays = BuildRays(tFront, new Vector3(0, 90, 0));
        backRays = BuildRays(tBack, new Vector3(0, 270, 0));
    }
    
   List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        //la rayCount se usa para nombrar los raycasts para asegurar el orden
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();
        // esto se ve así:
        //|0|1|2|
        //|3|4|5|
        //|6|7|8|

        for (int y = 1; y > -2; y--)
        {
            for (int x = -1; x < 2; x++)
            {
                Vector3 startPos = new Vector3(rayTransform.localPosition.x + x,
                    rayTransform.localPosition.y + y,
                    rayTransform.localPosition.z);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
            }
        }
        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays;
    }

    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        List<GameObject> facesHit = new List<GameObject>();

        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;
            RaycastHit hit;
        
            //el raycast toca algún objeto en la layermask?
            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.red);
                facesHit.Add(hit.collider.gameObject);
            }
            else
            {
                Debug.DrawRay(ray, rayTransform.forward * 3, Color.green);
            }    
        }
        return facesHit;
    }

}

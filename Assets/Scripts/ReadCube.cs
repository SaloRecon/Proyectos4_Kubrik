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
    
    private int layerMask = 1 << 8; //mascara de capa solamente para las caras
    private CubeState cubeState;
    
    void Start()
    {
        cubeState =  GetComponent<CubeState>();
        
        List<GameObject> facesHit = new List<GameObject>();
        Vector3 ray = tFront.transform.position;
        RaycastHit hit;
        
        //el raycast toca algún objeto en la layermask?
        if (Physics.Raycast(ray, tFront.right, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(ray, tFront.right * hit.distance, Color.red);
            facesHit.Add(hit.collider.gameObject);
        }
        else
        {
            Debug.DrawRay(ray, tFront.right * 1000, Color.green);
        }
        
        cubeState.front = facesHit;
    }
    
    void Update()
    {
       
    }

   /* List<GameObject> BuildRays(Transform rayTransform, Vector3 rayDirection)
    {
        //la rayCount se usa para nombrar los raycasts para asegurar el orden
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();
        // esto se ve así:
        //|0||1||2|
        //|3||4||4|
        //|6||7||8|

        for (int y = 1; y > -2; y--)
        {
            for (int x = -1; x < 2; x++)
            {
                Vector3 startPos = new Vector3(rayTransform.localPosition.x + x,
                    rayTransform.localPosition.y + y,
                    rayTransform.localPosition.z);
               // GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform)
            }
        }
        
    }*/
    
}

using System.Collections.Generic;
using UnityEngine;

public class SelectFace4x4 : MonoBehaviour
{
    [SerializeField] private CubeState4x4 cubeState4x4; 
    [SerializeField] private ReadCube4x4 readCube4x4;
    int layerMask = 1 << 8;
    private bool dragWait = false;
    private Vector3 mouseDownPos;
    private GameObject hitFace;
    private List<GameObject> outerFaceClicked;
    private Vector3 hitLocalPos;
    private string faceAxis;
    private float dragThreshold = 5;
    
    void Start()
    {
        cubeState4x4 = GetComponent<CubeState4x4>();
        readCube4x4 = GetComponent<ReadCube4x4>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            readCube4x4.ReadState();
            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                hitFace = hit.collider.gameObject;
                outerFaceClicked = GetOuterFace(hitFace);

                if (outerFaceClicked != null)
                {
                    //convierte el hitpoint a coordenada local
                    hitLocalPos = transform.InverseTransformPoint(hit.point);
                    faceAxis = GetFaceAxis(outerFaceClicked);
                    mouseDownPos = Input.mousePosition;
                    dragWait = true;
                }
                
            }
        }

        if (dragWait && Input.GetMouseButton(0))
        {
            Vector3 dragDelta = Input.mousePosition - mouseDownPos;

            if (dragDelta.magnitude >= dragThreshold)
            {
                dragWait = false;
                bool dragHorizontal = Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y);
                List<GameObject> sliceToRotate = PickSlice(outerFaceClicked, faceAxis, hitLocalPos, dragHorizontal);

                if (sliceToRotate != null)
                {
                    cubeState4x4.PickUp(sliceToRotate);
                }
            }
        }
        
        if (Input.GetMouseButtonUp(0)) dragWait = false;
    }

    private List<GameObject> GetOuterFace(GameObject face)
    {
        List<List<GameObject>> outerFaces = new List<List<GameObject>>()
        {
            cubeState4x4.up,
            cubeState4x4.down,
            cubeState4x4.left,
            cubeState4x4.right,
            cubeState4x4.front,
            cubeState4x4.back
        };
        foreach (var side in outerFaces)
            if (side.Contains(face)) return side;
        return null;
    }

    //devuelve el eje perpendicular a la cara
    private string GetFaceAxis(List<GameObject> face)
    {
        if (face == cubeState4x4.up || face == cubeState4x4.down) return "y";
        if (face == cubeState4x4.left || face == cubeState4x4.right) return "z";
        if (face == cubeState4x4.front || face == cubeState4x4.back) return "x";
        return "y";
    }

    private List<GameObject> PickSlice(List<GameObject> outerFace, string axis, Vector3 localHit, bool dragHorizontal)
    {
        //lee dos coordenadas de localHit para determinar qué slice girar según la dirección de drag

        if (axis == "y")
        {
            float coord = dragHorizontal ? localHit.z : localHit.y;
            string sliceAxis = dragHorizontal ? "z" : "x";
            return GetSliceByCoord(sliceAxis, coord, outerFace);
        }
        else if (axis == "z")
        {
            float coord = dragHorizontal ? localHit.x : localHit.y;
            string sliceAxis = dragHorizontal ? "x" : "y";
            return GetSliceByCoord(sliceAxis, coord, outerFace);
        }
        else
        {
            float coord = dragHorizontal ? localHit.z : localHit.y;
            string sliceAxis = dragHorizontal ? "z" : "y";
            return GetSliceByCoord(sliceAxis, coord, outerFace);
        }
    }
    
    //dado un eje y una coordenada de hit, devuelve la lista de slice correspondiente
    private List<GameObject> GetSliceByCoord(string sliceAxis, float coord, List<GameObject> outerFace)
    {
        float rounded = Mathf.Round(coord * 2f) / 2f;

        if (sliceAxis == "y")
        {
            if (Mathf.Approximately(rounded,  1.5f) || Mathf.Approximately(rounded, -1.5f))
                return outerFace == cubeState4x4.up ? cubeState4x4.up : cubeState4x4.down;
            if (Mathf.Approximately(rounded,  0.5f)) return cubeState4x4.up1;
            if (Mathf.Approximately(rounded, -0.5f)) return cubeState4x4.up2;
            if (Mathf.Approximately(rounded,  1.5f)) return cubeState4x4.up;
            if (Mathf.Approximately(rounded, -1.5f)) return cubeState4x4.down;
        }
        else if (sliceAxis == "z")
        {
            if (Mathf.Approximately(rounded,  1.5f)) return cubeState4x4.left;
            if (Mathf.Approximately(rounded,  0.5f)) return cubeState4x4.left1;
            if (Mathf.Approximately(rounded, -0.5f)) return cubeState4x4.left2;
            if (Mathf.Approximately(rounded, -1.5f)) return cubeState4x4.right;
        }
        else if (sliceAxis == "x")
        {
            if (Mathf.Approximately(rounded, -1.5f)) return cubeState4x4.front;
            if (Mathf.Approximately(rounded, -0.5f)) return cubeState4x4.front1;
            if (Mathf.Approximately(rounded,  0.5f)) return cubeState4x4.front2;
            if (Mathf.Approximately(rounded,  1.5f)) return cubeState4x4.back;
        }

        return outerFace;
    }
}

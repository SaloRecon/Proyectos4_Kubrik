using DG.Tweening;
using JetBrains.Annotations;
using ProjectFiles.Scripts.Game_Manager;
using UnityEngine;

public class RotateBigCube : MonoBehaviour
{
    
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    Vector3 previousMousePos;
    Vector3 mouseDelta;

    public GameObject target;

    private bool isSnapping;

    [SerializeField] private float snapDuration;
    [SerializeField] private float dragSensitivity;

    void Update()
    {
        Swipe();
        Drag();
    }

    void Drag()
    {
        if (Input.GetMouseButton(1))
        {
            //mientras se mantiene el mouse el cubo se puede mover en su eje
            isSnapping = false;
            transform.DOKill();
            mouseDelta = Input.mousePosition - previousMousePos;
            mouseDelta *= dragSensitivity;
            transform.rotation = Quaternion.Euler(mouseDelta.y, -mouseDelta.x, 0) * transform.rotation;
        }

        //automáticamente mover a la posición objetivo
        else
        {
            if (transform.rotation != target.transform.rotation)
            {
                isSnapping  = true;
                transform.DOKill();
                transform.DORotateQuaternion(target.transform.rotation, snapDuration).SetEase(Ease.OutCubic).OnComplete((() => isSnapping = false));
            }
        }
        previousMousePos = Input.mousePosition;
    }
    
    void Swipe()
    {
        if (Input.GetMouseButtonDown(1) && !PauseMenu.isPaused)
        {
            //recoge la posición 2D del primer click
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.DOKill();
        }
        if (Input.GetMouseButtonUp(1) && !PauseMenu.isPaused)
        {
            //obtiene la posición 2D del segundo click
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //crea un segundo vector de las posiciones del primer y segundo click
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            //normalizar el vector
            currentSwipe.Normalize();

            if (LeftSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 90, 0, Space.World);
            }
            else if (RightSwipe(currentSwipe))
            {
                target.transform.Rotate(0, -90, 0, Space.World);
            }
            else if (UpLeftSwipe(currentSwipe))
            {
                target.transform.Rotate(90, 0, 0, Space.World);
            }
            else if (UpRightSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 0, -90, Space.World);
            }
            else if (DownLeftSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 0, 90, Space.World);
            }
            else if (DownRightSwipe(currentSwipe))
            {
                target.transform.Rotate(-90, 0, 0, Space.World);
            }
        }

    }

    bool LeftSwipe(Vector2 swipe)
    {	
        return swipe.x < -0.5f && swipe.y > -0.5f && swipe.y < 0.5f;
    }

    bool RightSwipe(Vector2 swipe)
    { 
        return swipe.x > 0.5f && swipe.y > -0.5f && swipe.y < 0.5f;
    }

    bool UpLeftSwipe(Vector2 swipe)
    {
        return swipe.y > 0.5f && swipe.x < 0f;
    }
    
    bool UpRightSwipe(Vector2 swipe)
    {
        return swipe.y > 0.5f && swipe.x > 0f;
    }

    bool DownLeftSwipe(Vector2 swipe)
    {
        return swipe.y < -0.5f && swipe.x < 0f;
    }

    bool DownRightSwipe(Vector2 swipe)
    {
        return swipe.y < -0.5f && swipe.x > 0f;
    }
    
    //si reinicio o transiciono mato todas las animaciones
    private void OnDestroy()
    {
        transform.DOKill();
    }
    
}
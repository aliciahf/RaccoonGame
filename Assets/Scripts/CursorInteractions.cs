using UnityEngine;

public class CursorInteractions : MonoBehaviour
{
    void OnMouseEnter() 
    { 
        CursorController.Instance.SetHover(); 
    }

    void OnMouseExit() 
    { 
        CursorController.Instance.SetDefault(); 
    }

    void onMouseDown()
    {
        CursorController.Instance.SetGrabbing(); 
    }
}

using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController Instance;
    [Header("Cursor Textures")] 
    public Texture2D defaultCursor; 
    public Texture2D hoverCursor; 
    public Texture2D grabbingCursor;

    [Header("Cursor Hotspot/Location")]
    public Vector2 hotspot = Vector2.zero;
    
    void Start()
    {
        SetDefault();
    }

    public void SetDefault()
    {
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
    }

    public void SetHover()
    {
        Cursor.SetCursor(hoverCursor, hotspot, CursorMode.Auto);
    }

    public void SetGrabbing()
    {
        Cursor.SetCursor(grabbingCursor, hotspot, CursorMode.Auto);
    }
    
    void Awake()
    {
        Instance = this;
    }
}

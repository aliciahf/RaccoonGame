using UnityEngine;

public class CursorController : MonoBehaviour
{
    // public static CursorController Instance;
    // [Header("Cursor Textures")] 
    // public Texture2D defaultCursor; 
    // public Texture2D hoverCursor; 
    // public Texture2D grabbingCursor;

    // [Header("Cursor Hotspot/Location")]
    // public Vector2 hotspot = Vector2.zero;

    private SpriteRenderer rend;
    public Sprite CursorDefault;
    public Sprite CursorHover;
    public Sprite CursorActive;

    public GameObject clickEffect;

    void Start() {
        Cursor.visible = false;
        rend = GetComponent<SpriteRenderer>();
    }    

    void Update() {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;

        if (Input.GetMouseButtonDown(0)) {
            rend.sprite = CursorActive;
        } else if (Input.GetMouseButtonUp(0)) {
            rend.sprite = CursorDefault;
        }
    }
}

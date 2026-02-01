using UnityEngine;

public class HoverAnimation : MonoBehaviour
{
    float scaleFactor = 0.15f;
    SpriteRenderer mySprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        transform.localScale += new Vector3(scaleFactor, scaleFactor, 1);
        mySprite.material.color = Color.red;
    }

    private void OnMouseExit()
    {
        transform.localScale -= new Vector3(scaleFactor, scaleFactor, 1);
        mySprite.material.color = Color.white;
    }
}

using UnityEngine;

public class HoverAnimation : MonoBehaviour
{
    float scaleFactor = 0.2f;
    float scaleRate = 0.005f;
    Vector3 initialSize;
    SpriteRenderer mySprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        initialSize = transform.localScale;
    }

    /*private void OnMouseEnter()
    {
        transform.localScale += new Vector3(scaleFactor, scaleFactor, 1);
        mySprite.material.color = new Color32(255, 235, 200, 255);
    }*/

    private void OnMouseOver()
    {
        if (transform.localScale.x < initialSize.x + scaleFactor && transform.localScale.y < initialSize.y + scaleFactor) {
            transform.localScale += new Vector3(scaleRate, scaleRate, 1);
        }
        mySprite.material.color = new Color32(255, 235, 200, 255);
    }

    private void OnMouseExit()
    {
        transform.localScale -= new Vector3(scaleFactor, scaleFactor, 1);
        mySprite.material.color = Color.white;
    }
}

using UnityEngine;

public class HoverAnimation : MonoBehaviour
{
    float scaleFactor = 0.05f;
    float scaleRate = 0.003f;
    Vector3 initialSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialSize = transform.localScale;
    }

    /*private void OnMouseEnter()
    {
        transform.localScale += new Vector3(scaleFactor, scaleFactor, 1);
    }*/

    private void OnMouseOver()
    {
        if (transform.localScale.x < initialSize.x + scaleFactor && transform.localScale.y < initialSize.y + scaleFactor) {
            transform.localScale += new Vector3(scaleRate, scaleRate, 1);
        }
    }

    private void OnMouseExit()
    {
        transform.localScale -= new Vector3(scaleFactor, scaleFactor, 1);
    }
}

using UnityEngine;

public class ClickSound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public AudioSource audio;
    public void playButton()
    {
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

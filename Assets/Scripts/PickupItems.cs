using System.Collections.Generic;
using UnityEngine;

public class PickupItems : MonoBehaviour
{
    public string ItemName;

    [Range(GameController.VERY_RACCOON, GameController.VERY_HUMAN)]
    public int Value;

    //[Range(1, 5)]
    //public int showOnTier = 1;

    public List<PickupItems> connectedItems;

    public bool relyOnConnectedItem = false;

    public bool isConsumable = false;
    public Sprite newIcon;

    private Sprite sprite;
    private GameController controller;

    [Header("SFX")]
    public AudioClip clickSfx;
    [Range(0f, 1f)] public float clickVolume = 1f;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
        controller = FindAnyObjectByType<GameController>();
    }

    public Sprite GetIcon() => sprite;

    public virtual void OnMouseDown()
    {
            
        if (controller.IsDialogueActive()) return;

        if (clickSfx != null)
        {
            var cam = Camera.main;
            Vector3 pos = cam != null ? cam.transform.position : transform.position;
            AudioSource.PlayClipAtPoint(clickSfx, pos, clickVolume);
        }
        if (!isConsumable)
        {
            controller.PickUpItem(this);
            foreach (PickupItems pickup in connectedItems)
            {
                if (pickup != null) pickup.gameObject.SetActive(true);
            }
        }
        else
        {
            controller.AddToMeterValue(Value);
            if (newIcon != null)
            {
                var playerChar = GameObject.FindWithTag("Player");
                playerChar.GetComponent<SpriteRenderer>().sprite = newIcon;
            }
            
        }
        gameObject.SetActive(false);
    }
    public virtual void Reset()
    {
        gameObject.SetActive(true);
    }
}

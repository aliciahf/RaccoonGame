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

    private Sprite sprite;
    private GameController controller;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
        controller = FindAnyObjectByType<GameController>();
    }

    public Sprite GetIcon() => sprite;

    public virtual void OnMouseDown()
    {
        if (controller.IsDialogueActive()) return;

        controller.PickUpItem(this);

        gameObject.SetActive(false);
        foreach (PickupItems pickup in connectedItems)
        {
            if(pickup != null) pickup.gameObject.SetActive(true);
        }
    }

    public virtual void Reset()
    {
        gameObject.SetActive(true);
    }
}

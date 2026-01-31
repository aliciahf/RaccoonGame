using UnityEngine;

public class PickupItems : MonoBehaviour
{
    public string Name;

    [Range(GameController.VERY_RACCOON, GameController.VERY_HUMAN)]
    public int Value;

    private Sprite sprite;
    private GameController controller;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
        controller = FindAnyObjectByType<GameController>();
    }

    public Sprite GetIcon() => sprite;

    void OnMouseDown()
    {
        if (controller.IsDialogueActive()) return;

        controller.PickUpItem(this);

        gameObject.SetActive(false);
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}

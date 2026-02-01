using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string charName;
    public Sprite Portrait;

    [Header("Interactions")]
    public List<Interaction> interactions = new();

    private GameController controller;

    void Awake()
    {
        controller = FindFirstObjectByType<GameController>();
    }

    private void OnMouseDown()
    {
        if (controller.IsDialogueActive()) return;

        Interaction chosenInteraction = HowToInteract();
        if (chosenInteraction == null)
        {
            controller.StartDialogue(new List<string> { "..." }, charName, Portrait);
            return;
        }
        Interact(chosenInteraction);
    }

    private Interaction HowToInteract()
    {
        var equippedItem = controller.GetSelectedItem();
        if (equippedItem != null && !string.IsNullOrEmpty(equippedItem.ItemName))
        {
            foreach(var inter in interactions)
            {
                if (inter == null) continue;
                if (inter.triggerType != InteractionTriggerType.Item) continue;
                if (!string.IsNullOrEmpty(inter.triggerItem) && inter.triggerItem == equippedItem.ItemName)
                {
                    return inter;
                }
            }
        }

        MeterRank currentRank = controller.GetCurrentRank();
        foreach(var inter in interactions)
        {
            if (inter == null) continue;
            if (inter.triggerType != InteractionTriggerType.Rank) continue;
            if (currentRank == inter.triggerRank)
            {
                return inter;
            }
        }

        return null;
    }

    private void Interact(Interaction inter)
    {
        if(inter.dialogue != null && inter.dialogue.Count > 0)
        {
            controller.StartDialogue(inter.dialogue, charName, Portrait);
        }

        controller.AddToMeterValue(inter.meterValue);
        if (inter.advanceTier) controller.SetTier(controller.GetTier() + 1);
        if(inter.destroyCharacter) this.gameObject.SetActive(false);
        if (inter.newIcon) this.GetComponent<SpriteRenderer>().sprite = inter.newIcon;

        if (inter.endLevel)
        {
            controller.StartDialogue(new List<string> { "You Win!" }, charName, Portrait);
        }

    }
}

using System.Collections.Generic;
using UnityEngine;

public enum InteractionTriggerType
{
    Rank, 
    Item
}

[System.Serializable]
public class Interaction
{
    [Header("Trigger")]
    public InteractionTriggerType triggerType = InteractionTriggerType.Rank;

    public MeterRank triggerRank;
    public string triggerItem;

    [Header("Actions")]
    public List<string> dialogue = new();

    [Range(GameController.VERY_RACCOON, GameController.VERY_HUMAN)]
    public int meterValue;

    public bool advanceTier = false;
    public bool endLevel = false;

    public Sprite newIcon;
    public Sprite newRaccoonIcon;
    public bool destroyCharacter = false;

    public List<GameObject> spawnItems;
}

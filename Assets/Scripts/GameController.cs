using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Initial Meter Value")]
    [Range(-10, 10)]
    public float initialMeterValue = 0f;

    private float currentMeterValue;

    public const float VERY_RACCOON = -10f;
    public const float LITTLE_RACCOON = -5f;
    public const float WHAT_ARE_YOU = 0f;
    public const float HUMANISH = 5f;
    public const float VERY_HUMAN = 10f;

    [Header("Max Tiers")]
    [Range(1, 5)]
    public int maxTiers = 3;

    private int currentTier = 1;

    [Header("Canvas")]
    public Canvas canvas;

    private GameObject dialoguePanel;
    private Image dialoguePortrait;
    private TMP_Text dialogueName;
    private TMP_Text dialogueText;

    private Image selectedItemImage;
    private PickupItems currentItem;

    private RectTransform meterMarker;

    private readonly List<DialogueLine> dialogueLines = new();
    private int dialogueIndex = -1;
    private bool dialogueActive = false;

    Transform FindCanvasChild(string name)
    {
        foreach(var trans in canvas.GetComponentsInChildren<Transform>(true))
        {
            if (trans.name == name) return trans;
        }
        return null;
    }

    private void Awake()
    {
        if (canvas == null)
            canvas = FindFirstObjectByType<Canvas>();

        if(canvas == null)
        {
            Debug.LogError("No canvas found in scene");
            return;
        }

        dialoguePanel = FindCanvasChild("DialoguePanel")?.gameObject;
        dialoguePortrait = FindCanvasChild("Portrait")?.GetComponent<Image>();
        dialogueName = FindCanvasChild("Speaker")?.GetComponent<TMP_Text>();
        dialogueText = FindCanvasChild("DialogueText")?.GetComponent<TMP_Text>();

        selectedItemImage = FindCanvasChild("SelectedItem")?.GetComponent<Image>();

        meterMarker = FindCanvasChild("MeterTrack")?.GetComponent<RectTransform>();

        if (dialoguePanel == null) Debug.LogWarning("No dialogue panel");
        if (selectedItemImage == null) Debug.LogWarning("No selected item");
        if (meterMarker == null) Debug.LogWarning("No Meter Marker");

    }

    private void Start()
    {
        currentMeterValue = initialMeterValue;
        if(dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if(dialogueActive && Input.GetMouseButtonDown(0))
        {

        }
    }

    public float GetCurrentMeterValue() => currentMeterValue;

    public void SetMeterValue(float value)
    {
        currentMeterValue = Mathf.Clamp(value, VERY_RACCOON, VERY_HUMAN);
    }

    public void AddToMeterValue(float add)
    {
        SetMeterValue(currentMeterValue + add);
    }

    public MeterRank GetCurrentRank()
    {
        if (currentMeterValue <= VERY_RACCOON) return MeterRank.VeryRaccoon;
        if (currentMeterValue <= LITTLE_RACCOON) return MeterRank.ALittleRaccoon;
        if (currentMeterValue >= VERY_HUMAN) return MeterRank.VeryHuman;
        if (currentMeterValue >= HUMANISH) return MeterRank.Humanish;
        return MeterRank.WhatAreYou;
    }

    public PickupItems GetSelectedItem() => currentItem;

    public bool hasSelectedItem() => currentItem != null;

    public void PickUpItem(PickupItems item)
    {
        if (currentItem != null) DropItem();
        currentItem = item;
        selectedItemImage.sprite = item.GetIcon();
        AddToMeterValue(item.Value);
    }

    public void DropItem()
    {
        AddToMeterValue(currentItem.Value * -1);
        currentItem.Reset();
        currentItem = null;
        selectedItemImage.sprite = null;
        Debug.Log(currentMeterValue);
    }

    public int GetTier() => currentTier;

    public void SetTier(int tier)
    {
        currentTier = Mathf.Clamp(tier, 1, maxTiers);
    }

    public bool IsDialogueActive() => dialogueActive;

}

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Initial Meter Value")]
    [Range(-10, 10)]
    public int initialMeterValue = -10;

    private int currentMeterValue;

    public const int VERY_RACCOON = -10;
    public const int LITTLE_RACCOON = -5;
    public const int WHAT_ARE_YOU = 0;
    public const int HUMANISH = 5;
    public const int VERY_HUMAN = 10;

    private const float X_AT_RACCOON = -380f; 
    private const float X_AT_HUMAN = 440f;

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
    private RectTransform meterTrack;

    private List<string> activeDialogue = new();
    private int dialogueIndex = -1;
    private bool dialogueActive = false;

    private string activeSpeakerName;
    private Sprite activeSpeakerPortrait;

    private bool blockAdvanceThisClick = false;

    private PickupItems[] allPickups;
    private CombinableItem[] allCombinables;

    Transform FindCanvasChild(string name)
    {
        foreach(var trans in canvas.GetComponentsInChildren<Transform>(true))
        {
            if (trans.name == name) return trans;
        }
        return null;
    }

    void Awake()
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

        meterMarker = FindCanvasChild("MeterMarker")?.GetComponent<RectTransform>();
        meterTrack = FindCanvasChild("MeterTrack")?.GetComponent<RectTransform>();

        if (dialoguePanel == null) Debug.LogWarning("No dialogue panel");
        if (selectedItemImage == null) Debug.LogWarning("No selected item");
        if (meterMarker == null) Debug.LogWarning("No Meter Marker");
        allPickups = FindObjectsByType<PickupItems>(FindObjectsSortMode.None);
        allCombinables = FindObjectsByType<CombinableItem>(FindObjectsSortMode.None);
    }

    void Start()
    {
        currentMeterValue = initialMeterValue;
        selectedItemImage.enabled = false;
        if(dialoguePanel != null) dialoguePanel.SetActive(false);
        UpdateMeterUI();
        foreach (PickupItems pickup in allPickups)
        {
            if (pickup.relyOnConnectedItem) pickup.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!dialogueActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (blockAdvanceThisClick)
            {
                blockAdvanceThisClick = false;
                return;
            }

            AdvanceDialogue();
        }
    }

    public float GetCurrentMeterValue() => currentMeterValue;

    public void SetMeterValue(int value)
    {
        currentMeterValue = value;
        if (currentMeterValue > VERY_HUMAN) currentMeterValue = VERY_HUMAN;
        if (currentMeterValue < VERY_RACCOON) currentMeterValue = VERY_RACCOON;

    }

    public void AddToMeterValue(int add)
    {
        SetMeterValue(currentMeterValue + add);
        UpdateMeterUI();
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
        if (currentItem != null && item is not CombinableItem) DropItem();
        currentItem = item;
        selectedItemImage.sprite = item.GetIcon();
        AddToMeterValue(item.Value);
        selectedItemImage.enabled = true;
    }

    public void DropItem()
    {
        if(currentItem == null) return;
        AddToMeterValue(currentItem.Value * -1);
        if(currentItem is CombinableItem)
        {
            var combinable = (CombinableItem)currentItem;
            foreach(PickupItems comb in  combinable.combinors)
            {
                AddToMeterValue(comb.Value * -1);
            }
        }
        currentItem.Reset();
        currentItem = null;
        selectedItemImage.sprite = null;
        selectedItemImage.enabled = false;
    }

    public int GetTier() => currentTier;

    public void SetTier(int tier)
    {
        currentTier = Mathf.Clamp(tier, 1, maxTiers);
        //foreach(var pickup in allPickups)
        //{
        //   if (pickup.showOnTier >= currentTier) pickup.Reset();
        //}
    }

    public bool IsDialogueActive() => dialogueActive;

    private void UpdateMeterUI()
    {
        if (meterTrack == null || meterMarker == null) return;
        float v = Mathf.Clamp(currentMeterValue, VERY_RACCOON, VERY_HUMAN);

        float t = (v - VERY_RACCOON) / (VERY_HUMAN - VERY_RACCOON);

        float x = Mathf.Lerp(X_AT_RACCOON, X_AT_HUMAN, t);

        var pos = meterMarker.anchoredPosition;
        pos.x = x;
        meterMarker.anchoredPosition = pos;
    }

    public void StartDialogue(List<string> lines, string speakerName, Sprite portrait)
    {
        if(lines.Count == 0) return;

        activeDialogue = lines;
        activeSpeakerName = speakerName;
        activeSpeakerPortrait = portrait;
        dialogueIndex = -1;
        dialogueActive = true;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        blockAdvanceThisClick = true;
        AdvanceDialogue();
    }

    private void AdvanceDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex >= activeDialogue.Count)
        {
            dialogueActive = false;
            if (dialoguePanel != null) dialoguePanel.SetActive(false);
            return;
        }

        if (dialogueText != null) 
            dialogueText.text = activeDialogue[dialogueIndex];

        if (dialogueName != null)
            dialogueName.text = activeSpeakerName;

        if (dialoguePortrait != null)
        {
            dialoguePortrait.sprite = activeSpeakerPortrait;
            dialoguePortrait.enabled = true;
        }
    }
}

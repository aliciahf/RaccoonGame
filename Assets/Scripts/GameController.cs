using System;
using System.Collections.Generic;
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

        meterMarker = FindCanvasChild("MeterMarker")?.GetComponent<RectTransform>();
        meterTrack = FindCanvasChild("MeterTrack")?.GetComponent<RectTransform>();

        if (dialoguePanel == null) Debug.LogWarning("No dialogue panel");
        if (selectedItemImage == null) Debug.LogWarning("No selected item");
        if (meterMarker == null) Debug.LogWarning("No Meter Marker");

    }

    private void Start()
    {
        currentMeterValue = initialMeterValue;
        selectedItemImage.enabled = false;
        if(dialoguePanel != null) dialoguePanel.SetActive(false);
        UpdateMeterUI();
    }

    void Update()
    {
        if(dialogueActive && Input.GetMouseButtonDown(0))
        {
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
        if (currentItem != null) DropItem();
        currentItem = item;
        selectedItemImage.sprite = item.GetIcon();
        AddToMeterValue(item.Value);
        selectedItemImage.enabled = true;
    }

    public void DropItem()
    {
        if(currentItem == null) return;
        AddToMeterValue(currentItem.Value * -1);
        currentItem.Reset();
        currentItem = null;
        selectedItemImage.sprite = null;
        selectedItemImage.enabled = false;
    }

    public int GetTier() => currentTier;

    public void SetTier(int tier)
    {
        currentTier = Mathf.Clamp(tier, 1, maxTiers);
    }

    public bool IsDialogueActive() => dialogueActive;

    private void UpdateMeterUI()
    {
        if (meterTrack == null || meterMarker == null) return;
        Debug.Log(currentMeterValue);
        float v = Mathf.Clamp(currentMeterValue, VERY_RACCOON, VERY_HUMAN);

        float t = (v - VERY_RACCOON) / (VERY_HUMAN - VERY_RACCOON);

        float x = Mathf.Lerp(X_AT_RACCOON, X_AT_HUMAN, t);

        var pos = meterMarker.anchoredPosition;
        pos.x = x;
        meterMarker.anchoredPosition = pos;
    }

    public void ShowText(string text, string speakerName = "", Sprite portrait = null)
    {
        var lines = new List<DialogueLine>
        {
            new DialogueLine { text = text, speakerName = speakerName, portrait = portrait }
        };
        StartDialogue(lines);
    }

    public void StartDialogue(List<DialogueLine> lines)
    {
        dialogueLines.Clear();
        dialogueLines.AddRange(lines);
        dialogueIndex = -1;
        dialogueActive = true;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        AdvanceDialogue();
    }

    private void AdvanceDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex >= dialogueLines.Count)
        {
            dialogueActive = false;
            if (dialoguePanel != null) dialoguePanel.SetActive(false);
            return;
        }

        var line = dialogueLines[dialogueIndex];

        if (dialogueText != null) dialogueText.text = line.text ?? "";

        if (dialogueName != null)
            dialogueName.text = string.IsNullOrEmpty(line.speakerName) ? "" : line.speakerName;

        if (dialoguePortrait != null)
        {
            if (line.portrait != null)
            {
                dialoguePortrait.sprite = line.portrait;
                dialoguePortrait.enabled = true;
            }
            else
            {
                dialoguePortrait.sprite = null;
                dialoguePortrait.enabled = false;
            }
        }
    }
}

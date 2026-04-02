using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("TMP References")]
    [SerializeField] private TMP_Text sentenceText;
    [SerializeField] private TMP_Text totalWordsText;
    [SerializeField] private TMP_Text correctWordsText;
    [SerializeField] private TMP_Text skippedWordsText;
    [SerializeField] private TMP_Text completionText;
    [SerializeField] private TMP_Text recognizedWordText;

    [Header("Buttons")]
    [SerializeField] private Button recordButton;
    [SerializeField] private Button skipButton;

    [Header("Button Labels")]
    [SerializeField] private TMP_Text recordButtonLabel;

    private void Awake()
    {
        TryAutoAssign();
    }

    private void TryAutoAssign()
    {
        try
        {
            if (sentenceText == null) sentenceText = FindTmp("SentenceText");
            if (totalWordsText == null) totalWordsText = FindTmp("TotalWordsText");
            if (correctWordsText == null) correctWordsText = FindTmp("CorrectWordsText");
            if (skippedWordsText == null) skippedWordsText = FindTmp("SkippedWordsText");
            if (completionText == null) completionText = FindTmp("CompletionText");
            if (recognizedWordText == null) recognizedWordText = FindTmp("RecognizedWord");

            if (recordButton == null)
            {
                GameObject obj = GameObject.Find("RecordButton");
                if (obj != null) recordButton = obj.GetComponent<Button>();
            }

            if (skipButton == null)
            {
                GameObject obj = GameObject.Find("SkipButton");
                if (obj != null) skipButton = obj.GetComponent<Button>();
            }

            if (recordButtonLabel == null && recordButton != null)
            {
                recordButtonLabel = recordButton.GetComponentInChildren<TMP_Text>();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"UIManager auto-assign failed: {ex.Message}");
        }
    }

    public void SetSentenceText(string text)
    {
        try
        {
            if (sentenceText != null)
            {
                sentenceText.text = text;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"SetSentenceText failed: {ex.Message}");
        }
    }

    public void RefreshScore(ScoreManager scoreManager)
    {
        try
        {
            if (scoreManager == null) return;

            if (totalWordsText != null)
            {
                totalWordsText.text = $"Total Words: {scoreManager.TotalWords}";
            }

            if (correctWordsText != null)
            {
                correctWordsText.text = $"Correct Words: {scoreManager.CorrectWords}";
            }

            if (skippedWordsText != null)
            {
                skippedWordsText.text = $"Skipped Words: {scoreManager.SkippedWords}";
            }

            if (completionText != null)
            {
                completionText.text = $"Completion: {scoreManager.CompletionPercentage:0}%";
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"RefreshScore failed: {ex.Message}");
        }
    }

    public void SetButtonsInteractable(bool value)
    {
        try
        {
            if (recordButton != null) recordButton.interactable = value;
            if (skipButton != null) skipButton.interactable = value;
        }
        catch (Exception ex)
        {
            Debug.LogError($"SetButtonsInteractable failed: {ex.Message}");
        }
    }

    public void SetRecordButtonLabel(string value)
    {
        try
        {
            if (recordButtonLabel != null)
            {
                recordButtonLabel.text = value;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"SetRecordButtonLabel failed: {ex.Message}");
        }
    }

    public void SetRecognizedWord(string text)
    {
        try
        {
            if (recognizedWordText != null)
                recognizedWordText.text = $"Recognized Word: {text}";
        }
        catch (Exception ex)
        {
            Debug.LogError($"SetRecognizedWord failed: {ex.Message}");
        }
    }

    private TMP_Text FindTmp(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj == null) return null;
        return obj.GetComponent<TMP_Text>();
    }
}
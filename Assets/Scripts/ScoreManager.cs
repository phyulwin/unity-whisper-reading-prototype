using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int TotalWords { get; private set; }
    public int CorrectWords { get; private set; }
    public int SkippedWords { get; private set; }

    public int ProcessedWords => CorrectWords + SkippedWords;

    public float CompletionPercentage
    {
        get
        {
            if (TotalWords <= 0) return 0f;
            return (ProcessedWords / (float)TotalWords) * 100f;
        }
    }

    public void Initialize(string fullText)
    {
        try
        {
            CorrectWords = 0;
            SkippedWords = 0;
            TotalWords = CountWords(fullText);
        }
        catch (Exception ex)
        {
            Debug.LogError($"ScoreManager Initialize failed: {ex.Message}");
        }
    }

    public void AddCorrect(int amount)
    {
        try
        {
            CorrectWords += Mathf.Max(0, amount);
        }
        catch (Exception ex)
        {
            Debug.LogError($"AddCorrect failed: {ex.Message}");
        }
    }

    public void AddSkipped(int amount)
    {
        try
        {
            SkippedWords += Mathf.Max(0, amount);
        }
        catch (Exception ex)
        {
            Debug.LogError($"AddSkipped failed: {ex.Message}");
        }
    }

    private int CountWords(string text)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;

            string[] words = text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }
        catch
        {
            return 0;
        }
    }
}
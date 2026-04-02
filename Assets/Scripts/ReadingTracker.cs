using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ReadingTracker : MonoBehaviour
{
    private enum WordState
    {
        Pending,
        Correct,
        Skipped
    }

    private readonly List<string> originalWords = new List<string>();
    private readonly List<WordState> wordStates = new List<WordState>();

    private int currentWordIndex;

    public bool IsSentenceComplete => currentWordIndex >= originalWords.Count;

    public void SetSentence(string sentence)
    {
        try
        {
            originalWords.Clear();
            wordStates.Clear();
            currentWordIndex = 0;

            string[] split = sentence.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in split)
            {
                originalWords.Add(word);
                wordStates.Add(WordState.Pending);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"SetSentence failed: {ex.Message}");
        }
    }

    public int ProcessRecognizedText(string recognizedText)
    {
        int matched = 0;

        try
        {
            string[] spokenWords = recognizedText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < spokenWords.Length; i++)
            {
                if (currentWordIndex >= originalWords.Count) break;

                string expected = Normalize(originalWords[currentWordIndex]);
                string actual = Normalize(spokenWords[i]);

                if (expected == actual)
                {
                    wordStates[currentWordIndex] = WordState.Correct;
                    currentWordIndex++;
                    matched++;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"ProcessRecognizedText failed: {ex.Message}");
        }

        return matched;
    }

    public bool SkipCurrentWord()
    {
        try
        {
            if (currentWordIndex >= originalWords.Count) return false;

            wordStates[currentWordIndex] = WordState.Skipped;
            currentWordIndex++;
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"SkipCurrentWord failed: {ex.Message}");
            return false;
        }
    }

    public string GetFormattedSentence()
    {
        try
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < originalWords.Count; i++)
            {
                string word = EscapeRichText(originalWords[i]);

                switch (wordStates[i])
                {
                    case WordState.Correct:
                        builder.Append($"<color=#00B050>{word}</color>");
                        break;

                    case WordState.Skipped:
                        builder.Append($"<color=#FF0000>{word}</color>");
                        break;

                    default:
                        if (i == currentWordIndex)
                        {
                            builder.Append($"<u>{word}</u>");
                        }
                        else
                        {
                            builder.Append(word);
                        }
                        break;
                }

                if (i < originalWords.Count - 1)
                {
                    builder.Append(" ");
                }
            }

            return builder.ToString();
        }
        catch (Exception ex)
        {
            Debug.LogError($"GetFormattedSentence failed: {ex.Message}");
            return string.Empty;
        }
    }

    private string Normalize(string input)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            char[] chars = input.ToLowerInvariant().ToCharArray();
            List<char> cleaned = new List<char>();

            foreach (char c in chars)
            {
                if (char.IsLetterOrDigit(c) || c == '\'')
                {
                    cleaned.Add(c);
                }
            }

            return new string(cleaned.ToArray());
        }
        catch
        {
            return string.Empty;
        }
    }

    private string EscapeRichText(string input)
    {
        return input.Replace("<", "&lt;").Replace(">", "&gt;");
    }
}
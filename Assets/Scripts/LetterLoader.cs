using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LetterLoader : MonoBehaviour
{
    public string FullText { get; private set; } = string.Empty;

    private readonly List<string> sentences = new List<string>();

    public int SentenceCount => sentences.Count;

    public void LoadLetter(string resourceFileName)
    {
        try
        {
            sentences.Clear();
            FullText = string.Empty;

            TextAsset textAsset = Resources.Load<TextAsset>(resourceFileName);
            if (textAsset == null)
            {
                Debug.LogError($"Could not load Resources/{resourceFileName}.txt");
                return;
            }

            FullText = textAsset.text ?? string.Empty;
            ParseSentences(FullText);
        }
        catch (Exception ex)
        {
            Debug.LogError($"LetterLoader failed: {ex.Message}");
        }
    }

    public string GetSentence(int index)
    {
        if (index < 0 || index >= sentences.Count)
        {
            return string.Empty;
        }

        return sentences[index];
    }

    private void ParseSentences(string text)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                builder.Append(c);

                bool isSentenceEnd = c == '.' || c == '!' || c == '?';
                if (isSentenceEnd)
                {
                    string sentence = builder.ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(sentence))
                    {
                        sentences.Add(sentence);
                    }
                    builder.Clear();
                }
            }

            string leftover = builder.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(leftover))
            {
                sentences.Add(leftover);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Sentence parsing failed: {ex.Message}");
        }
    }
}
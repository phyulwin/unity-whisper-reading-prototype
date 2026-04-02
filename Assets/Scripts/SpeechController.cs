using System;
using System.Threading.Tasks;
using UnityEngine;
using Whisper;

public class SpeechController : MonoBehaviour
{
    [SerializeField] private WhisperManager whisperManager;
    [SerializeField] private int sampleRate = 16000;
    [SerializeField] private float processInterval = 1.8f;
    [SerializeField] private int recordingLength = 10;
    [SerializeField] private int channels = 1;

    private GameManager gameManager;
    private bool isRecording;
    private string micName;

    private AudioClip liveClip;
    private int lastSamplePosition;

    public void Initialize(GameManager manager)
    {
        try
        {
            gameManager = manager;

            if (whisperManager == null)
                whisperManager = FindFirstObjectByType<WhisperManager>();

            if (Microphone.devices.Length > 0)
                micName = Microphone.devices[0];
            else
                Debug.LogError("No microphone detected.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"SpeechController Initialize failed: {ex.Message}");
        }
    }

    public bool ToggleRecording()
    {
        try
        {
            if (!isRecording)
                StartContinuousListening();
            else
                StopListening();

            return isRecording;
        }
        catch (Exception ex)
        {
            Debug.LogError($"ToggleRecording failed: {ex.Message}");
            return false;
        }
    }

    private async void StartContinuousListening()
    {
        try
        {
            if (string.IsNullOrEmpty(micName)) return;

            isRecording = true;
            liveClip = Microphone.Start(micName, true, recordingLength, sampleRate);
            lastSamplePosition = 0;

            await Task.Delay(200);

            while (isRecording)
            {
                await Task.Delay(TimeSpan.FromSeconds(processInterval));

                int currentPosition = Microphone.GetPosition(micName);
                if (currentPosition <= 0) continue;

                int sampleCount;

                if (currentPosition >= lastSamplePosition)
                {
                    sampleCount = currentPosition - lastSamplePosition;
                    if (sampleCount == 0) continue;

                    float[] samples = new float[sampleCount];
                    liveClip.GetData(samples, lastSamplePosition);

                    var result = await whisperManager.GetTextAsync(samples, sampleRate, channels);

                    if (!string.IsNullOrWhiteSpace(result.Result))
                    {
                        Debug.Log("Recognized: " + result.Result);
                        gameManager?.OnSpeechRecognized(result.Result);
                    }
                }
                else
                {
                    int tailCount = liveClip.samples - lastSamplePosition;
                    int headCount = currentPosition;
                    sampleCount = tailCount + headCount;

                    float[] samples = new float[sampleCount];
                    float[] tail = new float[tailCount];
                    float[] head = new float[headCount];

                    liveClip.GetData(tail, lastSamplePosition);
                    liveClip.GetData(head, 0);

                    Array.Copy(tail, 0, samples, 0, tailCount);
                    Array.Copy(head, 0, samples, tailCount, headCount);

                    var result = await whisperManager.GetTextAsync(samples, sampleRate, channels);

                    if (!string.IsNullOrWhiteSpace(result.Result))
                    {
                        Debug.Log("Recognized: " + result.Result);
                        gameManager?.OnSpeechRecognized(result.Result);
                    }
                }

                lastSamplePosition = Mathf.Max(0, currentPosition - sampleRate / 2);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Continuous listening failed: {ex.Message}");
            isRecording = false;
        }
    }

    public void StopListening()
    {
        try
        {
            isRecording = false;

            if (!string.IsNullOrEmpty(micName) && Microphone.IsRecording(micName))
                Microphone.End(micName);
        }
        catch (Exception ex)
        {
            Debug.LogError($"StopListening failed: {ex.Message}");
        }
    }
}

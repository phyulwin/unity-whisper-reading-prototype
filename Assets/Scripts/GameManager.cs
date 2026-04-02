using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private string resourceTextFileName = "SampleText";

    [Header("Managers")]
    [SerializeField] private LetterLoader letterLoader;
    [SerializeField] private ReadingTracker readingTracker;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SpeechController speechController;

    [Header("UI")]
    [SerializeField] private Button recordButton;
    [SerializeField] private Button skipButton;

    private int currentSentenceIndex;
    private bool gameFinished;

    private void Awake()
    {
        TryAutoAssignReferences();
    }

    private void Start()
    {
        BindButtons();
        InitializeGame();
    }

    private void TryAutoAssignReferences()
    {
        try
        {
            if (letterLoader == null) letterLoader = FindFirstObjectByType<LetterLoader>();
            if (readingTracker == null) readingTracker = FindFirstObjectByType<ReadingTracker>();
            if (scoreManager == null) scoreManager = FindFirstObjectByType<ScoreManager>();
            if (uiManager == null) uiManager = FindFirstObjectByType<UIManager>();
            if (speechController == null) speechController = FindFirstObjectByType<SpeechController>();

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
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"GameManager reference assignment failed: {ex.Message}");
        }
    }

    private void BindButtons()
    {
        try
        {
            if (recordButton != null)
            {
                recordButton.onClick.RemoveListener(OnRecordButtonPressed);
                recordButton.onClick.AddListener(OnRecordButtonPressed);
            }

            if (skipButton != null)
            {
                skipButton.onClick.RemoveListener(OnSkipButtonPressed);
                skipButton.onClick.AddListener(OnSkipButtonPressed);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Button binding failed: {ex.Message}");
        }
    }

    private void InitializeGame()
    {
        try
        {
            gameFinished = false;
            currentSentenceIndex = 0;

            letterLoader.LoadLetter(resourceTextFileName);
            scoreManager.Initialize(letterLoader.FullText);
            speechController.Initialize(this);

            if (letterLoader.SentenceCount == 0)
            {
                uiManager.SetSentenceText("No sentences found.");
                uiManager.SetButtonsInteractable(false);
                return;
            }

            LoadCurrentSentence();
            uiManager.RefreshScore(scoreManager);
            uiManager.SetRecordButtonLabel("Record");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Game initialization failed: {ex.Message}");
        }
    }

    private void LoadCurrentSentence()
    {
        try
        {
            string sentence = letterLoader.GetSentence(currentSentenceIndex);
            readingTracker.SetSentence(sentence);
            uiManager.SetSentenceText(readingTracker.GetFormattedSentence());
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"LoadCurrentSentence failed: {ex.Message}");
        }
    }

    public void OnRecordButtonPressed()
    {
        try
        {
            if (gameFinished) return;

            bool isRecording = speechController.ToggleRecording();
            uiManager.SetRecordButtonLabel(isRecording ? "Pause" : "Record");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Record button action failed: {ex.Message}");
        }
    }

    public void OnSkipButtonPressed()
    {
        try
        {
            if (gameFinished) return;

            bool skipped = readingTracker.SkipCurrentWord();
            if (skipped)
            {
                scoreManager.AddSkipped(1);
                RefreshAfterWordUpdate();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Skip button action failed: {ex.Message}");
        }
    }

    public void OnSpeechRecognized(string recognizedText)
    {
        try
        {
            if (gameFinished) return;
            if (string.IsNullOrWhiteSpace(recognizedText)) return;

            uiManager.SetRecognizedWord(recognizedText);

            int matchedCount = readingTracker.ProcessRecognizedText(recognizedText);

            if (matchedCount > 0)
                scoreManager.AddCorrect(matchedCount);

            RefreshAfterWordUpdate(); // always refresh UI
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Speech processing failed: {ex.Message}");
        }
    }

    private void RefreshAfterWordUpdate()
    {
        try
        {
            uiManager.SetSentenceText(readingTracker.GetFormattedSentence());
            uiManager.RefreshScore(scoreManager);

            if (readingTracker.IsSentenceComplete)
            {
                GoToNextSentence();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"RefreshAfterWordUpdate failed: {ex.Message}");
        }
    }

    private void GoToNextSentence()
    {
        try
        {
            currentSentenceIndex++;

            if (currentSentenceIndex >= letterLoader.SentenceCount)
            {
                FinishGame();
                return;
            }

            LoadCurrentSentence();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"GoToNextSentence failed: {ex.Message}");
        }
    }

    private void FinishGame()
    {
        gameFinished = true;
        uiManager.SetSentenceText("Finished!");
        uiManager.SetButtonsInteractable(false);
        uiManager.SetRecordButtonLabel("Done");
    }
}
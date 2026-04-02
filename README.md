# Unity Whisper Reading Prototype

![](/Assets/Resources/screenshot1.png)

A standalone Unity 2D speech-reading prototype built to test real-time word recognition using the Whisper speech-to-text pipeline. The application loads story text, breaks it into readable sentences, and guides the player through each word sequentially with live visual feedback.

Users read the displayed sentence aloud, and recognized words are validated against the current target word. Correct words are highlighted in green, skipped words turn red, and the underline advances automatically as progression continues. A live scoring dashboard tracks total words, correct reads, skipped words, and completion percentage.

The system follows a modular architecture with dedicated managers for text loading, speech capture, word tracking, scoring, and UI orchestration, making it easy to scale into future narrative or educational reading experiences.

This repository is intended as an isolated prototype environment for validating Unity microphone streaming, Whisper integration, and real-time reading UX workflows before production migration.

This prototype builds on the open-source **whisper.unity** package for Unity-based speech recognition, which provides the Whisper model integration and transcription pipeline used for microphone streaming and live text processing. Credit to the original package and contributors for the foundational STT infrastructure that enabled this prototype workflow:

![](/Assets/Resources/screenshot2.png)

To update the reading content, navigate to Assets/Resources/SampleText.txt and replace the existing text with your new story, dialogue, or sentence set. The loader automatically reads from this file at runtime, parses the content into sentences, and updates the reading flow without requiring any code changes.
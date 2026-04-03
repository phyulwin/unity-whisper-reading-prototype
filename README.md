# Unity Whisper Reading Prototype

![](/Assets/Resources/screenshot1.png)

A standalone Unity 2D speech-reading prototype for testing real-time word recognition using the Whisper speech-to-text pipeline.

The application loads text content, parses it into sentences, and guides the user through each word sequentially with live feedback.

As the user reads aloud:

* Recognized words are validated against the current target word
* Correct words are highlighted in green
* Skipped or incorrect words are marked in red
* The underline advances automatically as progression continues

A live dashboard tracks:

* Total words
* Correct reads
* Skipped words
* Completion percentage

---

## Architecture

The system is built with a modular structure, separating core responsibilities into dedicated components:

* Text Loading
* Speech Capture
* Word Tracking
* Scoring
* UI Orchestration

This makes the prototype easy to extend into larger narrative or educational applications.

---

## Purpose

This repository is an isolated prototype environment designed to validate:

* Unity microphone streaming
* Whisper integration
* Real-time reading UX workflows

It serves as a testing ground before moving into production.

---

## Whisper Integration

This project builds on the open-source **whisper.unity** package, which provides the underlying Whisper model integration and transcription pipeline used for microphone streaming and real-time text processing.

Credit to the original contributors for the foundational STT infrastructure.

![](/Assets/Resources/screenshot2.png)

---

## Updating Reading Content

To modify the reading content:

```
Assets/Resources/SampleText.txt
```

Replace the file contents with your desired text (story, dialogue, or sentence set).

The system will automatically:

* Load the file at runtime
* Parse it into sentences
* Update the reading flow

No code changes required.

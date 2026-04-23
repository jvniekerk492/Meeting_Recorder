# Meeting Recorder Architecture

## Overview

Meeting Recorder is a WPF desktop application that records microphone input and system audio into a single WAV file, persists recording session metadata, and will support AI-powered transcription of recorded meetings.
The solution is organised into focused projects following a clean layered architecture.

## Solution Structure

```text
MeetingRecorder/
├── AudioManager/
│   ├── AudioManager.csproj
│   ├── AudioRecorder.cs
│   ├── IAudioRecorder.cs
│   └── Recorder.cs
├── Data/
│   ├── Data.csproj
│   ├── SqliteApplicationSettings.cs
│   ├── SqliteApplicationSettingsRepository.cs
│   ├── SqliteRecordingSession.cs
│   └── SqliteRecordingSessionRepository.cs
├── DataRepository/
│   ├── DataRepository.csproj
│   ├── ApplicationSettings.cs
│   ├── IApplicationSettings.cs
│   ├── IApplicationSettingsRepository.cs
│   ├── IRecordingSession.cs
│   ├── IRecordingSessionRepository.cs
│   └── RecordingSession.cs
├── InferenceService/
│   ├── InferenceService.csproj
│   ├── LanguageModelBase.cs
│   ├── LlamaModel.cs
│   └── OllamaModel.cs
├── Meeting_Recorder/
│   ├── Factories/
│   │   ├── ViewFactory.cs
│   │   └── ViewModelFactory.cs
│   ├── Interface/
│   │   ├── IView.cs
│   │   └── IViewModel.cs
│   ├── Resources/
│   │   └── Styles.xaml
│   ├── ViewModels/
│   │   ├── BasicSettingsViewModel.cs
│   │   ├── ITimer.cs
│   │   ├── MainWindowViewModel.cs
│   │   ├── RecorderViewModel.cs
│   │   ├── RelayCommand.cs
│   │   ├── RelayCommandGeneric.cs
│   │   ├── TranscribeMeetingViewModel.cs
│   │   └── ViewModelBase.cs
│   ├── Views/
│   │   ├── BasicSettings.xaml
│   │   ├── BasicSettings.xaml.cs
│   │   ├── Recorder.xaml
│   │   ├── Recorder.xaml.cs
│   │   ├── TranscribeMeeting.xaml
│   │   ├── TranscribeMeeting.xaml.cs
│   │   └── ViewType.cs
│   ├── App.xaml
│   ├── App.xaml.cs
│   ├── AssemblyInfo.cs
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
│   └── Meeting_Recorder.csproj
├── Meeting_Recorder.Tests/
│   ├── Data/
│   │   └── SqliteRecordingSessionRepositoryTests.cs
│   ├── InferenceService/
│   │   ├── LlamaModelTests.cs
│   │   └── OllamaModelTests.cs
│   ├── Infrastructure/
│   │   └── WpfApplicationFixture.cs
│   ├── TestDoubles/
│   │   └── InMemoryApplicationSettingsRepository.cs
│   ├── ViewModels/
│   │   ├── BasicSettingsViewModelTests.cs
│   │   ├── MainWindowViewModelTests.cs
│   │   ├── RecorderViewModelSessionTests.cs
│   │   ├── RecorderViewModelTests.cs
│   │   ├── TranscribeMeetingViewModelTests.cs
│   │   └── ViewModelFactory.cs
│   ├── Views/
│   │   ├── TranscribeMeetingViewTests.cs
│   │   └── ViewFactoryTests.cs
│   ├── AudioRecorderTests.cs
│   └── Meeting_Recorder.Tests.csproj
├── .github/
│   └── agents/
│       └── LatestPackageAgent.agent.md
├── Architecture.md
├── Directory.Packages.props
├── Meeting_Recorder.slnx
└── nuget.config
```

## Project Responsibilities

- **Meeting_Recorder.csproj**: WPF application entry point, UI views, view models, and navigation.
- **AudioManager/AudioManager.csproj**: Audio capture implementation using NAudio.
- **DataRepository/DataRepository.csproj**: Contracts (interfaces) and plain models for settings and recording sessions. No external dependencies.
- **Data/Data.csproj**: SQLite persistence implementations of the `DataRepository` interfaces using `Microsoft.Data.Sqlite`.
- **InferenceService/InferenceService.csproj**: AI language model integration via Microsoft Semantic Kernel and OllamaSharp, providing `LlamaModel` and `OllamaModel` for local and remote LLM inference.
- **Meeting_Recorder.Tests/Meeting_Recorder.Tests.csproj**: Unit tests covering view models, views, audio abstractions, SQLite repositories, and inference service models.

## Design and Architecture

### MVVM

- **Model/Service layer**: `IAudioRecorder` / `AudioRecorder` from `AudioManager`; `IRecordingSessionRepository` / `IApplicationSettingsRepository` from `DataRepository`.
- **ViewModel layer**: `RecorderViewModel`, `BasicSettingsViewModel`, `TranscribeMeetingViewModel`, and `MainWindowViewModel` expose UI state and commands.
- **View layer**: `MainWindow.xaml` hosts navigation; `Recorder`, `BasicSettings`, and `TranscribeMeeting` user controls bind to their respective view models.
- **Factories**: `ViewFactory` (singleton) and `ViewModelFactory` decouple view/view-model construction from navigation logic.

### Dependency direction

```text
Meeting_Recorder  →  AudioManager
                  →  DataRepository
                  →  Data
Meeting_Recorder.Tests  →  Meeting_Recorder
                        →  AudioManager
                        →  DataRepository
                        →  Data
                        →  InferenceService
InferenceService        (no internal project references)
Data            →  DataRepository
```

### Navigation

`MainWindowViewModel` uses `ViewFactory` and `ViewModelFactory` to create and display views. The `ViewType` enum gates which view/view-model pair is instantiated on navigation.

Current registered views:

| `ViewType`         | View                  | ViewModel                      |
|--------------------|-----------------------|-------------------------------|
| `Recorder`         | `Recorder.xaml`       | `RecorderViewModel`           |
| `TranscribeMeeting`| `TranscribeMeeting.xaml` | `TranscribeMeetingViewModel` |
| `BasicSettings`    | `BasicSettings.xaml`  | `BasicSettingsViewModel`      |

### Recording Session Persistence

When a recording starts, `RecorderViewModel` captures `DateTime.Now` as `RecordingStartedAt`. When recording stops, it saves an `IRecordingSession` (file path, file name, started-at timestamp) via `IRecordingSessionRepository`. The concrete implementation `SqliteRecordingSessionRepository` persists sessions to a `RecordingSession` table in the SQLite database.

## Audio Recording Pipeline

```text
Microphone (WasapiCapture)
  -> normalize to output format
Speaker/System audio (WasapiLoopbackCapture)
  -> normalize to output format
Both streams
  -> write samples to WaveFileWriter (.wav)
```

`AudioRecorder` uses:
- `WasapiCapture` for microphone input
- `WasapiLoopbackCapture` for speaker/system audio
- `WdlResamplingSampleProvider` for sample-rate conversion
- `WaveFileWriter` for output file persistence

## Inference Service

`InferenceService` provides a base class `LanguageModelBase` (using Microsoft Semantic Kernel) and two concrete implementations:

- **`LlamaModel`**: targets LM Studio or any OpenAI-compatible local endpoint; routes to `/v1/chat/completions`.
- **`OllamaModel`**: targets an Ollama endpoint using `OllamaSharp` registered via Semantic Kernel's dependency injection.

Both models maintain a `ChatHistory` and expose a chat-completion interface for future transcription and summarisation features.

## Package Management

Package versions are centrally managed in `Directory.Packages.props`.
All project files use `PackageReference` entries without inline versions.

Current centrally managed packages:

| Package | Version |
|---|---|
| `Microsoft.Data.Sqlite` | 10.0.7 |
| `Microsoft.NET.Test.Sdk` | 18.4.0 |
| `Microsoft.SemanticKernel` | 1.74.0 |
| `Moq` | 4.20.72 |
| `NAudio` | 2.3.0 |
| `OllamaSharp` | 5.4.25 |
| `xunit.runner.visualstudio` | 3.1.5 |
| `xunit.v3` | 3.2.2 |
| `xunit.v3.assert` | 3.2.2 |
| `Xunit.StaFact` | 3.0.13 |

## Technology Stack

- **.NET 10** (`net10.0-windows`)
- **WPF** desktop UI
- **NAudio** audio capture and WAV writing
- **Microsoft.Data.Sqlite** session and settings persistence
- **Microsoft Semantic Kernel** AI orchestration
- **OllamaSharp** Ollama LLM client
- **xUnit + Moq + Xunit.StaFact** testing stack

# Meeting Recorder Architecture

## Overview

Meeting Recorder is a WPF desktop application that records microphone input and system output audio into a single WAV file.
The solution is split into a UI project, an audio library project, and a test project.

## Solution Structure

```text
Meeting_Recorder/
├── AudioManager/
│   ├── AudioManager.csproj
│   ├── AudioRecorder.cs
│   ├── IAudioRecorder.cs
│   └── Recorder.cs
├── Meeting_Recorder.Tests/
│   ├── ViewModels/
│   │   └── RecorderViewModelTests.cs
│   ├── AudioRecorderTests.cs
│   ├── Meeting_Recorder.Tests.csproj
│   └── (obj/bin build output)
├── Resources/
│   └── Styles.xaml
├── ViewModels/
│   ├── ITimer.cs
│   ├── RecorderViewModel.cs
│   ├── RelayCommand.cs
│   └── ViewModelBase.cs
├── .github/
│   └── agents/
│       └── LatestPackageAgent.agent.md
├── App.xaml
├── App.xaml.cs
├── Architecture.md
├── AssemblyInfo.cs
├── Directory.Packages.props
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── Meeting_Recorder.csproj
├── Meeting_Recorder.slnx
└── nuget.config
```

## Project Responsibilities

- **Meeting_Recorder.csproj**: WPF application entry point and UI.
- **AudioManager/AudioManager.csproj**: Audio capture implementation using NAudio.
- **Meeting_Recorder.Tests/Meeting_Recorder.Tests.csproj**: Unit tests for ViewModel and audio abstractions.

## Design and Architecture

### MVVM

- **Model/Service layer**: `IAudioRecorder` and `AudioRecorder` from `AudioManager`.
- **ViewModel layer**: `RecorderViewModel` exposes UI state and commands.
- **View layer**: `MainWindow.xaml` binds to ViewModel properties and commands.

### Dependency direction

- `Meeting_Recorder` references `AudioManager`.
- `Meeting_Recorder.Tests` references both `Meeting_Recorder` and `AudioManager`.
- `RecorderViewModel` depends on `IAudioRecorder` (abstraction), not concrete implementation.

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

## Package Management

Package versions are centrally managed in `Directory.Packages.props`.
All project files use `PackageReference` entries without inline versions.

Current centrally managed packages:
- `Microsoft.NET.Test.Sdk` 18.4.0
- `Moq` 4.20.72
- `NAudio` 2.3.0
- `xunit.runner.visualstudio` 3.1.5
- `xunit.v3.assert` 3.2.2
- `Xunit.StaFact` 3.0.13

## Technology Stack

- **.NET 10** (`net10.0-windows`)
- **WPF** desktop UI
- **NAudio** audio capture and WAV writing
- **xUnit + Moq + Xunit.StaFact** testing stack

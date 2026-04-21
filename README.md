# Meeting Recorder

A .NET 10 WPF desktop application for recording microphone input and system audio into a single WAV file.

## Current Features

- Record from microphone and speaker/system output simultaneously
- Save recordings as `.wav` files
- Default output path generation in the user Documents folder
- Live elapsed time display while recording
- MVVM-based UI structure with command-driven actions
- Unit tests for audio abstraction and ViewModel behavior
- Centralized NuGet package version management via `Directory.Packages.props`

## Tech Stack

- .NET 10 (`net10.0-windows`)
- WPF
- NAudio
- xUnit, Moq, Xunit.StaFact

## Build

### Prerequisites

- Windows
- .NET 10 SDK
- Visual Studio 2026 (or `dotnet` CLI with WPF support)

### Using .NET CLI

```powershell
dotnet restore

dotnet build
```

## Run

### Using .NET CLI

```powershell
dotnet run --project .\Meeting_Recorder.csproj
```

### Using Visual Studio

1. Open `Meeting_Recorder.slnx`
2. Set `Meeting_Recorder` as startup project
3. Press `F5` or `Ctrl+F5`

## Test

```powershell
dotnet test
```

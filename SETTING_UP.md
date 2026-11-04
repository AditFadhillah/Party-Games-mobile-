# Setting Up Party Games

## Prerequisites

- .NET SDK 10.0 (10.0.302 or later)
- .NET MAUI workload
- Windows 10 (19041.0 or later) for the Windows target
- For Android: Android SDK, JDK 21, and an emulator (Pixel_10 or any API 36+ image)

JDK 25 is not supported by the Android build. Use JDK 21.

Check the setup:

```powershell
dotnet --version
dotnet workload list
```

`maui` should appear in the workload list. If it does not, run:

```powershell
dotnet workload restore
```

---

## Windows

### Build and run from a terminal

Run these from the repo root:

```powershell
dotnet build PartyGames\PartyGames.csproj -f net10.0-windows10.0.19041.0 -c Debug
.\PartyGames\bin\Debug\net10.0-windows10.0.19041.0\win-x64\PartyGames.exe
```

### Run from VS Code (F5)

1. Open the repo folder in VS Code.
2. Select `PartyGames` as the startup project (click the `{ }` item in the status bar, or run `.NET MAUI: Select Startup Project` from the Command Palette).
3. Select `Local Machine` as the debug target.
4. Press F5.

---

## Android

### Start an emulator

Either of these works:

- From a terminal (uses the default Android SDK location):

  ```powershell
  & "$env:LOCALAPPDATA\Android\Sdk\emulator\emulator.exe" -avd Pixel_10
  ```

  Keep this terminal open while you work.

- From Android Studio: open Device Manager and press the play button on the emulator. Android Studio does not need to stay open.

Confirm the emulator is connected:

```powershell
& "$env:LOCALAPPDATA\Android\Sdk\platform-tools\adb.exe" devices
```

It should list `emulator-5554   device`.

### Build and run from a terminal

```powershell
dotnet build -t:Run PartyGames\PartyGames.csproj -f net10.0-android -c Debug -p:JavaSdkDirectory="C:\Program Files\Microsoft\jdk-21.0.13.0.2-hotspot"
```

Change `JavaSdkDirectory` to your JDK 21 install path.

### Build and run from VS Code (F5)

1. Start the emulator (see above).
2. Select `PartyGames` as the startup project.
3. Select the running emulator (`Pixel_10`) as the debug target.
4. Press F5. The first launch takes about a minute.

### Android Studio

Android Studio does not build .NET MAUI projects. Use it only for the Android SDK and the emulator (Device Manager). Build and run with the terminal command or F5 in VS Code.

---

## Troubleshooting

**No startup project is set.** Select `PartyGames` in the MAUI status bar item or from the Command Palette, then try again.

**"Selected device is not running" (XA0010).** The debug target is an emulator that is not started. Start it, confirm it appears in `adb devices`, then select it again.

**Java SDK warning or error (XA5300, XA0030, XA0033).** Make sure `JavaSdkDirectory` points to JDK 21.

**Missing android.jar for API 36 (XA5207).** Install the matching platform in Android Studio's SDK Manager.

**Clean rebuild.**

```powershell
dotnet clean PartyGames\PartyGames.csproj
```

---

## Project Structure

```
PartyGames/
  Features/
    Main/          Game selection screen
    DiceRoller/    Dice roller (d4 to d20, up to 8 dice, history of 30 rolls)
    CoinFlip/      Coin flip
    Blackjack/     Placeholder
  ViewModels/      Shared base view model
  Services/        Game, score and navigation services
  AppShell.xaml    Navigation shell and routes
  MauiProgram.cs   Dependency injection and startup
```

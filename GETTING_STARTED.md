# Getting Started with Party Games

## Quick Start

### Prerequisites
- **.NET SDK 10.0.302+** ([Download](https://dotnet.microsoft.com/download))
- **.NET MAUI workload** installed for Windows
- **Windows 10** (19041.0 or later)

### Verify Prerequisites

```powershell
# Check .NET SDK version
dotnet --version

# Check installed workloads
dotnet workload list
```

You should see `maui` workload listed.

---

## Running the App

### Recommended: Build and Run Executable (Confirmed Working)

**If you're in the repo root:**
```powershell
cd PartyGames
dotnet build -f net10.0-windows10.0.19041.0 -c Debug
.\bin\Debug\net10.0-windows10.0.19041.0\win-x64\PartyGames.exe
```

**If you're already in the PartyGames folder:**
```powershell
dotnet build -f net10.0-windows10.0.19041.0 -c Debug
.\bin\Debug\net10.0-windows10.0.19041.0\win-x64\PartyGames.exe
```

### Quick Run (If Already Built)
```powershell
.\bin\Debug\net10.0-windows10.0.19041.0\win-x64\PartyGames.exe
```

### Build Only (Manual Run Later)
```powershell
dotnet build -f net10.0-windows10.0.19041.0 -c Debug
```

The executable will be at:
```
bin/Debug/net10.0-windows10.0.19041.0/win-x64/PartyGames.exe
```

---

## Testing Button Interaction

Once the app launches:

### Keyboard Test
- Press **Tab** to focus game buttons
- Press **Enter** to select a game

### Mouse Test
- **Click** directly on any game button (🎲 Dice Roller, 🃏 Blackjack, 🪙 Coin Flip)

Both interactions should navigate to the selected game page.

---

## Available Games

- **🎲 Dice Roller** - Roll two dice (2-12 range), track best roll and history
- **🃏 Blackjack** - Coming Soon in Phase 2
- **🪙 Coin Flip** - Predict heads/tails, track wins/losses/streak

---

## Troubleshooting

### App Won't Launch
```powershell
# Clean and rebuild
cd PartyGames
dotnet clean
dotnet build -f net10.0-windows10.0.19041.0 -c Debug
```

### Missing MAUI Workload
```powershell
dotnet workload restore
```

### Build Errors
Ensure your working directory is the `PartyGames` project folder (not the repo root):
```powershell
cd PartyGames
dotnet build -f net10.0-windows10.0.19041.0 -c Debug
```

---

## Development Workflow

### Hot Reload (Supported)
While the app is running, edit XAML files and save. Changes appear instantly without rebuilding.

### Debugging
Launch the app in VS Code debugger:
1. Set breakpoints in your code
2. Press **F5** (or select "Run and Debug")
3. Choose ".NET" launcher if prompted

---

## Project Structure

```
PartyGames/
├── MainPage.xaml          # Game selection menu
├── AppShell.xaml          # Navigation shell & routes
├── MauiProgram.cs         # Dependency injection & startup
├── Views/                 # All game page XAML files
├── ViewModels/            # MVVM logic & game state
├── Services/              # Navigation & game mechanics
├── Models/                # Data entities
└── bin/Debug/...          # Build output (executables)
```

---

## Next Steps

- ✅ Button interaction fixed (Navigation service rewritten)
- ✅ Game selection navigation working (Dice, Coin Flip, Blackjack)
- ✅ ViewModel dependency injection fixed
- ✅ InvertedBoolConverter added for XAML bindings
- ✅ Debug logging implemented (check C:\temp\partygames_debug.log)
- 🔄 Test Dice Roller and Coin Flip mechanics
- 📋 Phase 2: Implement Blackjack game logic
- 🔌 Phase 3: Android/iOS deployment

---

**Happy gaming! 🎮**

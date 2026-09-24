# Party Games MAUI Project

**Project Name:** PartyGames  
**Platform:** .NET MAUI (Android & iOS)  
**Target:** Cross-platform party game app with multiple mini-games  
**Created:** 2026-09-21

---

## 📱 Project Vision

A cross-platform mobile app (Android/iOS) that brings party games to life. Players can select from a collection of interactive games like dice rolling, blackjack, coin flips, and more. Each game includes score tracking, animations, and party-friendly UI. The app is designed for casual gatherings, team activities, and entertainment.

**Core Concept:** Simple, fun, quick-play games that work on any phone with instant setup (no multiplayer server needed initially).

---

## 🛠️ Technology Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| **Framework** | .NET MAUI | 9.0+ |
| **.NET Runtime** | .NET | 9.0+ |
| **Language** | C# | Latest |
| **UI** | XAML (MAUI controls) | Built-in |
| **Styling** | MAUI Styles / CSS-like | Built-in |
| **Data** | SQLite (local scores) | Optional |
| **APIs** | None (local-only MVP) | - |
| **Build Target** | Android 13+, iOS 14+ | Native |

**Why MAUI?**
- Single codebase for Android and iOS
- Native performance
- XAML for UI (familiar if you've used WPF/UWP)
- Strong tooling in Visual Studio
- Growing ecosystem

---

## 📂 Project Structure

```
PartyGames/
├── PartyGames.App/                # MAUI App (UI + Navigation)
│   ├── Views/
│   │   ├── MainPage.xaml          # Game selection screen
│   │   ├── DiceGamePage.xaml      # Dice game
│   │   ├── BlackjackPage.xaml     # Blackjack
│   │   ├── CoinFlipPage.xaml      # Coin flip
│   │   ├── ScoresPage.xaml        # High scores
│   │   └── SettingsPage.xaml      # Settings
│   ├── ViewModels/
│   │   ├── MainViewModel.cs
│   │   ├── DiceGameViewModel.cs
│   │   ├── BlackjackViewModel.cs
│   │   ├── CoinFlipViewModel.cs
│   │   └── ScoresViewModel.cs
│   ├── Models/
│   │   ├── Game.cs
│   │   ├── GameScore.cs
│   │   └── Player.cs
│   ├── Services/
│   │   ├── GameService.cs         # Game logic
│   │   ├── ScoreService.cs        # Score persistence
│   │   └── NavigationService.cs
│   ├── Resources/
│   │   ├── Styles/
│   │   │   └── Colors.xaml
│   │   ├── Images/
│   │   │   ├── dice-icon.png
│   │   │   ├── blackjack-icon.png
│   │   │   └── coin-icon.png
│   │   └── Fonts/
│   ├── App.xaml                   # App shell & resources
│   ├── AppShell.xaml              # Navigation structure
│   ├── MauiProgram.cs             # Dependency injection setup
│   └── Platforms/                 # Platform-specific code (Android/iOS)
│
├── PartyGames.Core/               # Shared game logic
│   ├── Games/
│   │   ├── DiceGame.cs
│   │   ├── BlackjackGame.cs
│   │   ├── CoinFlipGame.cs
│   │   └── IGame.cs               # Interface
│   └── Models/
│       ├── GameResult.cs
│       └── Player.cs
│
├── PartyGames.Tests/              # Unit tests
│   ├── DiceGameTests.cs
│   ├── BlackjackGameTests.cs
│   └── ScoreServiceTests.cs
│
└── README.md
```

---

## 🎮 Games (MVP - Phase 1)

### 1. **Dice Roller**
- **Description:** Shake 2 dice, see results
- **Features:**
  - Roll animation (shake-style)
  - Display 2 dice with pip graphics
  - Sum calculation
  - Roll history (last 5 rolls)
  - Leaderboard (highest roll of session)

### 2. **Blackjack (Simplified)**
- **Description:** Player vs dealer blackjack game
- **Features:**
  - Single deck
  - Hit/Stand/Double Down buttons
  - Card graphics or text display
  - Win/Loss/Tie logic
  - Running score per game round
  - Session stats (wins/losses)

### 3. **Coin Flip**
- **Description:** Flip a coin, heads or tails
- **Features:**
  - Heads/Tails prediction
  - Flip animation
  - Win/Loss tracking
  - Streak counter
  - Sound effects (optional)

### Phase 2 (Future)
- Uno card game
- Trivia mode
- Spin the wheel
- Rock-Paper-Scissors vs AI

---

## 🎯 Key Features

### Core Functionality
1. **Game Selection Screen** - Show all available games with icons and descriptions
2. **Game Play Screen** - Dynamic UI based on selected game
3. **Score/Results Screen** - Show session results, streaks, stats
4. **Navigation** - Fluent back/next between games
5. **Persistent Scores** - Save session data locally (SQLite optional)
6. **Theme Support** - Light/dark mode toggle

### UX Requirements
- **Responsive Design** - Works on phones (5-7"), tablets (10")
- **Touch-Friendly** - Large buttons, clear hit zones
- **Fast** - No loading delays, instant feedback
- **Animations** - Dice rolling, card dealing, coin spinning
- **Sound** - Optional SFX toggles
- **Party Mode** - Display winner/loser prominently on big screens

---

## 🏗️ Architecture Decisions

### MVVM Pattern
- **ViewModel:** Contains game logic, player state, score tracking
- **View:** XAML, binds to ViewModel properties
- **Model:** Game entities, scores, player data
- **Separation:** UI logic separate from game logic (testable)

### Dependency Injection
```csharp
// MauiProgram.cs
builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<ScoreService>();
builder.Services.AddSingleton<MainPage>();
builder.Services.AddSingleton<MainViewModel>();
```

### Data Persistence (Optional MVP)
- **SQLite:** Store high scores locally
- **File Storage:** Save/load game sessions
- **User Preferences:** Theme, sound settings

### No Backend (MVP)
- All data local to device
- No authentication needed
- No cloud sync (can add later)

---

## 📋 API Endpoints (N/A for MVP)

This is a local-only app initially. No server required.

**Future (if multiplayer is added):**
- `POST /games/{gameId}/score` - Submit score
- `GET /leaderboard` - Get global scores
- `POST /sessions/create` - Start multiplayer session

---

## 🎨 UI/UX Overview

### Main Screens

#### 1. Main Menu
```
┌─────────────────────────┐
│     PARTY GAMES         │
├─────────────────────────┤
│  ┌─────────────────┐    │
│  │  🎲 Dice Game  │    │
│  └─────────────────┘    │
│                          │
│  ┌─────────────────┐    │
│  │  ♠️ Blackjack   │    │
│  └─────────────────┘    │
│                          │
│  ┌─────────────────┐    │
│  │  🪙 Coin Flip   │    │
│  └─────────────────┘    │
│                          │
│  [ Settings ] [ Scores] │
└─────────────────────────┘
```

#### 2. Dice Game
```
┌─────────────────────────┐
│    DICE ROLLER          │
├─────────────────────────┤
│                          │
│      [🎲]  [🎲]        │
│       3      5           │
│      Total: 8           │
│                          │
│   ┌──────────────────┐  │
│   │  ROLL DICE (TAP) │  │
│   └──────────────────┘  │
│                          │
│  Last Rolls: 6,7,4      │
│  Best: 12               │
│                          │
│  [ Back ] [ Leaderboard]│
└─────────────────────────┘
```

#### 3. Blackjack
```
┌─────────────────────────┐
│    BLACKJACK            │
├─────────────────────────┤
│  Dealer: [K♠] [?]      │
│  (Score: 10+)           │
│                          │
│  You: [7♥] [8♦] [3♣]   │
│  (Score: 18)            │
│                          │
│  ┌──────────┬─────────┐ │
│  │   HIT    │  STAND  │ │
│  └──────────┴─────────┘ │
│                          │
│  [ Back ]  Session: 2W-1L
└─────────────────────────┘
```

---

## 🚀 Setup & Implementation Phases

### Phase 0: Project Setup
**Time:** 1-2 hours
1. Create MAUI project in Visual Studio
2. Install dependencies (.NET MAUI SDK, templates)
3. Configure Android/iOS build settings
4. Test basic "Hello World" on emulator
5. Set up Git repo

### Phase 1: Core App & Dice Game
**Time:** 4-6 hours
1. Create AppShell navigation structure
2. Build MainPage (game selection)
3. Implement DiceGame logic + ViewModel
4. Build DiceGamePage UI with animations
5. Add score tracking (in-memory)
6. Test on Android emulator

### Phase 2: Blackjack & Coin Flip
**Time:** 4-6 hours
1. Implement BlackjackGame logic
2. Build BlackjackPage UI (dealer/player cards)
3. Implement CoinFlipGame logic
4. Build CoinFlipPage UI
5. Add session statistics screen
6. Test both games

### Phase 3: Polish & Persistence
**Time:** 2-4 hours
1. Add SQLite for score persistence
2. Implement high scores page
3. Add theme toggle (light/dark)
4. Add sound effects (optional)
5. Optimize animations
6. Build release APK/IPA

---

## 📦 Dependencies

### NuGet Packages (Core)
```xml
<!-- MauiProgram.cs will auto-include MAUI essentials -->
Microsoft.Maui
Microsoft.Maui.Controls
Microsoft.Extensions.DependencyInjection

<!-- Optional: Persistence -->
sqlite-net-pcl
```

### Platform-Specific
- **Android:** Android 13+ SDK
- **iOS:** Xcode 14+, iOS 14+ SDK

### System Requirements
- **OS:** Windows 10/11 (Windows) or macOS (for iOS)
- **.NET SDK:** 9.0+
- **Visual Studio:** 2022 Community (or higher)
- **Android Emulator:** (or physical device)
- **iOS Simulator:** Requires macOS

---

## 🎮 Game Logic Pseudocode

### Dice Game
```csharp
public class DiceGame
{
    public int Roll()
    {
        Random rand = new Random();
        int die1 = rand.Next(1, 7);
        int die2 = rand.Next(1, 7);
        return die1 + die2;
    }
    
    public void RecordRoll(int result)
    {
        rollHistory.Add(result);
        if (result > bestRoll) bestRoll = result;
    }
}
```

### Blackjack Game
```csharp
public class BlackjackGame
{
    private List<Card> dealerHand;
    private List<Card> playerHand;
    
    public void Hit(Player player) => player.Hand.Add(deck.DrawCard());
    
    public int CalculateScore(List<Card> hand)
    {
        int score = hand.Sum(c => c.Value);
        // Handle Aces (11 or 1)
        return score > 21 ? 0 : score; // Bust
    }
    
    public GameResult DeterminWinner()
    {
        int dealerScore = CalculateScore(dealerHand);
        int playerScore = CalculateScore(playerHand);
        
        if (playerScore > dealerScore) return GameResult.Win;
        if (playerScore < dealerScore) return GameResult.Loss;
        return GameResult.Tie;
    }
}
```

---

## 🧪 Testing Strategy

### Unit Tests (C#)
- DiceGame.Roll() produces values 2-12
- BlackjackGame.CalculateScore() handles Aces correctly
- CoinFlipGame probability (heads ~50%)
- ScoreService persists/retrieves scores

### UI Tests (Manual for MVP)
- Games load without crashes
- Buttons respond to taps
- Animations are smooth
- Scores display correctly
- Navigation works (back button, game selection)

### Device Testing
- Test on Android emulator (Pixel 5 recommended)
- Test on iOS simulator (iPhone 15 recommended)
- Test on physical devices (1 Android, 1 iOS if available)

---

## 📱 Phone Simulation & Testing

### Built-in Emulators
1. **Android Emulator** (Windows/Mac/Linux)
   - Included with Android SDK
   - Slow but accurate
   - Setup: Visual Studio > Tools > Android Device Manager

2. **iOS Simulator** (Mac only)
   - Included with Xcode
   - Fast, accurate
   - Setup: Xcode > Preferences > Components

### Alternative Tools
- **MAUI Preview** (Visual Studio 2022+) - Hot reload preview
- **Physical Device Testing** - Best for animations/performance
- **Remote Mobile Emulator** (Microsoft App Center) - Cloud-based testing

### Quick Start: Android Emulator
```bash
# In Visual Studio
# 1. Tools > Android Device Manager
# 2. Click "Create Device"
# 3. Select "Pixel 5" template, Android 13+
# 4. Press Play (start emulator)
# 5. Build > Deploy to emulator
```

---

## 🔧 Prerequisites for Development

### Install
1. **Visual Studio 2022 Community** (free)
   - Desktop Development with C# workload
   - .NET MAUI workload

2. **.NET 9.0+ SDK**
   ```bash
   dotnet --version
   ```

3. **Android SDK**
   - Visual Studio installer → Mobile Development with .NET
   - Includes emulator + SDKs

4. **Optional: Xcode** (macOS only, for iOS)
   ```bash
   brew install xcode-command-line-tools
   ```

---

## 🚀 Getting Started (Step-by-Step)

### 1. Create Project
```bash
dotnet new maui -n PartyGames
cd PartyGames
```

### 2. Open in Visual Studio
```bash
# Visual Studio will auto-detect and load MAUI project
# NuGet packages auto-restore
```

### 3. Configure Android Emulator
- Tools > Android Device Manager > Create Device
- Start the emulator

### 4. Build & Run
```bash
# Visual Studio: Select device, press F5
# Terminal:
dotnet build -t Run -f net9.0-android
```

### 5. Test Dice Game
- See MainPage with game options
- Click Dice Game
- Tap "ROLL DICE" button
- Verify dice animation and result display

---

## 📝 File Example: DiceGameViewModel.cs

```csharp
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PartyGames.ViewModels
{
    public class DiceGameViewModel : BaseViewModel
    {
        private int die1, die2, total, bestRoll;
        private ObservableCollection<int> rollHistory;
        
        public ICommand RollCommand { get; }
        
        public DiceGameViewModel()
        {
            rollHistory = new ObservableCollection<int>();
            RollCommand = new Command(OnRoll);
        }
        
        private void OnRoll()
        {
            Random rand = new Random();
            Die1 = rand.Next(1, 7);
            Die2 = rand.Next(1, 7);
            Total = Die1 + Die2;
            
            if (Total > BestRoll) BestRoll = Total;
            rollHistory.Add(Total);
            
            OnPropertyChanged();
        }
        
        public int Die1 { get => die1; set => SetProperty(ref die1, value); }
        public int Die2 { get => die2; set => SetProperty(ref die2, value); }
        public int Total { get => total; set => SetProperty(ref total, value); }
        public int BestRoll { get => bestRoll; set => SetProperty(ref bestRoll, value); }
        public ObservableCollection<int> RollHistory => rollHistory;
    }
}
```

---

## 🎯 Success Criteria

- ✅ All 3 games playable (Dice, Blackjack, Coin Flip)
- ✅ Runs on Android emulator without crashes
- ✅ Responsive UI, no freezing
- ✅ Scores persist during session
- ✅ Navigation smooth (back button works)
- ✅ Animations are visible and fluid
- ✅ Code is testable (unit tests pass)

---

## 📚 Resources

- [Microsoft MAUI Docs](https://learn.microsoft.com/en-us/dotnet/maui/)
- [MAUI Samples](https://github.com/dotnet/maui-samples)
- [XAML Reference](https://learn.microsoft.com/en-us/dotnet/maui/xaml/)
- [Unit Testing in .NET](https://learn.microsoft.com/en-us/dotnet/core/testing/)
- [Dependency Injection](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/dependency-injection)

---

## 📞 Next Steps

1. Review this spec with team/reviewer
2. Set up development environment (Visual Studio + Android SDK)
3. Create MAUI project from template
4. Start Phase 0 (project setup)
5. Implement DiceGame first (quickest win)
6. Build Blackjack + Coin Flip
7. Add persistence + polish
8. Deploy APK/IPA for testing

---

**Created:** 2026-09-21  
**Status:** Ready for development  
**Last Updated:** 2026-09-21

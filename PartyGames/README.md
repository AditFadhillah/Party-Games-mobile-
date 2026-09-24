# Party Games MAUI 🎮

A cross-platform mobile party game app built with **.NET MAUI** for Android and iOS. Play quick, fun games like Dice Rolling, Coin Flip, and Blackjack!

## 📱 Features

- **🎲 Dice Roller** - Roll two dice, track your best score
- **🪙 Coin Flip** - Call heads or tails, build your streak
- **♠️ Blackjack** - (Coming in Phase 2) Classic card game vs dealer
- **📊 Score Tracking** - In-memory session scores
- **🎨 Responsive UI** - Works on phones and tablets
- **⚡ Hot Reload** - Instant UI updates during development
- **🏗️ MVVM Architecture** - Clean, testable code

## 🛠️ Tech Stack

| Component | Technology |
|-----------|-----------|
| **Framework** | .NET MAUI 10.0 |
| **Language** | C# |
| **UI** | XAML |
| **Platform** | Android 13+, iOS 14+ |
| **Architecture** | MVVM + Dependency Injection |
| **Data** | In-memory (SQLite ready) |

## 🚀 Quick Start

### Prerequisites
- **.NET 10.0+ SDK** ([Download](https://dotnet.microsoft.com/download))
- **Visual Studio 2022** Community or higher
- **MAUI Workload** (auto-installed during setup)

### Setup (5 minutes)

```bash
# Clone/navigate to project
cd c:\Users\fadhi\PROJECTS\Party-Games-mobile\PartyGames

# Restore workloads
dotnet workload restore

# Run with Android Emulator
dotnet maui run -f net10.0-android --verbose
```

### Using Hot Reload (Recommended)

1. **Open in Visual Studio 2022**
   - File → Open Folder → Select PartyGames folder

2. **Start Debugging** (F5)
   - Selects Android Emulator automatically
   - App builds and deploys to emulator

3. **Edit & See Live Changes**
   - Open `Views/MainPage.xaml`
   - Change any label text or color
   - Press Ctrl+S
   - **Changes appear instantly on device!** 🎉

For detailed Hot Reload guide, see [HOT_RELOAD_GUIDE.md](HOT_RELOAD_GUIDE.md)

## 📂 Project Structure

```
PartyGames/
├── Views/                    # XAML UI pages
│   ├── MainPage.xaml        # Game menu
│   ├── DiceGamePage.xaml    # Dice game
│   └── CoinFlipPage.xaml    # Coin flip game
├── ViewModels/               # Business logic & data binding
│   ├── BaseViewModel.cs     # MVVM base class
│   ├── MainViewModel.cs     # Menu logic
│   ├── DiceGameViewModel.cs # Dice game logic
│   └── CoinFlipViewModel.cs # Coin flip logic
├── Models/                   # Data entities
│   ├── Game.cs              # Game info
│   ├── GameScore.cs         # Score record
│   ├── GameResult.cs        # Game outcome
│   └── Player.cs            # Player info
├── Services/                 # App services
│   ├── GameService.cs       # Game operations
│   ├── ScoreService.cs      # Score management
│   └── NavigationService.cs # Navigation logic
├── Resources/                # App assets
│   ├── Styles/              # XAML styles
│   ├── Images/              # Game icons
│   └── Fonts/               # Custom fonts
├── MauiProgram.cs           # DI configuration
├── App.xaml                 # App resources
├── AppShell.xaml            # Navigation routes
└── HOT_RELOAD_GUIDE.md      # Hot Reload setup
```

## 🎮 Playing Games

### Dice Roller
- Tap the dice to roll
- Watch real-time animation
- Track your best roll
- See roll history (last 5)

### Coin Flip
- Choose Heads or Tails
- Watch coin flip animation
- Win/Loss automatically tracked
- Streak counter for consecutive wins

### Blackjack (Coming Soon)
- Player vs Dealer gameplay
- Hit/Stand/Double Down
- Score tracking
- Session statistics

## 🔧 Building & Deployment

### Build for Android
```bash
# Debug APK
dotnet build -f net10.0-android

# Release APK
dotnet publish -f net10.0-android -c Release
```

### Build for iOS (Mac only)
```bash
dotnet build -f net10.0-ios
```

### Clean & Rebuild
```bash
dotnet clean
dotnet workload restore
dotnet build -f net10.0-android
```

## 🧪 Testing

### Run Unit Tests
```bash
dotnet test
```

### Test on Device
1. Connect Android device via USB
2. Enable Developer Mode & USB Debugging
3. Select device from Visual Studio dropdown
4. Press F5

### Test on Emulator
1. Create virtual device (Android Device Manager)
2. Start emulator
3. Visual Studio auto-detects and deploys
4. Press F5

## 📊 Development Phases

### Phase 0: ✅ Project Setup (Complete)
- Project structure created
- MVVM infrastructure ready
- Dice and Coin games implemented
- Navigation configured

### Phase 1: 🔄 Polish & Testing (Current)
- Dice game animations
- Coin flip animations
- Sound effects
- Device testing

### Phase 2: 📋 Extended Games
- Blackjack implementation
- Uno card game
- Trivia mode
- Leaderboard with SQLite

## 🐛 Troubleshooting

| Problem | Solution |
|---------|----------|
| Workload not installed | Run `dotnet workload restore` |
| Emulator won't start | Check virtualization in BIOS, use physical device |
| Hot Reload not working | Restart debug session, check debug mode enabled |
| Build fails | Run `dotnet nuget locals all --clear` then `dotnet build` |

For more help, see [HOT_RELOAD_GUIDE.md](HOT_RELOAD_GUIDE.md) Troubleshooting section.

## 📚 Learning Resources

- **MAUI Docs**: https://learn.microsoft.com/en-us/dotnet/maui/
- **XAML Guide**: https://learn.microsoft.com/en-us/dotnet/maui/xaml/
- **Hot Reload**: https://learn.microsoft.com/en-us/dotnet/maui/xaml/hot-reload
- **Data Binding**: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/data-binding/
- **MAUI Samples**: https://github.com/dotnet/maui-samples

## 🎯 Success Criteria

- ✅ Runs on Android emulator without crashes
- ✅ Dice game fully playable with animations
- ✅ Coin flip fully playable with streak tracking
- ✅ Hot Reload works for instant UI updates
- ✅ Responsive design works on different screen sizes
- ✅ Score tracking persists during session
- ✅ Clean MVVM architecture

## 📝 Git Workflow

```bash
# Create feature branch
git checkout -b feature/dice-animation

# Make changes and test with Hot Reload

# Commit
git add .
git commit -m "feat: add dice roll animation"

# Push
git push origin feature/dice-animation

# Create pull request
```

## 🎨 Customization

### Change App Theme
Edit `Resources/Styles/Colors.xaml` and update primary/secondary colors.

### Add Custom Fonts
1. Add `.ttf` files to `Resources/Fonts/`
2. Register in `MauiProgram.cs`
3. Use in XAML: `FontFamily="MyFont"`

### Modify Game Logic
- Dice: `ViewModels/DiceGameViewModel.cs`
- Coin: `ViewModels/CoinFlipViewModel.cs`
- Services: `Services/GameService.cs`

## 📞 Support

For issues or questions:
1. Check [HOT_RELOAD_GUIDE.md](HOT_RELOAD_GUIDE.md)
2. Review the inline code comments
3. Check .NET MAUI official documentation
4. Test with Hot Reload to isolate issues

---

**Created**: 2026-09-21  
**Last Updated**: 2026-09-23  
**Status**: 🟢 Ready for Development with Hot Reload Preview

**Next Action**: Open in Visual Studio 2022 and press F5 to start! 🚀

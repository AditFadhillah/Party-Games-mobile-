# Party Games MAUI - Setup & Hot Reload Guide

## ✅ What Has Been Set Up

### Project Structure Created
```
PartyGames/
├── Views/
│   ├── MainPage.xaml/.cs         # Game selection screen
│   ├── DiceGamePage.xaml/.cs     # Dice game
│   ├── CoinFlipPage.xaml/.cs     # Coin flip game
│   └── BlackjackPage.xaml/.cs    # Coming soon placeholder
├── ViewModels/
│   ├── BaseViewModel.cs          # Base class with INotifyPropertyChanged
│   ├── MainViewModel.cs          # Game selection logic
│   ├── DiceGameViewModel.cs      # Dice game logic
│   └── CoinFlipViewModel.cs      # Coin flip game logic
├── Models/
│   ├── Player.cs                 # Player entity
│   ├── GameScore.cs              # Score entity
│   ├── Game.cs                   # Game metadata
│   └── GameResult.cs             # Game result entity
├── Services/
│   ├── GameService.cs            # Game logic and operations
│   ├── ScoreService.cs           # Score persistence
│   └── NavigationService.cs      # App navigation
├── Resources/
│   ├── Styles/                   # XAML styling
│   ├── Images/                   # Game assets
│   └── Fonts/                    # Custom fonts
├── AppShell.xaml                 # Navigation shell with routes
├── MauiProgram.cs                # Dependency injection setup
└── App.xaml                      # App-level resources
```

### Features Implemented
- ✅ **MVVM Architecture** - Clean separation of concerns
- ✅ **Dependency Injection** - Service registration in MauiProgram
- ✅ **Data Binding** - Full XAML binding support
- ✅ **Navigation Service** - Routing between game pages
- ✅ **Dice Game** - Roll animation with score tracking
- ✅ **Coin Flip Game** - Prediction-based gameplay with streak tracking
- ✅ **Score Service** - In-memory score management (SQLite ready)
- ✅ **Responsive UI** - Touch-friendly buttons and layouts

---

## 🚀 Using MAUI Hot Reload Preview

### What is Hot Reload Preview?
MAUI Hot Reload Preview lets you see UI changes instantly without rebuilding the app. This is essential for rapid UI development and testing.

### Prerequisites
1. **.NET 10.0+ SDK** ✅ (Already installed - version 10.0.302)
2. **Visual Studio 2022** (Community or higher)
3. **MAUI Workloads** (Being installed automatically)

### Method 1: Using Visual Studio 2022 (Recommended)

#### Step 1: Open the Project
1. Open Visual Studio 2022
2. File → Open → Folder
3. Navigate to `c:\Users\fadhi\PROJECTS\Party-Games-mobile\PartyGames`
4. Wait for the solution to load and NuGet packages to restore

#### Step 2: Configure Debug Target
1. In the toolbar, locate the **Debug Target** dropdown (currently says "net10.0-android")
2. Select **"Android Emulator"** or **"Physical Device"**
   - For emulator: Device Manager must have an emulator created
   - For device: USB debugging enabled

#### Step 3: Enable Hot Reload
1. Debug → Windows → Hot Reload (or Ctrl+Alt+F)
2. Hot Reload panel opens on the right side
3. Start debugging (F5)

#### Step 4: Make Changes & See Live Updates
1. Edit any XAML file (e.g., MainPage.xaml)
2. Change a Label text or Button color
3. Save (Ctrl+S)
4. **Hot Reload automatically applies changes** - no rebuild needed!

### Method 2: Command Line Hot Reload

```bash
cd c:\Users\fadhi\PROJECTS\Party-Games-mobile\PartyGames

# For Android Emulator
dotnet maui run -f net10.0-android --verbose

# For iOS Simulator (Mac only)
dotnet maui run -f net10.0-ios --verbose

# With hot reload enabled
dotnet maui run -f net10.0-android --no-build
```

### Method 3: Using Visual Studio Code

1. Install C# Dev Kit extension
2. Open the project folder
3. Run in terminal:
   ```bash
   dotnet maui run -f net10.0-android
   ```

---

## 📱 Setting Up Android Emulator

### Quick Setup (Recommended Device: Pixel 5)
1. **Open Android Device Manager**
   - Visual Studio: Tools → Android → Android Device Manager

2. **Create Virtual Device**
   - Click "Create Device"
   - Select "Pixel 5" template
   - Android OS Version: **API 35** (Android 15) or API 34
   - RAM: 4GB minimum
   - Storage: 32GB minimum
   - Click "Create"

3. **Start the Emulator**
   - In Device Manager, click Play button
   - Wait for boot (1-2 minutes on first run)

4. **Deploy App**
   - Visual Studio: Select emulator from dropdown
   - Press F5 to build and run
   - App appears in emulator automatically

### Emulator Performance Tips
- Use **Android 14+ (API 34-35)** for better performance
- Allocate 4-6 GB RAM to emulator
- Enable "Use Host GPU" in emulator settings
- Close other apps to free resources

---

## 🎮 Testing Each Game

### Dice Roller
1. Click "Dice Roller" from main menu
2. Tap "🎲 ROLL DICE" button
3. Dice animate and show result
4. Best roll tracks highest value
5. Last rolls displays history

### Coin Flip
1. Click "Coin Flip" from main menu
2. Choose "HEADS" or "TAILS"
3. Coin animates and shows result
4. Win/Loss/Streak updates display
5. Can play multiple rounds

### Hot Reload During Testing
- **Edit Button Text**: Change `Text="ROLL DICE"` in DiceGamePage.xaml → Save → See instant update
- **Change Colors**: Modify `BackgroundColor="#FF9500"` → Save → Instant visual feedback
- **Adjust Layouts**: Change `Padding="20"` values → Immediate layout update
- **Update Data**: Modify labels → See binding updates instantly

---

## 🔧 Troubleshooting

### Issue: "Workloads not installed" Error
**Solution:**
```bash
cd c:\Users\fadhi\PROJECTS\Party-Games-mobile\PartyGames
dotnet workload restore
# Or specific workload:
dotnet workload install maui-android
```

### Issue: Emulator Won't Start
**Solution:**
1. Check virtualization enabled in BIOS
2. Restart Android Device Manager
3. Use alternative emulator (Genymotion) or physical device

### Issue: Hot Reload Not Working
**Solution:**
1. Make sure you're in Debug mode (not Release)
2. Supported file types: XAML, C# ViewModels, Resource files
3. Some code changes require rebuild (adding new controls)
4. Restart debugging session if necessary

### Issue: Build Fails with SDK Errors
**Solution:**
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore workloads
dotnet workload restore

# Clean and rebuild
dotnet clean
dotnet build
```

---

## 📝 Next Steps for Development

### Phase 1 (Current - MVP)
- ✅ Dice Roller (Complete)
- ✅ Coin Flip (Complete)
- ⏳ Blackjack (Placeholder)
- [ ] Polish animations
- [ ] Add sound effects
- [ ] Test on multiple devices

### Phase 2 (Future)
- Implement full Blackjack game
- Add Uno card game
- Implement Trivia mode
- Add SQLite persistence
- Implement leaderboard
- Add theme switching

---

## 🎯 Quick Commands Reference

```bash
# Build for Android
dotnet build -f net10.0-android

# Run with hot reload
dotnet maui run -f net10.0-android --verbose

# Clean build
dotnet clean && dotnet build -f net10.0-android

# Create release APK
dotnet publish -f net10.0-android -c Release

# Run tests
dotnet test

# Install workload
dotnet workload install maui-android
```

---

## 📚 Useful Resources

- **MAUI Official Docs**: https://learn.microsoft.com/en-us/dotnet/maui/
- **Hot Reload Guide**: https://learn.microsoft.com/en-us/dotnet/maui/xaml/hot-reload
- **XAML Reference**: https://learn.microsoft.com/en-us/dotnet/maui/xaml/
- **Data Binding**: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/data-binding/
- **Android Emulator**: https://learn.microsoft.com/en-us/dotnet/maui/android/emulator/

---

## 🎨 Customization

### Changing Colors
Edit `Resources/Styles/Colors.xaml` or inline in XAML:
```xaml
BackgroundColor="#F5F5F5"  <!-- Light gray -->
TextColor="#333333"         <!-- Dark text -->
BorderColor="#E0E0E0"       <!-- Light border -->
```

### Adding Custom Fonts
1. Add font files to `Resources/Fonts/`
2. Register in MauiProgram.cs:
   ```csharp
   .ConfigureFonts(fonts => {
       fonts.AddFont("MyFont.ttf", "MyFont");
   });
   ```

### Adjusting Layouts
- `Padding="20"` - Inner spacing
- `Margin="0,20,0,10"` - Outer spacing
- `Spacing="15"` - Space between children
- `HorizontalOptions="FillAndExpand"` - Size control

---

**Status**: Ready for development with Hot Reload! 🚀
**Last Updated**: 2026-09-23

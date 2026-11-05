# Party Games

A cross-platform party game app built with .NET MAUI for Android and Windows.

## Games

- **Dice Roller:** Choose d4, d6, d8, d10, d12 or d20 and roll up to 8 dice at once. Roll history keeps the last 30 rolls.
- **Coin Flip:** Flip a coin.
- **Tic-Tac-Toe:** Play against a friend or the CPU (Easy, Medium or Hard). Pick X or O for the CPU mode; X always goes first. Scores are tracked for X and O.
- **Blackjack:** In progress.

## Requirements

- .NET 10 SDK
- .NET MAUI workload
- Android SDK for Android builds
- JDK 21 (JDK 25 is not supported)

## Build and Run

Android:

```
dotnet build -t:Run -f net10.0-android -c Debug -p:JavaSdkDirectory="<path-to-jdk-21>"
```

Windows:

```
dotnet build PartyGames\PartyGames.csproj -f net10.0-windows10.0.19041.0 -c Debug
```

## Project Structure

```
PartyGames/
  Features/      Each game in its own folder (Main, DiceRoller, CoinFlip, TicTacToe, Blackjack)
  ViewModels/    Shared base view model
  Services/      Game, score and navigation services
```

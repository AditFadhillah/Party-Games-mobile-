# Party Games TODO

## Tic-Tac-Toe
- [x] Create `TicTacToe` folder with ViewModel and Page, following the Blackjack structure
- [x] Build a 3x3 board with `Border` cells and a tap command per cell
- [x] Track current player (X/O) and show a turn label
- [x] Detect win (rows, columns, diagonals) and draw
- [x] Highlight the winning line and show a result popup
- [x] Add a "New Game" button and a score tally for X and O
- [x] Register the page and ViewModel in `MauiProgram.cs` and add a menu entry
- [x] Optional: CPU opponent using minimax

## Higher or Lower
- [x] Create `HigherLower` folder with ViewModel and Page
- [x] Reuse `Card` and the card template to show the current card
- [x] Add Higher and Lower buttons; reveal the next card after each guess
- [x] Handle ties (a tie counts as a wrong guess)
- [x] Two-player scoring: a wrong guess awards the point to the other player
- [x] Use one shuffled 52-card deck; the game ends when the deck is empty
- [x] Register in `MauiProgram.cs` and add a menu entry

## Memory Match with Timer
- [ ] Create `MemoryMatch` folder with ViewModel and Page
- [ ] Build a card grid (for example 4x4) of face-down `Border` tiles
- [ ] Flip two tiles per turn and compare; hide again on mismatch after a short delay
- [ ] Shuffle pairs at the start of each game
- [ ] Add a timer using `IDispatcherTimer`, started on the first flip and stopped on completion
- [ ] Show elapsed time and a win popup with the final time
- [ ] Add a best-time record saved with `Preferences`
- [ ] Add a flip animation (`ScaleToAsync` or `RotateYTo`)
- [ ] Ensure the timer stops and disposes when leaving the page
- [ ] Register in `MauiProgram.cs` and add a menu entry

## Hangman with Typeable Word
- [ ] Create `Hangman` folder with ViewModel and Page
- [ ] Load a word list (embedded text or a `List<string>` in code)
- [ ] Display masked word with underscores for unguessed letters
- [ ] Add an `Entry` for typing a letter; accept only one letter A-Z, then submit on Enter or a Guess button
- [ ] Reject repeated letters and non-letter input with a short status message
- [ ] Track wrong guesses (for example 6 max) and draw the hangman parts with `Ellipse`, `Line`, or `Path` shapes
- [ ] Show win and lose states with the answer revealed on loss
- [ ] Add a "New Word" button
- [ ] Register in `MauiProgram.cs` and add a menu entry

## Shared Steps (all games)
- [ ] Add each game to the main menu
- [ ] Build on Windows and launch on the Pixel_10 emulator to verify each screen
- [ ] Commit and push per game, keeping only source files staged

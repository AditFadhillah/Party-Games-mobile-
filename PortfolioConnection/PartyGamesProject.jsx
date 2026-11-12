import { ArrowLeft, Github } from "lucide-react";
import { Link } from "react-router-dom";
import { ThemeToggle } from "../components/ThemeToggle";

const games = [
  {
    name: "Dice Roller",
    description:
      "Roll up to eight dice with a choice of six-sided and polyhedral dice, plus a recent-roll history.",
  },
  {
    name: "Coin Flip",
    description:
      "Flip a coin with an animated result and keep track of recent flips.",
  },
  {
    name: "Tic-Tac-Toe",
    description:
      "Play locally with a friend or challenge the CPU across three difficulty levels.",
  },
  {
    name: "Higher or Lower",
    description:
      "Take turns guessing the next card from a shuffled deck and compete for points.",
  },
  {
    name: "Higher or Lower +",
    description:
      "A party-friendly variation that turns card differences into sips.",
  },
  {
    name: "Blackjack",
    description:
      "Play against the dealer with betting, hit, stand, double down, and split actions.",
  },
];

const ScreenshotPlaceholder = ({ label }) => (
  <div
    role="img"
    aria-label={`${label} screenshot placeholder`}
    className="flex aspect-[4/3] w-full items-center justify-center rounded-xl border-2 border-dashed border-primary/40 bg-card p-6 text-center"
  >
    <div>
      <p className="font-semibold">Screenshot Placeholder</p>
      <p className="mt-2 text-sm text-muted-foreground">{label}</p>
      <p className="mt-1 text-xs text-muted-foreground">
        Game screenshots will be added later.
      </p>
    </div>
  </div>
);

export const PartyGamesProject = () => {
  return (
    <div className="min-h-screen bg-background px-4 py-12 text-foreground">
      <ThemeToggle />
      <div className="container mx-auto max-w-4xl space-y-12 text-left">
        <Link
          to="/"
          className="inline-flex items-center gap-2 text-primary transition-opacity hover:opacity-80"
        >
          <ArrowLeft size={18} />
          Back to Portfolio
        </Link>

        <section className="space-y-5">
          <div className="flex flex-wrap items-center gap-3">
            <p className="text-sm font-semibold uppercase tracking-wide text-primary">
              Personal Project
            </p>
            <span className="rounded-full border border-primary/40 bg-primary/10 px-3 py-1 text-sm font-medium text-primary">
              Ongoing
            </span>
          </div>
          <h1 className="text-4xl font-bold leading-tight md:text-5xl">
            Party Games
          </h1>
          <p className="text-xl text-muted-foreground">
            A collection of quick, local multiplayer games for sharing with
            friends.
          </p>
          <p className="text-primary">
            .NET MAUI, C#, XAML, MVVM, Dependency Injection
          </p>

          <div className="flex flex-wrap gap-4 pt-2">
            <a
              href="https://github.com/AditFadhillah/Party-Games-mobile-"
              target="_blank"
              rel="noopener noreferrer"
              className="cosmic-button inline-flex items-center gap-2"
            >
              <Github size={18} />
              Source Code
            </a>
          </div>
        </section>

        <ScreenshotPlaceholder label="Party Games app preview" />

        <section className="space-y-4">
          <h2 className="text-2xl font-bold">Overview</h2>
          <p className="leading-relaxed text-muted-foreground">
            Party Games is a cross-platform app that brings several familiar
            mini-games together in one place. It is designed for quick setup
            and casual play, with game logic running locally on the device.
            The project is still ongoing, with more polish and improvements to
            come.
          </p>
        </section>

        <section className="space-y-6">
          <h2 className="text-2xl font-bold">Games</h2>
          <div className="grid gap-6 md:grid-cols-2">
            {games.map((game) => (
              <article
                key={game.name}
                className="space-y-3 rounded-xl border border-border bg-card p-4"
              >
                <ScreenshotPlaceholder label={`${game.name} preview`} />
                <h3 className="text-xl font-semibold">{game.name}</h3>
                <p className="text-muted-foreground">{game.description}</p>
              </article>
            ))}
          </div>
        </section>

        <section className="space-y-4">
          <h2 className="text-2xl font-bold">Technical Highlights</h2>
          <ul className="list-disc space-y-2 pl-5 text-muted-foreground">
            <li>
              Built with .NET MAUI to share one app codebase across supported
              platforms.
            </li>
            <li>
              Uses MVVM to keep game rules and state in ViewModels, separate
              from the XAML screens.
            </li>
            <li>
              Uses dependency injection for game and navigation services.
            </li>
            <li>
              Includes interactive game flows, score tracking, and animations.
            </li>
          </ul>
        </section>

        <section className="space-y-4 rounded-xl border border-primary/30 bg-primary/5 p-6">
          <h2 className="text-2xl font-bold">Project Status</h2>
          <p className="leading-relaxed text-muted-foreground">
            This project is still in progress. The current version includes six
            playable games, and I am continuing to improve the experience and
            refine the app.
          </p>
        </section>
      </div>
    </div>
  );
};

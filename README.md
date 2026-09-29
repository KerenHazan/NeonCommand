# Neon Command

A 2D arcade defense game inspired by Missile Command, created by Keren Hazan for the Unity final project at MTA (2026). Defend four cities, set off missile chain reactions and beat your best score.

## How to Play

- **PC:** left-click a point in the sky to launch an interceptor. Press **Escape** to pause or resume.
- **Android:** tap the sky to target. Use the on-screen **Pause** and **Resume** buttons.
- Select **Play** from the Main Menu. Pause offers Resume, Restart and Main Menu; Game Over offers Retry and Main Menu.

Each shot costs one ammo. Only one interceptor can be in flight at a time. Clicks/taps over UI or below the playable sky area are ignored.

## Gameplay

Enemy missiles target living cities. Launch from the central battery to create an expanding explosion at your chosen point. Destroyed missiles produce smaller explosions that can continue a chain reaction; player explosions never damage cities.

Wave 1 has five missiles, Wave 2 has six, and each later wave adds one. Ammo refills to **10** when a wave starts. Clear all scheduled missiles while keeping at least one city alive to advance. The run ends when all four cities are destroyed.

Direct interceptions award **100 points**. Each chain generation adds **50**, giving 100, 150, 200, and so on. City impacts award no destruction points. Wave clear adds **100 per surviving city + 10 per unused shot**. Best Score is saved locally on Game Over using PlayerPrefs key `BestScore`.

## Main Features

- Wave progression, limited ammo and missile chain reactions.
- Generation scoring, wave bonuses and persistent Best Score.
- Enemy missile pooling, reusable explosions and a reusable interceptor.
- Responsive PC/Android HUD and menus with Android safe-area support.
- Main Menu, wave transitions, pause/resume, restart and Game Over flow.
- Simple neon-inspired shapes and a 60 FPS target.

See [the Game Design Document](Docs/GDD.md) for exact rules, architecture and future polish. Audio and advanced visual effects are not part of the current MVP.

## Platforms

**Windows PC** and **Android**, with landscape orientation on Android. Both use Unity's Input System and the same targeting flow.

## Unity Version

**Unity 6.3 LTS — 6000.3.20f1**, using URP 2D.

## Build

Open the project in the specified Unity version. `Assets/Scenes/Game.unity` is the enabled build scene and opens into the Main Menu. Select Windows or Android in Build Profiles; Android requires Unity's Android Build Support.

Generate Android builds locally under **`Builds/Android/`**, for example `Builds/Android/NeonCommand.apk`. **Do not commit generated builds to Git.** The build directory and APK/AAB files are ignored. Validate the latest build on the intended PC or Android device before distributing it.

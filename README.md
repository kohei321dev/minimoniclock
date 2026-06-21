# minimoniclock

`minimoniclock` is a Windows desktop clock app for a small secondary display.

The first target display is JAPANNEXT `JN-MD-IPS7842`, a 7.8-inch `400x1280` display. Phase 1 focuses only on an always-visible 24-hour digital clock that fits both portrait and landscape orientations.

## Phase 1 Scope

- Always-visible digital clock.
- 24-hour time display.
- Portrait and landscape JUSTFIT layout.
- Windows desktop runtime.
- No activity logs, history, cloud sync, or external communication.

## Development

Requirements:

- Windows 11
- .NET SDK 8

Run:

```powershell
dotnet run --project src/Minimoniclock/Minimoniclock.csproj
```

Build:

```powershell
dotnet build src/Minimoniclock/Minimoniclock.csproj
```

## Future Scope

- Pomodoro timer.
- Countdown timer.
- Alarm.
- Completion display such as `0` or `00:00`.
- Optional local settings.

## Docs

- [docs overview](./docs/README.md)
- [request definition](./docs/01-request-definition.md)
- [requirements definition](./docs/02-requirements-definition.md)
- [feasibility and design notes](./docs/03-feasibility-and-design-notes.md)
- [phase 1 acceptance checklist](./docs/phase1-acceptance-checklist.md)
- [open items](./docs/99-open-items.md)

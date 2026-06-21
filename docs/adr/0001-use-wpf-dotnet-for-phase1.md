# ADR-0001: Phase 1 は WPF / .NET で実装する

## Status

Accepted

## Context

Phase 1 の目的は、JAPANNEXT `JN-MD-IPS7842` の `400x1280` 相当の縦向き表示と `1280x400` 相当の横向き表示で、24 時間デジタル時計を常時見やすく表示することである。

Phase 1 ではポモドーロ、カウントダウン、アラーム、設定保存、自動起動は扱わない。

## Decision

Phase 1 の実装技術として WPF / .NET 8 を採用する。

## Rationale

- Windows 専用アプリとして要件と合っている。
- WPF の `Viewbox` とベクター描画により、時刻表示をウィンドウサイズへ追従させやすい。
- ボーダーレス表示、常に手前表示、DPI awareness など、Phase 1 で必要な Windows デスクトップ制御を扱いやすい。
- 外部パッケージなしで MVP を始められる。

## Alternatives

- WinUI 3: Windows 11 らしい UI には向くが、Phase 1 の表示要件に対して初期導入が重い。
- Tauri: Web UI で作り込みやすいが、Windows 専用のマルチディスプレイ制御が主論点のため第一候補にしない。
- Electron: 試作は速いが、常時表示時計としてはメモリ使用量が大きくなりやすい。

## Consequences

- Windows 以外は対象外にする。
- ビルドには .NET SDK 8 が必要。
- 実機 DPI と表示スケーリングは、実装後に `JN-MD-IPS7842` で確認する。

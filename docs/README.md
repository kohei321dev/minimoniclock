# minimoniclock docs

## 目的

`minimoniclock` は、Windows 端末に接続した小型サブディスプレイ上で、縦向き・横向きのどちらでも見やすい 24 時間表記のデジタル時計を常時表示するデスクトップアプリである。

最初の対象ディスプレイは JAPANNEXT の 7.8 インチ級サブディスプレイ `JN-MD-IPS7842` とする。

[事実] JAPANNEXT 公式ページでは、`JN-MD-IPS7842` は 7.8 インチ IPS パネル、解像度 `400x1280`、縦横使用可能、Windows のみに対応、スピーカー非搭載と説明されている。

## ドキュメント構成

- [要求定義](./01-request-definition.md)
- [要件定義](./02-requirements-definition.md)
- [実現性・設計検討](./03-feasibility-and-design-notes.md)
- [フェーズ計画と Issue 分割](./04-phase-plan-and-issue-slice.md)
- [Phase 1 受け入れチェックリスト](./phase1-acceptance-checklist.md)
- [ADR-0001: Phase 1 は WPF / .NET で実装する](./adr/0001-use-wpf-dotnet-for-phase1.md)
- [未対応・未決リスト](./99-open-items.md)

## 現時点の結論

- [事実] Windows 上の小型サブディスプレイ向けに、時計、ポモドーロタイマー、アラームを同一画面で表示するローカルアプリを作ることは実現可能。
- [事実] 対象ディスプレイは `400x1280` の縦型小型ディスプレイで、横向き利用も想定されている。
- [推測] 実装方式は WPF / .NET を第一候補、WinUI 3 を第二候補、Tauri / Electron をプロトタイプまたは Web UI 優先時の候補とする。
- [未検証] Windows 上の実 DPI、表示スケール、ディスプレイ名、回転時の OS 挙動、常時表示時の焼き付き・輝度・発熱影響は実機で確認する必要がある。

## Phase 1 MVP

Phase 1 では、ログ保存やクラウド同期は扱わない。まず「見やすい 24 時間デジタル時計を常時表示し、縦向き・横向きのどちらでも JUSTFIT する」状態を目標にする。

ポモドーロ、カウントダウン、アラーム、完了時 `0` 表示は Phase 2 以降の拡張機能として扱う。

## 参照

- JAPANNEXT 公式製品ページ: <https://jp.japannext.com/products/jn-md-ips7842>

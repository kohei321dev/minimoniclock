# minimoniclock

`minimoniclock` は、小型サブディスプレイ向けの Windows デスクトップ時計アプリです。

最初の対象ディスプレイは JAPANNEXT `JN-MD-IPS7842` です。Phase 1 では、縦向き・横向きのどちらでも見やすい 24 時間表記の常時表示時計に絞っています。

## Phase 1 スコープ

- 常時表示のデジタル時計。
- 24 時間表記。
- 縦向き・横向きの JUSTFIT レイアウト。
- Windows デスクトップでの起動。
- 操作ログ、履歴、クラウド同期、外部通信なし。

## 起動方法

### GitHub からダウンロードして起動する

1. GitHub の repository ページで `Code` を押す。
2. `Download ZIP` を選ぶ。
3. ダウンロードした zip を展開する。
4. 展開したフォルダの `run-minimoniclock.cmd` を実行する。

ランチャーは `.NET SDK 8` の有無を確認し、必要に応じて build したあと、`minimoniclock` を起動します。

### コマンドから起動する

```powershell
.\run-minimoniclock.cmd
```

ビルド済みファイルをそのまま起動したい場合:

```powershell
.\run-minimoniclock.cmd -NoBuild
```

代表サイズでレイアウトを確認する場合:

```powershell
.\run-minimoniclock.cmd -WindowSize 400x1280
.\run-minimoniclock.cmd -WindowSize 1280x400
```

ランチャーは通常、アプリ終了まで console を開いたままにします。起動直後に終了した場合は、終了までの秒数と app log directory を表示します。console を待機させずに起動する場合は `.\run-minimoniclock.cmd -Detached` を使います。

直接 `dotnet run` する場合:

```powershell
dotnet run --project src/Minimoniclock/Minimoniclock.csproj
```

詳しいローカル起動手順と縦横表示の変更方法は [Windows ローカル起動と縦横表示](./docs/windows-local-usage.md) を参照してください。

## 開発環境

- Windows 11
- .NET SDK 8

ビルド:

```powershell
dotnet build src/Minimoniclock/Minimoniclock.csproj
```

## 今後の拡張候補

- ポモドーロタイマー。
- カウントダウンタイマー。
- アラーム。
- `0` または `00:00` の完了表示。
- ローカル設定保存。

## ドキュメント

- [docs overview](./docs/README.md)
- [要求定義](./docs/01-request-definition.md)
- [要件定義](./docs/02-requirements-definition.md)
- [実現性・設計検討](./docs/03-feasibility-and-design-notes.md)
- [Windows ローカル起動と縦横表示](./docs/windows-local-usage.md)
- [Phase 1 受け入れチェックリスト](./docs/phase1-acceptance-checklist.md)
- [未対応・未決リスト](./docs/99-open-items.md)

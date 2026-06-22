# Windows ローカル起動と縦横表示

## 前提

- Windows 11。
- `.NET SDK 8` がインストールされている。
- repository が Windows ローカルに clone されている。

## clone

```powershell
git clone git@github.com:kohei321dev/minimoniclock.git
cd minimoniclock
```

Phase 1 実装確認中のブランチを使う場合:

```powershell
git switch codex/phase1-wpf-clock
```

## 起動

PowerShell または Windows Terminal で repository root から実行する。

推奨:

```powershell
.\run-minimoniclock.cmd
```

ランチャーは `.NET SDK 8` の有無を確認し、必要に応じて build したあと、ビルド済み exe を起動する。

ビルドを省略して起動する場合:

```powershell
.\run-minimoniclock.cmd -NoBuild
```

`dotnet run` で直接起動する場合:

```powershell
dotnet run --project src/Minimoniclock/Minimoniclock.csproj
```

ビルドだけ確認する場合:

```powershell
dotnet build src/Minimoniclock/Minimoniclock.csproj
```

ビルド済み exe を直接起動する場合:

```powershell
.\src\Minimoniclock\bin\Debug\net8.0-windows\Minimoniclock.exe
```

## サブディスプレイへ表示する

1. minimoniclock を起動する。
2. ウィンドウを `JN-MD-IPS7842` 側の画面へ移動する。
3. `FIT` を押して、ボーダーレス最大化表示にする。
4. 必要なら `TOP` を押して、常に手前表示にする。

## 縦横表示を変更する

Phase 1 では、アプリ内に縦横切替設定を持たない。モニターの向きは Windows のディスプレイ設定で変更する。

1. デスクトップを右クリックする。
2. `ディスプレイ設定` を開く。
3. `JN-MD-IPS7842` に該当するディスプレイを選択する。
4. `画面の向き` を選ぶ。
   - 縦向きで使う場合: `縦`
   - 横向きで使う場合: `横`
5. `変更の維持` を選ぶ。
6. minimoniclock を対象ディスプレイへ移動し、必要なら `FIT` を押す。

Windows 側で横向きにすると、対象ディスプレイは `1280x400` 相当の横長表示になる。minimoniclock はウィンドウの縦横比を見て、自動で `PORTRAIT` / `LANDSCAPE` を切り替える。

## 操作

- `FIT`: ボーダーレス最大化を切り替える。
- `TOP`: 常に手前表示を切り替える。
- `F11`: `FIT` と同じ。
- `Esc`: `FIT` 中に通常表示へ戻る。
- `T`: `TOP` と同じ。

## Phase 1 の方針

- アプリ内の設定ページは作らない。
- 縦横の向きは Windows 設定を正とする。
- 表示先ディスプレイの保存や自動復元は Phase 2 以降で検討する。
- ポモドーロ、カウントダウン、アラームは Phase 2 以降で扱う。

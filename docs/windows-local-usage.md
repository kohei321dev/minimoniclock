# Windows ローカル起動と縦横表示

## 前提

- Windows 11。
- `.NET SDK 8` がインストールされている。
- repository が Windows ローカルに clone されている。

## GitHub から zip をダウンロードして起動する

1. GitHub の repository ページで `Code` を押す。
2. `Download ZIP` を選ぶ。
3. ダウンロードした zip を展開する。
4. 展開したフォルダを開く。
5. `run-minimoniclock.cmd` を実行する。

ランチャーは `.NET SDK 8` の有無を確認し、必要に応じて build したあと、ビルド済み exe を起動する。

## clone して起動する

```powershell
git clone git@github.com:kohei321dev/minimoniclock.git
cd minimoniclock
```

Phase 1 実装確認中のブランチを使う場合:

```powershell
git switch codex/phase1-wpf-clock
```

## コマンドから起動

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

ランチャーは通常、アプリ終了まで console を開いたままにする。起動直後に終了した場合は、終了までの秒数と app log directory を表示する。

console を待機させずに起動する場合:

```powershell
.\run-minimoniclock.cmd -Detached
```

代表サイズで起動する場合:

```powershell
.\run-minimoniclock.cmd -WindowSize 400x1280
.\run-minimoniclock.cmd -WindowSize 1280x400
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

## ディスプレイ情報を確認する

実機検証では、Windows 上の解像度、作業領域、DPI、表示スケールを記録する。

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\scripts\collect-display-info.ps1
```

このスクリプトは Windows が取得できるディスプレイ情報を表示する。DPI や表示スケールは Windows 設定画面の表示と合わせて確認する。
出力をそのまま公開せず、以下の必要項目だけを docs に転記する。

記録する項目:

- Windows の表示スケール。
- `JN-MD-IPS7842` の Windows 上のディスプレイ名。
- 縦向き時の論理サイズと作業領域。
- 横向き時の論理サイズと作業領域。
- `FIT` 時にタスクバーや余白が出るか。

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

## 代表サイズでレイアウトを確認する

実機接続前でも、対象ディスプレイ相当のウィンドウサイズでレイアウトを確認できる。

縦向き相当:

```powershell
.\run-minimoniclock.cmd -WindowSize 400x1280
```

横向き相当:

```powershell
.\run-minimoniclock.cmd -WindowSize 1280x400
```

確認観点:

- 時刻表示が文字切れしない。
- 上部の `TOP` / `FIT` 操作が時刻表示を大きく邪魔しない。
- 下部の `PORTRAIT` / `LANDSCAPE` 表示が見える。
- ウィンドウをリサイズしても表示方向ラベルが縦横比に追従する。

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

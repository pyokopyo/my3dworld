# CHANGELOG

## v2026.1.2 - 2026-04-13

### Fixed
- **WebGLビルドでUI Toolkitの入力が正常に動作しない問題を修正**
  - UIが開いてもマウスカーソーが表示されなかった問題を修正
    - `LoginView` / `QuitConfirmationView` / `GlbSelectorView` の `OnEnable()` で `Cursor.visible = true` を設定（全プラットフォーム共通）
  - キーボードを押すとボタンが選択状態になってしまう問題を修正
    - WebGLビルド時のみ（`#if UNITY_WEBGL`）ボタンの `focusable = false` を設定し、ブラウザのキーイベントによる意図しないフォーカスを防止
  - `LoginView` 表示時に `UsernameField` を自動フォーカスし、すぐ入力できるように改善

---

## v2026.1.1 - 2026-04-08

### Fixed
- **ログイン画面での視点移動バグを修正**
  - 起動直後・ログイン画面表示中にマウスで視点が動いてしまう問題を修正
  - `GlbSelectorView.Start()` が `SetPanelVisible(false)` を呼んで誤って視点を有効化していた原因を特定・修正
  - `PlayerController.LookEnabled` を `[System.NonSerialized]` にし、Inspectorによる誤保存を防止

- **ESC → 終了キャンセル時の視点誤復帰を修正**
  - ログイン画面でESCを押して「いいえ」を選ぶと視点が動き始めるバグを修正
  - `LookEnabledBeforeQuit` フラグで事前状態を保存し、ゲーム中だった場合のみ復帰するよう変更

### Added
- **Space / Left Ctrl で高さ方向の移動に対応**
  - `Space` で上昇、`Left Ctrl` で下降できるようになりました

- **視点制御の有効/無効フロー整理**
  - ログイン前: すべての移動・視点操作を無効化
  - ログイン後: `LoginView` が `LookEnabled = true` にして有効化
  - GLBパネル開放時: `GlbSelectorView` が `LookEnabled = false` に切り替え
  - GLBパネル閉鎖時: `GlbSelectorView` が `LookEnabled = true` に復帰
  - ESC: `LookEnabled = false`、終了「いいえ」でゲーム中のみ復帰

---

## v2026.1.0 - 2026-04-01

### Added
- 初期実装
  - `AppBootstrapper` によるマルチシーン起動（Permanent / UI_Overlay / World）
  - `LoginView` / `LoginViewModel` によるMVVMログインUI
  - `PlayerController` による WASD 移動・マウス視点回転（一人称）
  - `CameraFollow` による目線高さ固定
  - `GlbSelectorView` による GLB ファイル選択・ランタイムロード（G キー）
  - `GlbSpawner` / `LocalFileScanner` によるローカル GLB 読み込み
  - `SpaceGenerator` によるオブジェクト生成
  - `QuitConfirmationView` による終了確認ダイアログ（ESC キー）

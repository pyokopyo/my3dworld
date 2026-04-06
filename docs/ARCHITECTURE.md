# ARCHITECTURE.md - 設計規約

## 非同期処理規約
- **標準**: `UnityEngine.Awaitable` を使用。
- **実装パターン**: `async Awaitable` メソッド内で `await AsyncOperation` する形式を徹底。
- **例**:
  ```csharp
  public async UnityEngine.Awaitable LoadSceneAsync(string sceneName, LoadSceneMode mode)
  {
      var asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);
      if (asyncOperation == null)
      {
          throw new System.InvalidOperationException($"Failed to load scene: {sceneName}");
      }
      await asyncOperation;
  }
  ```
- **禁止**: `Task`, `async void`, 直接 `AsyncOperation.completed` イベント使用。

## シーン管理規約
- **インターフェース**: `ISceneLoader` を介した疎結合設計。
- **実装**: `SceneWorkflowManager` が `ISceneLoader` を実装。
- **禁止**: 直接 `SceneManager` を呼ばない。常に `ISceneLoader` を介する。
- **マルチシーン**: Permanent (常駐), UI_Overlay (メニュー), World (3D空間) のAdditive構成。

## フォルダ構造規約
```
Assets/_Project/
├── Art/                # 3Dモデル、マテリアル、テクスチャ
├── Docs/               # 本設計ドキュメント、API仕様書
├── Editor/             # 初期化・自動化スクリプト、カスタムエディタ
├── Prefabs/            # Bootstrap, Player, UI_Parts
├── Scenes/
│   ├── Permanent.unity # 常駐シーン（初期起動時にLoadSceneMode.Single）
│   ├── UI_Overlay.unity# メニューシーン（LoadSceneMode.Additive）
│   └── World.unity     # ワールド本体（LoadSceneMode.Additive）
├── Scripts/
│   ├── Core/           # Bootstrapper, DI, SceneContext
│   ├── Interfaces/     # 全てのインターフェース定義（疎結合の核）
│   ├── UI/             # View, ViewModel (MVVM)
│   ├── World/          # ModelLoader, DynamicRegistry, LOD
│   ├── Player/         # Controller, AvatarSwitcher
│   ├── Networking/     # FusionCallbacks, NetworkRunner
│   └── IO/             # SaveSystem, LocalFileScanner
├── Tests/              # Unity Test Framework用スクリプト
└── Settings/           # URP, Input, Tags/Layers
```

## 命名規則
- **インターフェース**: `I` プレフィックス (例: `ISceneLoader`)
- **クラス**: PascalCase (例: `SceneWorkflowManager`)
- **メソッド**: PascalCase (例: `LoadSceneAsync`)
- **変数**: camelCase (例: `sceneName`)
- **定数**: UPPER_SNAKE_CASE (例: `REQUIRED_FOLDERS`)

## 共通UIコンポーネント
- **終了ボタン**: `btn-exit` など明確な命名を使用。
- **UI処理**: UIからの終了要求は必ず `Application.Quit()` を呼び出し、エディタ上では `Debug.Log` で通知。
- **ログ**: UIイベントは詳細ログを残し、初期化失敗箇所を特定しやすくする。

## UI設計規約
- **破壊的なアクション**: アプリ終了等の前には、必ず ModalView によるユーザー確認を挟むこと。

## 依存関係
- **禁止**: 循環参照。Core -> Interfaces -> Core のような依存禁止。
- **DI**: サービスロケーターパターンまたはシンプルDIコンテナ使用。
- **テスト**: 全てのLogicクラスはPure C# Test可能に設計。

## 評価・検証
- **Unit Testable**: エディタ再生不要のテスト。
- **Log System**: `Debug.Log` 直接使用禁止。カテゴリ分けロガー使用。
- **Automated Check**: リファクタ時はTests/内のテスト整合性確認。
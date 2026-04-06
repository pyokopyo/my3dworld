# SEQUENCE.md - 処理フロー

## アプリ起動シーケンス

```mermaid
sequenceDiagram
    participant App as Unity App
    participant Bootstrap as AppBootstrapper
    participant Loader as ISceneLoader
    participant Manager as SceneWorkflowManager
    participant SceneMgr as SceneManager

    App->>Bootstrap: RuntimeInitializeOnLoadMethod (BeforeSceneLoad)
    Bootstrap->>Loader: new SceneWorkflowManager()
    Bootstrap->>Loader: LoadSceneAsync("Permanent", Single)
    Loader->>Manager: LoadSceneAsync("Permanent", Single)
    Manager->>SceneMgr: LoadSceneAsync("Permanent", Single)
    SceneMgr-->>Manager: AsyncOperation
    Manager->>Manager: await AsyncOperation
    Manager-->>Loader: Awaitable (completed)
    Loader-->>Bootstrap: Awaitable (completed)

    Bootstrap->>Loader: LoadSceneAsync("UI_Overlay", Additive)
    Loader->>Manager: LoadSceneAsync("UI_Overlay", Additive)
    Manager->>SceneMgr: LoadSceneAsync("UI_Overlay", Additive)
    SceneMgr-->>Manager: AsyncOperation
    Manager->>Manager: await AsyncOperation
    Manager-->>Loader: Awaitable (completed)
    Loader-->>Bootstrap: Awaitable (completed)

    Bootstrap->>Loader: LoadSceneAsync("World", Additive)
    Loader->>Manager: LoadSceneAsync("World", Additive)
    Manager->>SceneMgr: LoadSceneAsync("World", Additive)
    SceneMgr-->>Manager: AsyncOperation
    Manager->>Manager: await AsyncOperation
    Manager-->>Loader: Awaitable (completed)
    Loader-->>Bootstrap: Awaitable (completed)

    Bootstrap-->>App: Initialization Complete
```

## ログイン実行シーケンス

```mermaid
sequenceDiagram
    participant User as User
    participant LoginView as LoginView (UI Toolkit)
    participant AuthMgr as AuthManager (Mock)
    participant SceneCtrl as SceneController
    participant Loader as ISceneLoader

    User->>LoginView: Enter username & click login
    LoginView->>AuthMgr: Validate(username)
    AuthMgr->>AuthMgr: Check local storage
    AuthMgr-->>LoginView: Validation result

    alt Valid
        LoginView->>SceneCtrl: OnLoginSuccess()
        SceneCtrl->>Loader: LoadSceneAsync("World", Additive) or Enable Camera
        Loader->>Loader: Scene transition
        SceneCtrl-->>LoginView: Transition complete
        LoginView->>LoginView: Hide login UI
    else Invalid
        AuthMgr-->>LoginView: Error message
        LoginView->>LoginView: Show error UI
    end
```

## シーンフロー概要
1. **起動**: AppBootstrapper が Permanent → UI_Overlay → World の順でロード
2. **ログイン**: UI_Overlay の LoginView で認証
3. **遷移**: 成功時 World シーン有効化、3D空間操作開始
4. **操作**: モデル配置、ネットワーク同期などの機能実行

## 非同期処理フロー
- 全てのシーン読み込みは `async Awaitable` メソッド経由
- `ISceneLoader` インターフェースで疎結合
- エラー時は `InvalidOperationException` スロー
- 完了までUIブロッキングなし
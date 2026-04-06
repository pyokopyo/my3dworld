# Project Blueprint: Scanning Open World (SOW) - v2026.1

## 1. システム概要 & 技術スタック
* **Engine**: Unity 6 (6000.0.x) - URP
* **Scripting**: C# 12 / .NET 9 Standard
* **Async**: `Awaitable` (Unity 6 Native)
* **Networking**: Photon Fusion 2 (Shared Mode)
* **UI System**: UI Toolkit (UXML / USS)
* **3D Loader**: glTFast (Runtime GLB/GLTF loading)
* **Dependency Injection**: シンプルなDIコンテナ または サービスロケーターパターン（疎結合維持のため）

## 2. 設計原則 (Design Principles) - **Copilotへの絶対命令**
1.  **SOLID原則 & SRP**: 1クラス1責任。特に「データ保持(Model)」「表示(View)」「制御(Logic)」を徹底分離する。
2.  **Interface-First**: 具象クラスの前に必ず `interface` を作成する。Copilotは実装クラス作成時にこのinterfaceを継承すること。
3.  **Additive Multi-Scene Architecture**: シーンは「分ける」のではなく「重ねる」。
    * `Permanent`: 常駐（Manager類）
    * `UI_Overlay`: メニュー・ログイン（UI Toolkit）
    * `Environment`: 3Dワールド本体
4.  **Async/Awaitable Standard**: 全てのI/O、通信、重い処理は `Awaitable` を使用し、`CancellationToken` を適切に伝搬させる。
5.  **Fail-Safe & Validator**: 外部3Dデータのロード時は必ずバリデーション（容量・ポリゴン数チェック）を行い、例外処理を完備する。

## 3. フォルダ構成 (Directory Structure)
```text
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

## 4. 必須実装機能（スモールスタート・セット）
| 機能カテゴリ | 詳細仕様 |
| :--- | :--- |
| **Bootstrap** | 起動時に `Permanent` シーンを読み込み、DIコンテナに各マネージャーを登録する |
| **Additive Manager** | 状態（Title/Lobby/InGame）に応じてUIシーンとWorldシーンを動的に加算ロード/破棄する |
| **Login (Mock)** | 最初はローカル保存のユーザー名でログイン。将来的にPhoton認証へ差し替え可能な設計 |
| **Dynamic Loader** | `persistentDataPath/ScannedData/` 内の .glb を検知し、ワールドに順次配置 |
| **Save System** | `Newtonsoft.Json` を使用。アバターID、配置済みモデルのTransform、設定値を保存 |
| **Quality Scoper** | 実行環境（PCスペック）に合わせて、動的ロードするモデルのLODレベルを自動調整 |

## 5. 評価と検証 (Validation & Testing)
1.  **Unit Testable**: 全ての `Logic` クラスはUnityエディタを再生せずにテスト可能（Pure C# Test）にすること。
2.  **Log System**: `Debug.Log` を直接使わず、カテゴリ分けされたカスタムロガーを介して、実行時の挙動を追跡可能にする。
3.  **Automated Check**: Copilotに対し、「リファクタリング時は必ず `Tests/` 内の既存テストと整合性が取れているか確認せよ」と指示する。
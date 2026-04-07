# Scanning Open World (SOW) - v2026.1.1

## プロジェクト概要

Scanning Open World (SOW) は、個人スキャンデータ（.glb/.gltf）を活用したパーソナル3D空間の構築を目的としたUnityプロジェクトです。ユーザーがスキャンした3Dモデルをランタイムでロードし、共有可能な仮想空間を提供します。

## 技術スタック

- **Engine**: Unity 6 (6000.0.x) - URP (Universal Render Pipeline)
- **Scripting**: C# 12 / .NET Standard 2.1
- **Async**: `UnityEngine.Awaitable` (Unity 6 Native)
- **Networking**: Photon Fusion 2 (Shared Mode)
- **UI System**: UI Toolkit (UXML / USS)
- **3D Loader**: glTFast (Runtime GLB/GLTF loading)
- **Dependency Injection**: シンプルなDIコンテナまたはサービスロケーターパターン

## 設計原則

- **SOLID原則 & SRP**: 1クラス1責任。特に「データ保持(Model)」「表示(View)」「制御(Logic)」を徹底分離。
- **Interface-First**: 具象クラスの前に必ず `interface` を作成。
- **Additive Multi-Scene Architecture**: シーンは「分ける」のではなく「重ねる」。
- **Async/Awaitable Standard**: 全てのI/O、通信、重い処理は `Awaitable` を使用。
- **Fail-Safe & Validator**: 外部3Dデータのロード時は必ずバリデーション（容量・ポリゴン数チェック）を行い、例外処理を完備。

## セットアップ方法

1. **Unityのインストール**
   - Unity Hubをインストールし、Unity 6 (6000.0.x)をインストールしてください。

2. **プロジェクトのクローン**
   ```bash
   git clone <リポジトリURL>
   cd My3DWorld
   ```

3. **依存関係のインストール**
   - Unityでプロジェクトを開くと、Package Managerから必要なパッケージが自動的にインストールされます。
   - Photon Fusion 2、glTFastなどのパッケージを追加してください。

4. **ビルド設定**
   - Project Settings > Player でプラットフォームを設定してください。
   - URPを設定し、必要なレンダリング設定を行ってください。

## 使用方法

1. **起動**
   - Unity Editorで `Assets/_Project/Scenes/Permanent.unity` を開いてください。
   - Playボタンを押すと、自動的にシーンがロードされます。

2. **ログイン**
   - ログイン画面でユーザー名を入力してLoginボタンを押してください。
   - ログイン後、カーソルがロックされてプレイヤー操作が有効になります。

3. **プレイヤー操作**
   - `W / A / S / D` : 前後左右移動
   - `Space` : 上昇
   - `Left Ctrl` : 下降
   - `マウス` : 視点回転
   - `G` : GLBファイル選択パネルの開閉
   - `Esc` : カーソル解放（終了確認ダイアログを表示）

4. **3DモデルのロードとWebGL対応状況**
   - **Windowsスタンドアローン / Editor**: ローカルの任意フォルダから `.glb` ファイルを選択してワールドに配置できます。
   - **WebGL**: `StreamingAssets/Models/` に `.glb` を配置すればロード可能ですが、フォルダスキャンには制限があります（後述）。

## フォルダ構成

```
Assets/_Project/
├── Art/                # 3Dモデル、マテリアル、テクスチャ
├── Docs/               # 設計ドキュメント、API仕様書
├── Editor/             # 初期化・自動化スクリプト、カスタムエディタ
├── Prefabs/            # Bootstrap, Player, UI_Parts
├── Scenes/
│   ├── Permanent.unity # 常駐シーン
│   ├── UI_Overlay.unity# メニューシーン
│   └── World.unity     # ワールド本体
├── Scripts/
│   ├── Core/           # Bootstrapper, DI, SceneContext
│   ├── Interfaces/     # 全てのインターフェース定義
│   ├── UI/             # View, ViewModel (MVVM)
│   ├── World/          # ModelLoader, DynamicRegistry, LOD
│   ├── Player/         # Controller, AvatarSwitcher
│   ├── Networking/     # FusionCallbacks, NetworkRunner
│   └── IO/             # SaveSystem, LocalFileScanner
├── Tests/              # Unity Test Framework用スクリプト
└── Settings/           # URP, Input, Tags/Layers
```

## ドキュメント

詳細なドキュメントは `docs/` フォルダにあります：

- [ARCHITECT_BLUEPRINT.md](docs/ARCHITECT_BLUEPRINT.md) - プロジェクト設計の青写真
- [ARCHITECTURE.md](docs/ARCHITECTURE.md) - アーキテクチャ概要
- [CONTEXT.md](docs/CONTEXT.md) - プロジェクト概要
- [SEQUENCE.md](docs/SEQUENCE.md) - シーケンス図
- [TODO.md](docs/TODO.md) - 今後のロードマップ
- [UI_DESIGN.md](docs/UI_DESIGN.md) - UI設計

## 貢献方法

1. このリポジトリをフォークしてください。
2. 機能ブランチを作成してください (`git checkout -b feature/AmazingFeature`)。
3. 変更をコミットしてください (`git commit -m 'Add some AmazingFeature'`)。
4. ブランチにプッシュしてください (`git push origin feature/AmazingFeature`)。
5. Pull Requestを作成してください。

## ライセンス

このプロジェクトはMITライセンスの下で公開されています。詳細はLICENSEファイルを参照してください。

## 連絡先

質問やフィードバックがある場合は、[Issues](https://github.com/your-repo/issues) を使用してください。
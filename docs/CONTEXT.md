# CONTEXT.md - プロジェクト概要

## プロジェクト名
Scanning Open World (SOW) - v2026.1

## 目的
個人スキャンデータ（.glb/.gltf）を活用したパーソナル3D空間の構築。
ユーザーがスキャンした3Dモデルをランタイムでロードし、共有可能な仮想空間を提供する。

## 現状
- Permanent, UI_Overlay, World のマルチシーン基盤が完成。
- AppBootstrapper による自動シーン読み込み実装済み。
- ISceneLoader インターフェースを介したシーン管理基盤構築。

## 技術スタック
- **Engine**: Unity 6 (6000.0.x) - URP
- **Scripting**: C# 12 / .NET Standard 2.1
- **Async**: `UnityEngine.Awaitable` (Unity 6 Native)
- **Networking**: Photon Fusion 2 (Shared Mode)
- **UI System**: UI Toolkit (UXML / USS)
- **3D Loader**: glTFast (Runtime GLB/GLTF loading)
- **Dependency Injection**: シンプルなDIコンテナまたはサービスロケーターパターン

## 設計原則
- SOLID原則 & SRP: 1クラス1責任。
- Interface-First: 具象クラスの前に必ず `interface` を作成。
- Additive Multi-Scene Architecture: シーンは重ねる。
- Async/Awaitable Standard: 全てのI/Oは `Awaitable` を使用。
- Fail-Safe & Validator: 外部3Dデータのロード時はバリデーション必須。

## 必須実装機能
- Bootstrap: 起動時に Permanent シーンをロード、DIコンテナにマネージャー登録。
- Additive Manager: 状態に応じてUIシーンとWorldシーンを動的に加算ロード/破棄。
- Login (Mock): ローカル保存のユーザー名でログイン。
- Dynamic Loader: persistentDataPath/ScannedData/ 内の .glb を検知し配置。
- Save System: Newtonsoft.Json を使用した保存。
- Quality Scoper: 実行環境に合わせてLODレベル調整。

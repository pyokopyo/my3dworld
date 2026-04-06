# TODO.md - 今後のロードマップ

## 短期目標 (v2026.1 - 完了予定: 2026年4月)
### ログイン機能の実装
- [ ] `Assets/_Project/Scripts/UI/LoginView.cs` 作成 (UI Toolkit ViewModel)
- [ ] `Assets/_Project/Scripts/UI/LoginViewModel.cs` 作成 (MVVMパターン)
- [ ] `Assets/_Project/Scripts/Core/AuthManager.cs` 作成 (Mock認証)
- [ ] UI_OverlayシーンのUXML/USS設定
- [ ] ローカルPlayerPrefsを使用したユーザー名保存/読み込み

### UI Toolkit によるプロフ入力
- [ ] ユーザー情報入力フォーム (名前、説明文)
- [ ] アバター選択UI (プレビュー表示)
- [ ] 設定保存機能 (JsonUtility使用)

## 中期目標 (v2026.2 - 完了予定: 2026年6月)
### アバター表示
- [ ] `Assets/_Project/Scripts/Player/AvatarController.cs` 作成
- [ ] `Assets/_Project/Scripts/Player/AvatarSwitcher.cs` 作成
- [ ] アバターモデルのランタイム切り替え
- [ ] アニメーション同期 (Animator使用)

### スキャンデータ（.glb）のランタイムロード
- [ ] `Assets/_Project/Scripts/World/ModelLoader.cs` 作成
- [ ] glTFast統合 (Runtime GLB/GLTF loading)
- [ ] `Assets/_Project/Scripts/World/DynamicRegistry.cs` 作成 (配置管理)
- [ ] `persistentDataPath/ScannedData/` 監視機能
- [ ] バリデーション (容量・ポリゴン数チェック)

### Photon Fusion 2 による同期
- [ ] `Assets/_Project/Scripts/Networking/FusionRunner.cs` 作成
- [ ] `Assets/_Project/Scripts/Networking/NetworkCallbacks.cs` 作成
- [ ] Shared Mode設定
- [ ] プレイヤー位置同期
- [ ] モデル配置同期

## 長期目標 (v2026.3 - 完了予定: 2026年9月)
### 高度な3D操作
- [ ] LOD (Level of Detail) 自動調整
- [ ] 衝突判定最適化
- [ ] ライトマップ動的生成

### パフォーマンス最適化
- [ ] Object Pooling 実装
- [ ] アセットバンドル化
- [ ] メモリ管理改善

### 拡張機能
- [ ] VR/AR対応
- [ ] モバイル最適化
- [ ] クラウド保存連携

## 技術的懸念事項
- **Awaitable互換性**: Unity 6の仕様変更に追従
- **Photon Fusion**: Shared Modeの安定性確認
- **glTFast**: ランタイムパフォーマンス検証
- **UI Toolkit**: モバイルデバイス対応

## テスト計画
- **Unit Test**: Scripts/Tests/ に全Logicクラステスト追加
- **Integration Test**: シーン遷移・同期機能テスト
- **Performance Test**: モデル数増加時のFPS測定
- **Compatibility Test**: 複数デバイスでの動作確認

## リスク管理
- **API変更**: Unity 6の更新に追従
- **ライブラリ互換**: Photon, glTFastのバージョン管理
- **パフォーマンス**: モバイルデバイス対応確認
- **セキュリティ**: ネットワーク通信の安全対策
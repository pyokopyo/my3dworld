# UI_DESIGN.md - UI設計書

## 技術仕様
- **UI Framework**: UI Toolkit (UXML / USS)
- **アーキテクチャ**: MVVM (Model-View-ViewModel)
- **テーマ**: ダークテーマベース、USS変数使用
- **レスポンシブ**: モバイル/PC両対応

## 画面構成
### LoginView (UI_Overlayシーン)
- **目的**: ユーザー認証（Mock）
- **要素**:
  - ユーザー名入力フィールド (TextField)
  - ログインボタン (Button)
  - エラーメッセージ表示 (Label)
- **遷移**: ログイン成功後、Worldシーン有効化またはカメラ切り替え

### WorldUI (Worldシーンオーバーレイ)
- **目的**: 3D空間内操作UI
- **要素**:
  - メニュー開閉ボタン
  - モデル配置ツールバー
  - 設定パネル

## 命名規則
### USSクラス名
- **ボタン**: `btn-` プレフィックス (例: `btn-primary`, `btn-login`)
- **ラベル**: `lbl-` プレフィックス (例: `lbl-title`, `lbl-error`)
- **入力フィールド**: `input-` プレフィックス (例: `input-username`)
- **コンテナ**: `container-` プレフィックス (例: `container-login`)

### VisualElement ID
- **PascalCase**: `LoginButton`, `UsernameField`
- **機能別プレフィックス**: `BtnLogin`, `FldUsername`

## スタイル定義例 (USS)
```uss
.btn-primary {
    background-color: var(--primary-color);
    border-radius: 4px;
    padding: 8px 16px;
}

.lbl-error {
    color: var(--error-color);
    font-weight: bold;
}

.input-field {
    border: 1px solid var(--border-color);
    border-radius: 2px;
    padding: 4px;
}
```

## 処理フロー
1. **アプリ起動**: UI_Overlayシーン読み込み、LoginView表示
2. **ユーザー入力**: ユーザー名入力、ログイン実行
3. **認証**: ローカル保存データ検証（Mock）
4. **遷移**: 成功時Worldシーン有効化、カメラ制御開始
5. **エラー**: 失敗時エラーメッセージ表示、再入力促す

## アクセシビリティ
- **キーボード操作**: Tabキーでのフォーカス移動
- **スクリーンリーダー**: Labelのtextプロパティ適切設定
- **コントラスト**: WCAG 2.1 AA準拠カラー使用

## パフォーマンス
- **遅延読み込み**: UI要素は必要時のみインスタンス化
- **プーリング**: 頻繁に表示/非表示される要素はObjectPool使用
- **最適化**: USS変数使用で動的スタイル変更最小化
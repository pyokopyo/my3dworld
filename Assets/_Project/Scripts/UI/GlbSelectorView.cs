using System;
using System.Collections.Generic;
using System.IO;
using My3DWorld.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace My3DWorld.UI
{
    /// <summary>
    /// 特定フォルダのGLBファイルを選択してワールドに生成するUI Toolkit製パネル。
    /// GキーでパネルのON/OFFを切り替える（Inspectorで変更可能）。
    /// GlbSpawner が同一GameObjectになければ自動でAddComponentする。
    /// Scripts/UI/ に配置（設計規約準拠）。
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class GlbSelectorView : MonoBehaviour
    {
        [Header("GLBフォルダ設定")]
        [Tooltip(
            "GLBファイルを格納するフォルダの絶対パス。\n" +
            "空の場合: Editorは Assets/_Project/Models、ビルドは StreamingAssets/Models を使用。")]
        [SerializeField] private string _glbFolderPath = "";

        [Header("スポーン設定")]
        [Tooltip("生成基点となるTransform。Nullのときはメインカメラ前方にスポーン")]
        [SerializeField] private Transform _spawnOrigin;

        [SerializeField] private float _spawnDistance = 3f;

        [Header("キー設定")]
        [SerializeField] private Key _toggleKey = Key.G;

        // ── UI要素 ─────────────────────────────────────────────
        private UIDocument _uiDocument;
        private VisualElement _panel;
        private Label _folderLabel;
        private ScrollView _scrollView;
        private Label _statusLabel;
        private Button _refreshButton;
        private Button _closeButton;

        // ── 内部状態 ──────────────────────────────────────────
        private World.GlbSpawner _spawner;
        private IReadOnlyList<string> _glbFiles = Array.Empty<string>();
        private bool _isLoading;

        // ── ライフサイクル ────────────────────────────────────
        private void Awake()
        {
            _spawner = GetComponent<World.GlbSpawner>();
            if (_spawner == null)
            {
                _spawner = gameObject.AddComponent<World.GlbSpawner>();
            }

            _uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(_glbFolderPath))
            {
#if UNITY_EDITOR
                _glbFolderPath = Path.Combine(Application.dataPath, "_Project", "Models");
#else
                _glbFolderPath = Path.Combine(Application.streamingAssetsPath, "Models");
#endif
            }

            BindUI();
            InitPanelHidden();
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current[_toggleKey].wasPressedThisFrame)
            {
                TogglePanel();
            }
        }

        // ── UIバインド ────────────────────────────────────────
        private void BindUI()
        {
            if (_uiDocument == null) return;

            var root = _uiDocument.rootVisualElement;

            _panel        = root.Q<VisualElement>("GlbSelectorPanel");
            _folderLabel  = root.Q<Label>("FolderLabel");
            _scrollView   = root.Q<ScrollView>("GlbScrollView");
            _statusLabel  = root.Q<Label>("StatusLabel");
            _refreshButton = root.Q<Button>("RefreshButton");
            _closeButton  = root.Q<Button>("CloseButton");

            if (_refreshButton != null) _refreshButton.clicked += RefreshList;
            if (_closeButton   != null) _closeButton.clicked   += () => SetPanelVisible(false);
        }

        // ── ロジック ─────────────────────────────────────────
        private void TogglePanel()
        {
            bool next = _panel != null && _panel.ClassListContains("hidden");
            SetPanelVisible(next);
            if (next) RefreshList();
        }

        /// <summary>起動時の初期非表示。LookEnabled・カーソルには触れない。</summary>
        private void InitPanelHidden()
        {
            if (_panel == null) return;
            _panel.AddToClassList("hidden");
        }

        private void SetPanelVisible(bool visible)
        {
            if (_panel == null) return;
            if (visible)
            {
                _panel.RemoveFromClassList("hidden");
                if (My3DWorld.Player.PlayerController.Instance != null)
                    My3DWorld.Player.PlayerController.Instance.LookEnabled = false;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
            }
            else
            {
                _panel.AddToClassList("hidden");
                if (My3DWorld.Player.PlayerController.Instance != null)
                    My3DWorld.Player.PlayerController.Instance.LookEnabled = true;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
            }
        }

        private void RefreshList()
        {
            _glbFiles = LocalFileScanner.ScanForGlb(_glbFolderPath);

            if (_folderLabel != null)
                _folderLabel.text = _glbFolderPath;

            if (_scrollView == null) return;

            _scrollView.Clear();

            if (_glbFiles.Count == 0)
            {
                SetStatus("GLBファイルが見つかりません。");
                return;
            }

            SetStatus("");
            foreach (var filePath in _glbFiles)
            {
                var captured = filePath;
                var btn = new Button(() => OnGlbSelected(captured))
                {
                    text = Path.GetFileNameWithoutExtension(filePath)
                };
                btn.AddToClassList("btn-glb-item");
                _scrollView.Add(btn);
            }
        }

        private void SetStatus(string message)
        {
            if (_statusLabel != null)
                _statusLabel.text = message;
        }

        private void OnGlbSelected(string filePath)
        {
            if (_isLoading) return;
            _ = SpawnGlbAsync(filePath);
        }

        private async Awaitable SpawnGlbAsync(string filePath)
        {
            _isLoading = true;
            SetStatus("読み込み中...");
            if (_refreshButton != null) _refreshButton.SetEnabled(false);

            try
            {
                var pos = GetSpawnPosition();
                await _spawner.LoadAndSpawnGlbAsync(filePath, pos);
                SetPanelVisible(false);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GlbSelectorView] GLB生成エラー: {ex.Message}");
                SetStatus("生成に失敗しました。");
            }
            finally
            {
                _isLoading = false;
                if (_refreshButton != null) _refreshButton.SetEnabled(true);
            }
        }

        private Vector3 GetSpawnPosition()
        {
            if (_spawnOrigin != null)
                return _spawnOrigin.position + _spawnOrigin.forward * _spawnDistance;

            var cam = Camera.main;
            if (cam != null)
                return cam.transform.position + cam.transform.forward * _spawnDistance;

            return Vector3.zero;
        }
    }
}

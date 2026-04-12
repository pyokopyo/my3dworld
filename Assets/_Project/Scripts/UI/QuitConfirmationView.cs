using My3DWorld.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.UI
{
    public class QuitConfirmationView : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;

        private Button _btnYes;
        private Button _btnNo;

        private void OnEnable()
        {
            if (_uiDocument == null)
            {
                _uiDocument = GetComponent<UIDocument>();
            }

            if (_uiDocument == null)
            {
                Debug.LogError("UIDocument is not assigned and could not be found on the GameObject.");
                return;
            }

            var root = _uiDocument.rootVisualElement;

            _btnYes = root.Q<Button>("BtnYes");
            if (_btnYes == null)
            {
                Debug.LogError("BtnYes not found in UXML.");
            }

            _btnNo = root.Q<Button>("BtnNo");
            if (_btnNo == null)
            {
                Debug.LogError("BtnNo not found in UXML.");
            }

            if (_btnYes == null || _btnNo == null)
            {
                Debug.LogError("Required UI elements not found in UXML. QuitConfirmationView cannot initialize properly.");
                return;
            }

            _btnYes.clicked += OnYesButtonClicked;
            _btnNo.clicked += OnNoButtonClicked;

            // UIが表示されている間はカーソルを必ず可視にする（全プラットフォーム共通）
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

#if UNITY_WEBGL
            // WebGL: ブラウザのキーイベントでボタンが意図せず選択されるのを防ぐ
            _btnYes.focusable = false;
            _btnNo.focusable = false;
#endif
        }

        private void OnDisable()
        {
            if (_btnYes != null)
            {
                _btnYes.clicked -= OnYesButtonClicked;
            }

            if (_btnNo != null)
            {
                _btnNo.clicked -= OnNoButtonClicked;
            }
        }

        private void OnYesButtonClicked()
        {
            Debug.Log("User confirmed quit.");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnNoButtonClicked()
        {
            Debug.Log("User cancelled quit.");
            gameObject.SetActive(false);
            // ESC前にゲームモードだった場合のみ視点操作を復帰（ログイン画面からのESCは復帰しない）
            if (PlayerController.Instance != null && PlayerController.Instance.LookEnabledBeforeQuit)
            {
                PlayerController.Instance.LookEnabled = true;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
            }
        }
    }
}
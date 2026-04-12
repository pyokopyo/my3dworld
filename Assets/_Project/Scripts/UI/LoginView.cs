using My3DWorld.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.UI
{
    public class LoginView : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private LoginViewModel _viewModel;

        private TextField _usernameField;
        private Button _loginButton;
        private Button _exitButton;
        private Label _errorLabel;

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

            if (_viewModel == null)
            {
                _viewModel = new LoginViewModel();
            }

            var root = _uiDocument.rootVisualElement;

            _usernameField = root.Q<TextField>("UsernameField");
            if (_usernameField == null)
            {
                Debug.LogError("UsernameField not found in UXML.");
            }

            _loginButton = root.Q<Button>("LoginButton");
            if (_loginButton == null)
            {
                Debug.LogError("LoginButton not found in UXML.");
            }

            _exitButton = root.Q<Button>("ExitButton");
            if (_exitButton == null)
            {
                Debug.LogError("ExitButton not found in UXML.");
            }

            _errorLabel = root.Q<Label>("ErrorLabel");
            if (_errorLabel == null)
            {
                Debug.LogError("ErrorLabel not found in UXML.");
            }

            if (_usernameField == null || _loginButton == null || _exitButton == null || _errorLabel == null)
            {
                Debug.LogError("Required UI elements not found in UXML. LoginView cannot initialize properly.");
                return;
            }

            _loginButton.clicked += OnLoginButtonClicked;
            _exitButton.clicked += OnExitButtonClicked;

            // UIが表示されている間はカーソルを必ず可視にする（全プラットフォーム共通）
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

#if UNITY_WEBGL
            // WebGL: ブラウザのキーイベントでボタンが意図せず選択されるのを防ぐ
            _loginButton.focusable = false;
            _exitButton.focusable = false;
#endif

            // レイアウト完了後にUsernameFieldをフォーカスしてすぐ入力できるようにする
            _usernameField.schedule.Execute(() => _usernameField.Focus());
        }

        private void OnDisable()
        {
            if (_loginButton != null)
            {
                _loginButton.clicked -= OnLoginButtonClicked;
            }

            if (_exitButton != null)
            {
                _exitButton.clicked -= OnExitButtonClicked;
            }
        }

        private void OnLoginButtonClicked()
        {
            var username = _usernameField.value?.Trim();
            Debug.Log($"Login attempt with: {username}");

            if (string.IsNullOrEmpty(username))
            {
                ShowError("Please enter a username.");
                return;
            }

            HideError();
            _viewModel?.OnLoginRequested(username);

            // ログイン成功：UIを隠してゲームモードに切り替える
            if (_uiDocument != null)
                _uiDocument.rootVisualElement.style.display = UnityEngine.UIElements.DisplayStyle.None;

            if (PlayerController.Instance != null)
                PlayerController.Instance.LookEnabled = true;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }

        private void OnExitButtonClicked()
        {
            Debug.Log("Application Quit requested from UI.");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void ShowError(string message)
        {
            _errorLabel.text = message;
            _errorLabel.style.display = DisplayStyle.Flex;
        }

        private void HideError()
        {
            _errorLabel.style.display = DisplayStyle.None;
        }
    }
}
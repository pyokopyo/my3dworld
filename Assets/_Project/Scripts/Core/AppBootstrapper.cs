using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Project.Interfaces;
using Project.UI;

namespace Project.Core
{
    internal static class AppBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void Initialize()
        {
            CreateBootstrapperObject();

            ISceneLoader sceneLoader = new SceneWorkflowManager();

            await sceneLoader.LoadSceneAsync("Permanent", LoadSceneMode.Single);
            await sceneLoader.LoadSceneAsync("UI_Overlay", LoadSceneMode.Additive);
            await sceneLoader.LoadSceneAsync("World", LoadSceneMode.Additive);
        }

        private static void CreateBootstrapperObject()
        {
            var bootstrapperObject = new GameObject("AppBootstrapper");
            Object.DontDestroyOnLoad(bootstrapperObject);
            bootstrapperObject.AddComponent<AppBootstrapperBehaviour>();
        }

        private sealed class AppBootstrapperBehaviour : MonoBehaviour
        {
            private QuitConfirmationView _quitConfirmationView;

            private void Start()
            {
                Debug.Log("Searching for QuitConfirmationView...");
                _quitConfirmationView = FindAnyObjectByType<QuitConfirmationView>(FindObjectsInactive.Include);
                if (_quitConfirmationView == null)
                {
                    Debug.LogWarning("QuitConfirmationView not found in the scene.");
                }
                else
                {
                    Debug.Log("QuitConfirmationView found and cached.");
                }
            }

            private void Update()
            {
                if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    if (_quitConfirmationView == null)
                    {
                        _quitConfirmationView = FindAnyObjectByType<QuitConfirmationView>();
                    }

                    if (_quitConfirmationView != null)
                    {
                        _quitConfirmationView.gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("QuitConfirmationView not available, quitting directly.");
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    }
                }
            }
        }
    }
}

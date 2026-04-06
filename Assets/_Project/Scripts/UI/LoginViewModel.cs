using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Core;
using Project.Interfaces;

namespace Project.UI
{
    public class LoginViewModel
    {
        private readonly ISceneLoader _sceneLoader;

        public LoginViewModel()
            : this(new SceneWorkflowManager())
        {
        }

        public LoginViewModel(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
        }

        public async Awaitable OnLoginRequested(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                Debug.LogError("Username is required.");
                return;
            }

            // Save username to PlayerPrefs (mock authentication)
            PlayerPrefs.SetString("Username", username);
            PlayerPrefs.Save();

            Debug.Log($"User '{username}' logged in successfully.");

            // Transition to World scene
            // Since World is already loaded additively, we unload UI_Overlay
            await UnloadUIOverlayAsync();

            // World scene is now active
            Debug.Log("Transitioned to World scene.");
        }

        private async Awaitable UnloadUIOverlayAsync()
        {
            var unloadOperation = SceneManager.UnloadSceneAsync("UI_Overlay");
            if (unloadOperation == null)
            {
                Debug.LogError("Failed to start unloading UI_Overlay scene.");
                return;
            }

            await unloadOperation;
            Debug.Log("UI_Overlay scene unloaded.");
        }
    }
}
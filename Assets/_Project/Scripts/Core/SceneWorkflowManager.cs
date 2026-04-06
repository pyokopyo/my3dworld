using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Interfaces;

namespace Project.Core
{
    public sealed class SceneWorkflowManager : ISceneLoader
    {
        public async UnityEngine.Awaitable LoadSceneAsync(string sceneName, LoadSceneMode mode)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);
            
            if (asyncOperation == null)
            {
                throw new System.InvalidOperationException($"Failed to load scene: {sceneName}");
            }

            await asyncOperation;
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Interfaces
{
    public interface ISceneLoader
    {
        Awaitable LoadSceneAsync(string sceneName, LoadSceneMode mode);
    }
}

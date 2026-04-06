using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectInitializer
{
    internal static class ProjectInitializer
    {
        private static readonly string[] RequiredFolders =
        {
            "Assets/_Project/Art",
            "Assets/_Project/Docs",
            "Assets/_Project/Editor",
            "Assets/_Project/Prefabs",
            "Assets/_Project/Scenes",
            "Assets/_Project/Scripts/Core",
            "Assets/_Project/Scripts/Interfaces",
            "Assets/_Project/Scripts/UI",
            "Assets/_Project/Scripts/World",
            "Assets/_Project/Scripts/Player",
            "Assets/_Project/Scripts/Networking",
            "Assets/_Project/Scripts/IO",
            "Assets/_Project/Tests",
            "Assets/_Project/Settings"
        };

        private static readonly string[] RequiredScenes =
        {
            "Permanent",
            "UI_Overlay",
            "World"
        };

        [MenuItem("Tools/Project Setup/Create _Project Folder Structure and Scenes")]
        private static void CreateProjectFoldersAndScenes()
        {
            var createdAny = false;

            foreach (var folder in RequiredFolders)
            {
                if (!AssetDatabase.IsValidFolder(folder))
                {
                    CreateFolderRecursive(folder);
                    Debug.Log($"Created folder: {folder}");
                    createdAny = true;
                }
            }

            if (!createdAny)
            {
                Debug.Log("All required folders already exist.");
            }

            CreateDefaultScenes();
            AssetDatabase.Refresh();
        }

        private static void CreateDefaultScenes()
        {
            const string scenesFolder = "Assets/_Project/Scenes";
            var originalScene = EditorSceneManager.GetActiveScene();
            var originalScenePath = originalScene.path;

            foreach (var sceneName in RequiredScenes)
            {
                var scenePath = Path.Combine(scenesFolder, sceneName + ".unity").Replace("\\", "/");
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

                if (sceneAsset == null)
                {
                    var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
                    EditorSceneManager.SaveScene(newScene, scenePath);
                    Debug.Log($"Created scene: {scenePath}");
                }
                else
                {
                    Debug.Log($"Scene already exists: {scenePath}");
                }
            }

            UpdateBuildSettingsScenes();

            if (!string.IsNullOrEmpty(originalScenePath))
            {
                EditorSceneManager.OpenScene(originalScenePath, OpenSceneMode.Single);
            }
        }

        private static void UpdateBuildSettingsScenes()
        {
            const string scenesFolder = "Assets/_Project/Scenes";
            var requiredScenePaths = RequiredScenes
                .Select(sceneName => Path.Combine(scenesFolder, sceneName + ".unity").Replace("\\", "/"))
                .ToArray();

            var updatedScenes = requiredScenePaths
                .Select(path => new EditorBuildSettingsScene(path, true))
                .ToArray();

            var currentScenes = EditorBuildSettings.scenes;
            var removedScenes = currentScenes
                .Where(entry => !requiredScenePaths.Contains(entry.path))
                .Select(entry => entry.path)
                .ToArray();

            if (removedScenes.Length > 0)
            {
                foreach (var removedPath in removedScenes)
                {
                    Debug.Log($"Removed undefined scene from Build Settings: {removedPath}");
                }
            }

            if (!BuildSettingsScenesEqual(currentScenes, updatedScenes))
            {
                EditorBuildSettings.scenes = updatedScenes;
                Debug.Log("Updated Build Settings scenes to match ARCHITECT_BLUEPRINT.md requirements.");
            }
            else
            {
                Debug.Log("Build Settings scenes already match ARCHITECT_BLUEPRINT.md requirements.");
            }
        }

        private static bool BuildSettingsScenesEqual(EditorBuildSettingsScene[] current, EditorBuildSettingsScene[] updated)
        {
            if (current.Length != updated.Length)
                return false;

            for (var index = 0; index < current.Length; index++)
            {
                if (current[index].path != updated[index].path || current[index].enabled != updated[index].enabled)
                    return false;
            }

            return true;
        }

        private static void CreateFolderRecursive(string folderPath)
        {
            if (AssetDatabase.IsValidFolder(folderPath))
                return;

            var parent = Path.GetDirectoryName(folderPath)?.Replace("\\", "/");
            if (!string.IsNullOrEmpty(parent) && !AssetDatabase.IsValidFolder(parent))
            {
                CreateFolderRecursive(parent);
            }

            var folderName = Path.GetFileName(folderPath);
            if (!string.IsNullOrEmpty(parent) && !string.IsNullOrEmpty(folderName))
            {
                AssetDatabase.CreateFolder(parent, folderName);
            }
        }
    }
}

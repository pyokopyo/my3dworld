using System;
using System.IO;
using GLTFast;
using My3DWorld.Interfaces;
using UnityEngine;

namespace My3DWorld.World
{
    /// <summary>
    /// GLBファイルをGLTFastでランタイムロードし空間に生成するクラス（IGlbLoader実装）。
    /// Scripts/World/ に配置（設計規約準拠）。
    /// </summary>
    public class GlbSpawner : MonoBehaviour, IGlbLoader
    {
        /// <summary>
        /// 指定パスのGLBをロードして position に生成する。
        /// アーキテクチャ規約に従い async Awaitable を使用。
        /// </summary>
        public async Awaitable LoadAndSpawnGlbAsync(string filePath, Vector3 position)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"[GlbSpawner] ファイルが見つかりません: {filePath}");
                return;
            }

            // ローカルファイルパスを file:// URI に変換（WindowsパスをGLTFastに渡すため）
            var uriString = new Uri(Path.GetFullPath(filePath)).AbsoluteUri;

            var gltf = new GltfImport();

            bool loaded = await gltf.Load(uriString);
            if (!loaded)
            {
                Debug.LogError($"[GlbSpawner] GLBの読み込みに失敗しました: {filePath}");
                return;
            }

            var root = new GameObject(Path.GetFileNameWithoutExtension(filePath));
            root.transform.position = position;

            bool instantiated = await gltf.InstantiateMainSceneAsync(root.transform);
            if (!instantiated)
            {
                Debug.LogError($"[GlbSpawner] GLBのインスタンス化に失敗しました: {filePath}");
                Destroy(root);
                return;
            }

            Debug.Log($"[GlbSpawner] GLBを生成しました: {Path.GetFileName(filePath)} @ {position}");
        }
    }
}

using My3DWorld.Interfaces;
using UnityEngine;

namespace My3DWorld.World
{
    /// <summary>
    /// 空間オブジェクト生成クラス（ISpaceGenerator実装）。
    /// Scripts/World/ に配置（設計規約準拠）。
    /// </summary>
    public class SpaceGenerator : MonoBehaviour, ISpaceGenerator
    {
        [Header("生成するPrefab")] public GameObject spawnPrefab;

        public void SpawnObject(Vector3 position)
        {
            if (spawnPrefab == null)
            {
                Debug.LogWarning("[SpaceGenerator] spawnPrefab が未設定です。");
                return;
            }
            Instantiate(spawnPrefab, position, Quaternion.identity);
        }
    }
}

using UnityEngine;

namespace My3DWorld.Interfaces
{
    /// <summary>
    /// 空間オブジェクト生成のインターフェース。
    /// </summary>
    public interface ISpaceGenerator
    {
        void SpawnObject(Vector3 position);
    }
}

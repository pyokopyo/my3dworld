using UnityEngine;

namespace My3DWorld.Interfaces
{
    /// <summary>
    /// GLBファイルをランタイムでロードして空間に生成するインターフェース。
    /// </summary>
    public interface IGlbLoader
    {
        /// <summary>
        /// 指定パスのGLBファイルをロードし、positionに生成する。
        /// </summary>
        Awaitable LoadAndSpawnGlbAsync(string filePath, Vector3 position);
    }
}

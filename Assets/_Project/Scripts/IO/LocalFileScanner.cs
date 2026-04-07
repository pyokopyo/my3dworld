using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace My3DWorld.IO
{
    /// <summary>
    /// ローカルフォルダのGLBファイルを列挙するユーティリティ。
    /// </summary>
    public static class LocalFileScanner
    {
        /// <summary>
        /// 指定フォルダ直下の .glb ファイルパス一覧を返す。
        /// フォルダが存在しない場合は空リストを返す。
        /// </summary>
        public static IReadOnlyList<string> ScanForGlb(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Debug.LogWarning($"[LocalFileScanner] フォルダが存在しません: {folderPath}");
                return System.Array.Empty<string>();
            }

            return Directory.GetFiles(folderPath, "*.glb", SearchOption.TopDirectoryOnly);
        }
    }
}

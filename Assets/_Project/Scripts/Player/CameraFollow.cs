using UnityEngine;

namespace My3DWorld.Player
{
    /// <summary>
    /// 一人称視点用：カメラをプレイヤーの目の高さに固定するスクリプト。
    /// Main CameraをPlayerの子オブジェクトにし、このスクリプトをアタッチすること。
    /// 視点回転はPlayerControllerが担当する。
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        [Header("目の高さ（ローカルY）")] public float eyeHeight = 1.7f;

        void Start()
        {
            transform.localPosition = new Vector3(0f, eyeHeight, 0f);
            transform.localRotation = Quaternion.identity;
        }
    }
}

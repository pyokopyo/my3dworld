using UnityEngine;
using UnityEngine.InputSystem;

namespace My3DWorld.Player
{
    /// <summary>
    /// 一人称視点：WASD移動 + マウスで視点回転を担当するプレイヤーコントローラー（SRP準拠）。
    /// カメラはプレイヤーの子オブジェクトとして設定すること。
    /// 空間生成はISpaceGenerator / SpaceGeneratorが担当する。
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [Header("移動速度")] public float moveSpeed = 5f;
        [Header("マウス感度")] public float mouseSensitivity = 100f;
        [Header("カメラTransform（子オブジェクトのMain Camera）")] public Transform cameraTransform;
        [Header("ピッチ最小角度")] public float minPitch = -80f;
        [Header("ピッチ最大角度")] public float maxPitch = 80f;

        /// <summary>視点移動・プレイヤー操作の有効フラグ。ログイン後にtrueにすること。</summary>
        [System.NonSerialized] public bool LookEnabled = false;

        /// <summary>ESCを押す直前のLookEnabled値。QuitConfirmationViewがキャンセル時に参照する。</summary>
        [System.NonSerialized] public bool LookEnabledBeforeQuit = false;

        private float _pitch;

        void Awake()
        {
            Instance = this;
            // 起動時はカーソルを解放（ログイン画面対応）
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void Update()
        {
            // 視点操作が無効なら何もしない
            if (!LookEnabled) return;

            // WASD移動 + Space/Ctrl で高さ移動
            if (Keyboard.current != null)
            {
                float h = 0f, v = 0f, y = 0f;
                if (Keyboard.current.aKey.isPressed) h -= 1f;
                if (Keyboard.current.dKey.isPressed) h += 1f;
                if (Keyboard.current.wKey.isPressed) v += 1f;
                if (Keyboard.current.sKey.isPressed) v -= 1f;
                if (Keyboard.current.spaceKey.isPressed) y += 1f;
                if (Keyboard.current.leftCtrlKey.isPressed) y -= 1f;

                Vector3 move = (transform.right * h + transform.forward * v + Vector3.up * y) * moveSpeed * Time.deltaTime;
                transform.Translate(move, Space.World);
            }

            // マウスで視点回転
            if (Mouse.current != null)
            {
                float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
                float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

                // 水平回転：プレイヤー本体を回す
                transform.Rotate(0f, mouseX, 0f);

                // 垂直回転：カメラのみをピッチ（上下）
                _pitch = Mathf.Clamp(_pitch - mouseY, minPitch, maxPitch);
                if (cameraTransform != null)
                    cameraTransform.localEulerAngles = new Vector3(_pitch, 0f, 0f);
            }

            // Escキーで視点無効化・カーソル解放（QuitConfirmationView用に前状態を保存）
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                LookEnabledBeforeQuit = LookEnabled;
                LookEnabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}

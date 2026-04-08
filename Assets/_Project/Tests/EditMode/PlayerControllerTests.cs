using NUnit.Framework;
using UnityEngine;
using My3DWorld.Player;

namespace My3DWorld.Tests.EditMode
{
    /// <summary>
    /// PlayerController の純粋関数部分を対象とした Edit Mode テスト。
    /// Unity Test Runner → Edit Mode で実行してください。
    /// </summary>
    public class PlayerControllerTests
    {
        // ─── ClampPitch ───────────────────────────────────────────────

        [Test]
        public void ClampPitch_通常範囲内_そのまま返る()
        {
            float result = PlayerController.ClampPitch(0f, 10f, -80f, 80f);
            Assert.AreEqual(-10f, result, 0.001f);
        }

        [Test]
        public void ClampPitch_上限超過_最大値にクランプされる()
        {
            float result = PlayerController.ClampPitch(75f, -20f, -80f, 80f); // 75 - (-20) = 95 → 80
            Assert.AreEqual(80f, result, 0.001f);
        }

        [Test]
        public void ClampPitch_下限超過_最小値にクランプされる()
        {
            float result = PlayerController.ClampPitch(-75f, 20f, -80f, 80f); // -75 - 20 = -95 → -80
            Assert.AreEqual(-80f, result, 0.001f);
        }

        [Test]
        public void ClampPitch_デルタゼロ_変化なし()
        {
            float result = PlayerController.ClampPitch(30f, 0f, -80f, 80f);
            Assert.AreEqual(30f, result, 0.001f);
        }

        // ─── CalcMoveVector ───────────────────────────────────────────

        [Test]
        public void CalcMoveVector_前方移動_forwardベクトル方向に進む()
        {
            Vector3 right   = Vector3.right;
            Vector3 forward = Vector3.forward;

            Vector3 result = PlayerController.CalcMoveVector(right, forward, 0f, 1f, 0f, 5f, 1f);

            Assert.AreEqual(Vector3.forward * 5f, result);
        }

        [Test]
        public void CalcMoveVector_右移動_rightベクトル方向に進む()
        {
            Vector3 right   = Vector3.right;
            Vector3 forward = Vector3.forward;

            Vector3 result = PlayerController.CalcMoveVector(right, forward, 1f, 0f, 0f, 5f, 1f);

            Assert.AreEqual(Vector3.right * 5f, result);
        }

        [Test]
        public void CalcMoveVector_上昇_Vector3upに進む()
        {
            Vector3 right   = Vector3.right;
            Vector3 forward = Vector3.forward;

            Vector3 result = PlayerController.CalcMoveVector(right, forward, 0f, 0f, 1f, 5f, 1f);

            Assert.AreEqual(Vector3.up * 5f, result);
        }

        [Test]
        public void CalcMoveVector_入力なし_ゼロベクトル()
        {
            Vector3 result = PlayerController.CalcMoveVector(
                Vector3.right, Vector3.forward, 0f, 0f, 0f, 5f, 1f);

            Assert.AreEqual(Vector3.zero, result);
        }

        [Test]
        public void CalcMoveVector_速度スケール_deltaTimeに比例する()
        {
            Vector3 r1 = PlayerController.CalcMoveVector(Vector3.right, Vector3.forward, 0f, 1f, 0f, 10f, 0.5f);
            Vector3 r2 = PlayerController.CalcMoveVector(Vector3.right, Vector3.forward, 0f, 1f, 0f, 10f, 1.0f);

            Assert.AreEqual(r1 * 2f, r2);
        }
    }
}

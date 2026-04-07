namespace My3DWorld.Player
{
    /// <summary>
    /// プレイヤーコントローラーのインターフェース（設計規約に準拠）
    /// </summary>
    public interface IPlayerController
    {
        void Move(float horizontal, float vertical);
        void SpawnObject();
    }
}

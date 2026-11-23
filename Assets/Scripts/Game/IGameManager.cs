public interface IGameManager : IAccessTarget
{
    void HandlePlayStart();
    void HandleFailure();
    void HandleSuccess();
    void HandleSceneEnd();
}

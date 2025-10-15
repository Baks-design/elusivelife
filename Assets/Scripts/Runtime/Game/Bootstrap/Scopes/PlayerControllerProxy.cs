using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using VContainer.Unity;

public class PlayerControllerProxy : IPlayerController, ITickable, ILateTickable
{
    private readonly PlayerControllerFactory _factory;
    private IPlayerController _currentController;

    public PlayerControllerProxy(PlayerControllerFactory factory) => _factory = factory;

    public void Tick()
    {
        _currentController ??= _factory.GetOrCreatePlayerController();
        if (_currentController is ITickable tickable)
            tickable.Tick();
    }

    public void LateTick()
    {
        if (_currentController is ILateTickable lateTickable)
            lateTickable.LateTick();
    }
}
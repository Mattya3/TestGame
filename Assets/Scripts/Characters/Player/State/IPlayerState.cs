using UnityEngine;

public interface IPlayerState
{
}

public abstract class PlayableState : IPlayerState
{
    private PlayerActionConfiguration _configuration;    
}

public abstract class UnPlayableState : IPlayerState
{
}

public class GroundState: PlayableState
{
    public void OnJump(){
        _ApplyJump();
        _sounds.OnJump();
        _configuration.ChangeState(new AirState());
    }

    public void _Move(){
        Vector2 convertedDirection = MoveController.ConvertInputDirection(_inputDirection);
        _configuration.ApplyMovement(convertedDirection);
    }
}

public class AirState: PlayableState
{
    public void OnJump(){
        _ApplyJump();
        _sounds.OnJump();
        _configuration.ChangeState(new AirState());
    }

    public void _Move(){
        Vector2 convertedDirection = MoveController.ConvertInputDirection(_inputDirection);
        _configuration._ApplyMovement(convertedDirection);
    }   
}
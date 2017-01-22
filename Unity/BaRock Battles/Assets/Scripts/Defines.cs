using InputWrapper;

public class Defines
{
    public const EKeyMode KEY_MODE = EKeyMode.EKeyMode_PrioritizeController;
    public const int      KEY_MAXCONTROLLERS = 5;
    
    public const float PLAYER_MINY = -10.0f;

    public const int AUDIO_NUMRESERVEDCHANNELS = 16;

    public enum EPlayerType
    {
        heavy,
        medium,
        light,
        strange
    };
}

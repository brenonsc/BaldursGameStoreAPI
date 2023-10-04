namespace BaldursGame.Security;

public class Settings
{
    private static string secret = "aa82cac0a767d382340ef5cf1d0a2112468ab31ec6008a9467383ffa7b171262";
    
    public static string Secret
    {
        get => secret;
        set => secret = value;
    }
}


public class Mail
{
    private static string _email;
    public static string Email
    {
        get { return _email;}
        set
        {
            _email = value.Replace("@", "(at)");
            _email = value.Replace(".", "(dot)");
        }
    }
    
    public static bool IsSkiped;
}

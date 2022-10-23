namespace Pennywize.CLI;

public static class Rejectors
{
    public static Action<Void> RejectText(string text) => (Void _) => Console.WriteLine(text);
}
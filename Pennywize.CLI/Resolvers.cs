using ConsoleTables;

namespace Pennywize.CLI;

public static class Resolvers
{
    public static void ResolveText(string text) => Console.WriteLine(text);
    public static void ResolveRow<T>(T model) => ResolveTable<T>(new T[] { model });
    public static void ResolveTable<T>(IEnumerable<T> models) => ConsoleTable.From<T>(models).Write();
    public static Action<Void> ResolveDone(string message) => _ => ResolveText(message);
}
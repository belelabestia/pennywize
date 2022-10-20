using System.CommandLine;
using System.CommandLine.Invocation;

public static class Utils
{
    public static RootCommand RootCommand(
        string description,
        IEnumerable<Command> commands)
    {
        var root = new RootCommand(description);

        foreach (var c in commands) root.AddCommand(c);

        return root;
    }

    public static Command Command(
        string name,
        string description,
        Action? handle = null,
        IEnumerable<Command>? commands = null,
        IEnumerable<Option>? options = null,
        IEnumerable<Argument>? arguments = null)
    {
        var command = new Command(name, description);

        foreach (var c in commands ?? Enumerable.Empty<Command>()) command.AddCommand(c);
        foreach (var o in options ?? Enumerable.Empty<Option>()) command.AddOption(o);
        foreach (var a in arguments ?? Enumerable.Empty<Argument>()) command.AddArgument(a);

        if (handle is not null) command.SetHandler(handle);

        return command;
    }

    public static Command Command<T>(
        string name,
        string description,
        Action<T> handle,
        Func<InvocationContext, T> bind,
        IEnumerable<Command>? commands = null,
        IEnumerable<Option>? options = null,
        IEnumerable<Argument>? arguments = null)
    {
        var command = new Command(name, description);

        foreach (var c in commands ?? Enumerable.Empty<Command>()) command.AddCommand(c);
        foreach (var o in options ?? Enumerable.Empty<Option>()) command.AddOption(o);
        foreach (var a in arguments ?? Enumerable.Empty<Argument>()) command.AddArgument(a);

        command.SetHandler(context => handle(bind(context)));

        return command;
    }

    public static Option<T> Option<T>(
        string name,
        string description,
        bool isRequired = false,
        Func<object?>? defaultFactory = null)
    {
        var option = new Option<T>(name, description) { IsRequired = isRequired };

        if (defaultFactory is not null) option.SetDefaultValueFactory(defaultFactory);

        return option;
    }
}
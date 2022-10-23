namespace Pennywize.CLI;

using System.CommandLine;
using System.CommandLine.Invocation;

/// <summary>
/// This module implements the main Pennywize CLI API.
/// </summary>
public static class CommandLine
{
    /// <summary>
    /// Creates an instance of Pennywize CLI root command.
    /// </summary>
    /// <param name="description"></param>
    /// <param name="commands"></param>
    /// <returns></returns>
    public static RootCommand RootCommand(
        string description,
        IEnumerable<Command> commands)
    {
        var root = new RootCommand(description);
        foreach (var c in commands) root.AddCommand(c);
        return root;
    }

    /// <summary>
    /// Creates a command with no handler.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="commands"></param>
    /// <param name="options"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public static Command Command(
        string name,
        string description,
        IEnumerable<Command>? commands = null,
        IEnumerable<Option>? options = null,
        IEnumerable<Argument>? arguments = null) => Command(
            name,
            description,
            handle: justSucceed,
            resolve: doNothing,
            reject: doNothing,
            commands,
            options,
            arguments);

    /// <summary>
    /// Creates a command with a parameterless handler.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="handle"></param>
    /// <param name="resolve"></param>
    /// <param name="reject"></param>
    /// <param name="commands"></param>
    /// <param name="options"></param>
    /// <param name="arguments"></param>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TErr"></typeparam>
    /// <returns></returns>
    public static Command Command<TOut, TErr>(
        string name,
        string description,
        Func<Result<TOut, TErr>> handle,
        Action<TOut> resolve,
        Action<TErr> reject,
        IEnumerable<Command>? commands = null,
        IEnumerable<Option>? options = null,
        IEnumerable<Argument>? arguments = null) => Command<InvocationContext, TOut, TErr>(
            name,
            description,
            bind: context => context,
            handle: _ => handle(),
            resolve: output => resolve(output),
            reject: err => reject(err),
            commands,
            options,
            arguments);

    /// <summary>
    /// Creates a command with a handler that accepts a typed parameter.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="bind"></param>
    /// <param name="handle"></param>
    /// <param name="resolve"></param>
    /// <param name="reject"></param>
    /// <param name="commands"></param>
    /// <param name="options"></param>
    /// <param name="arguments"></param>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TErr"></typeparam>
    /// <returns></returns>
    public static Command Command<TIn, TOut, TErr>(
        string name,
        string description,
        Func<InvocationContext, TIn> bind,
        Func<TIn, Result<TOut, TErr>> handle,
        Action<TOut> resolve,
        Action<TErr> reject,
        IEnumerable<Command>? commands = null,
        IEnumerable<Option>? options = null,
        IEnumerable<Argument>? arguments = null)
    {
        var command = new Command(name, description);

        foreach (var c in commands ?? Enumerable.Empty<Command>()) command.AddCommand(c);
        foreach (var o in options ?? Enumerable.Empty<Option>()) command.AddOption(o);
        foreach (var a in arguments ?? Enumerable.Empty<Argument>()) command.AddArgument(a);

        var commandHandler = getCommandHandler(bind, handle, resolve, reject);
        command.SetHandler(commandHandler);

        return command;
    }

    /// <summary>
    /// Creates a typed Option.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="isRequired"></param>
    /// <param name="defaultFactory"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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

    /// <summary>
    /// Creates a Command handler given its dependencies.
    /// </summary>
    /// <param name="bind"></param>
    /// <param name="handle"></param>
    /// <param name="resolve"></param>
    /// <param name="reject"></param>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TErr"></typeparam>
    /// <returns></returns>
    static Action<InvocationContext> getCommandHandler<TIn, TOut, TErr>(
        Func<InvocationContext, TIn> bind,
        Func<TIn, Result<TOut, TErr>> handle,
        Action<TOut> resolve,
        Action<TErr> reject) => context =>
    {
        var result = handle(bind(context));

        if (result.Success) resolve(result.Output!);
        else reject(result.Error!);
    };

    /// <summary>
    /// Represents a "null" () => Result function.
    /// </summary>
    /// <returns></returns>
    static Result<Void, Void> justSucceed() => Result.Success<Void>();

    /// <summary>
    /// Represents a "null" _ => { } function.
    /// </summary>
    /// <param name="_"></param>
    /// <typeparam name="T"></typeparam>
    static void doNothing<T>(T _) { }
}
namespace Pennywize.CLI;

using System.CommandLine;
using static CommandLine;

/// <summary>
/// This module provides the fully initialized root command tree.
/// </summary>
public static class Commands
{
    public static RootCommand GetRoot()
    {
        var transactions = getTransactionsCommand();
        return RootCommand("Pennywize - Personal finance", new[] { transactions });
    }

    static Command getTransactionsCommand()
    {
        var commands = getCommands();

        return Command(
            name: "transactions",
            description: "Handle transactions.",
            commands: commands);
    }

    static IEnumerable<Command> getCommands() => new[]
        {
            Command(
                name: "list",
                description: "List transactions.",
                handle: Result.From<IEnumerable<Transaction>, Void>(Transactions.List),
                resolve: Resolvers.ResolveTable,
                reject: Rejectors.RejectText("Operation failed")),
            getAddCommand(),
            getUpdateCommand(),
            getRemoveCommand()
        };

    static Command getAddCommand()
    {
        var (amount, note, dateTime) = getAddOptions();

        return Command(
            name: "add",
            description: "Add transaction.",
            bind: Binders.BindTransaction(amount, note, dateTime),
            handle: Result.From<Transaction, Transaction, Void>(Transactions.Add),
            resolve: Resolvers.ResolveRow,
            reject: Rejectors.RejectText("Operation failed."),
            options: new Option[] { amount, note, dateTime });
    }

    static Command getUpdateCommand()
    {
        var (id, amount, note, dateTime) = getUpdateOptions();

        return Command(
            name: "update",
            description: "Update transaction.",
            bind: Binders.BindTransaction(amount, note, dateTime, id),
            handle: Result.From<Transaction, Transaction, Void>(Transactions.Update),
            resolve: Resolvers.ResolveRow,
            reject: Rejectors.RejectText("Operation failed."),
            options: new Option[] { id, amount, note, dateTime });
    }

    static Command getRemoveCommand()
    {
        var id = getRemoveOptions();

        return Command(
            name: "remove",
            description: "Remove transaction.",
            bind: Binders.BindId(id),
            handle: Result.From<string, Void>(Transactions.Remove),
            resolve: Resolvers.ResolveDone("Transaction removed."),
            reject: Rejectors.RejectText("Operation failed."),
            options: new Option[] { id });
    }

    static (Option<decimal>, Option<string>, Option<DateTime>) getAddOptions() => (
        Option<decimal>(
            name: "--amount",
            description: "The amount of the transaction.",
            isRequired: true),
        Option<string>(
            name: "--note",
            description: "A note about the transaction.",
            defaultFactory: () => string.Empty),
        Option<DateTime>(
            name: "--date-time",
            description: "Date and time of the transaction",
            defaultFactory: () => DateTime.Now));

    static (Option<string>, Option<decimal>, Option<string>, Option<DateTime>) getUpdateOptions() => (
        Option<string>(
            name: "--id",
            description: "The transaction id.",
            isRequired: true),
        Option<decimal>(
            name: "--amount",
            description: "The amount of the transaction."),
        Option<string>(
            name: "--note",
            description: "A note about the transaction.",
            defaultFactory: () => string.Empty),
        Option<DateTime>(
            name: "--date-time",
            description: "Date and time of the transaction",
            defaultFactory: () => DateTime.Now));

    static Option<string> getRemoveOptions() => Option<string>(
        name: "--id",
        description: "The transaction id.",
        isRequired: true);
}
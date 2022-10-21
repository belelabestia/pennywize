using System.CommandLine;
using static Utils;

namespace Pennywize.CLI;

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
                handle: Transactions.List),
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
            handle: Transactions.Add,
            bind: Transactions.Bind(amount, note, dateTime),
            options: new Option[] { amount, note, dateTime });
    }

    static Command getUpdateCommand()
    {
        var (id, amount, note, dateTime) = getUpdateOptions();

        return Command(
            name: "update",
            description: "Update transaction.",
            handle: Transactions.Update,
            bind: Transactions.Bind(amount, note, dateTime, id),
            options: new Option[] { id, amount, note, dateTime });
    }

    static Command getRemoveCommand()
    {
        var id = getRemoveOptions();

        return Command(
            name: "remove",
            description: "Remove transaction.",
            handle: Transactions.Remove,
            bind: Transactions.BindId(id),
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
        description: "The transaction id.");
}
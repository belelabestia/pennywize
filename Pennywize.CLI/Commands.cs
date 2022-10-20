using System.CommandLine;
using Pennywize.CLI.Transactions;
using static Utils;

public static class Commands
{
    public static RootCommand GetRoot()
    {
        var transactions = getTransactionsCommand();
        return RootCommand("Pennywize - Personal finance", new[] { transactions });
    }

    static Command getTransactionsCommand()
    {
        var add = getAddCommand();
        var list = getListCommand();

        var transactions = Command(
            name: "transactions",
            description: "Handle transactions.",
            commands: new[] { list, add });

        return transactions;
    }

    static Command getListCommand() => Command(
        name: "list",
        description: "List transactions.",
        handle: Handlers.List);

    static Command getAddCommand()
    {
        var amount = Option<decimal>(
            name: "--amount",
            description: "The amount of the transaction.",
            isRequired: true);

        var note = Option<string>(
            name: "--note",
            description: "A note about the transaction.",
            defaultFactory: () => string.Empty);

        var dateTime = Option<DateTime>(
            name: "--date-time",
            description: "Date and time of the transaction",
            defaultFactory: () => DateTime.Now);

        return Command(
            name: "add",
            description: "Add transaction.",
            handle: Handlers.Add,
            bind: Binders.Transaction(amount, note, dateTime),
            options: new Option[] { amount, note, dateTime }
        );
    }
}
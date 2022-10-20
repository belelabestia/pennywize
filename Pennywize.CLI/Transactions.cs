using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using Pennywize.Core;
using T = Pennywize.Core.Transactions;

namespace Pennywize.CLI.Transactions;

public static class Handlers
{
    public static void List()
    {
        var transactions = T.List();
        var output = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });

        Console.WriteLine("Transactions");
        Console.WriteLine(output);
    }

    public static void Add(Transaction transaction)
    {
        var id = T.Add(transaction);
        var output = JsonSerializer.Serialize(transaction with { Id = id }, new JsonSerializerOptions { WriteIndented = true });

        Console.WriteLine($"Transaction created");
        Console.WriteLine(output);
    }
}

public static class Binders
{
    public static Func<InvocationContext, Transaction> Transaction(
        Option<decimal> amount,
        Option<string> note,
        Option<DateTime> dateTime) => context => new Transaction(
            Amount: context.ParseResult.GetValueForOption(amount),
            Note: context.ParseResult.GetValueForOption(note)!,
            DateTime: context.ParseResult.GetValueForOption(dateTime));
}
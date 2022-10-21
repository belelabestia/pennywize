using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using LiteDB;
using Pennywize.Core;
using JSON = System.Text.Json.JsonSerializer;
using T = Pennywize.Core.Transactions;

namespace Pennywize.CLI;

public static class Transactions
{
    public static Func<InvocationContext, Transaction> Bind(
        Option<decimal> amount,
        Option<string> note,
        Option<DateTime> dateTime,
        Option<string>? id = null) => context =>
        {
            var transaction = new Transaction(
                Amount: context.ParseResult.GetValueForOption(amount),
                Note: context.ParseResult.GetValueForOption(note)!,
                DateTime: context.ParseResult.GetValueForOption(dateTime));

            return id is null ?
                transaction :
                transaction with { Id = BindId(id)(context) };
        };

    public static Func<InvocationContext, ObjectId> BindId(Option<string> id) => context => new ObjectId(context.ParseResult.GetValueForOption(id));

    public static void List()
    {
        var transactions = T.List();
        var output = JSON.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });

        Console.WriteLine($"Transactions ({transactions.Count()})");
        Console.WriteLine(output);
    }

    public static void Add(Transaction transaction)
    {
        var id = T.Add(transaction);
        var output = JSON.Serialize(transaction with { Id = id }, new JsonSerializerOptions { WriteIndented = true });

        Console.WriteLine("Transaction created");
        Console.WriteLine(output);
    }

    public static void Update(Transaction transaction)
    {
        T.Update(transaction);
        Console.WriteLine("Transaction updated.");
    }

    public static void Remove(ObjectId id)
    {
        T.Remove(id);
        Console.WriteLine("Transaction removed.");
    }
}
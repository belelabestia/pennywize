using LiteDB;

namespace Pennywize.Core;
using static Database;


public record Transaction(
    decimal Amount,
    string Note,
    DateTime DateTime)
{
    public ObjectId? Id { get; init; }
    public bool IsSaved => Id is not null;
    public bool IsNew => !IsSaved;
}

public static class Transactions
{
    public static List<Transaction> List() =>
        Use<Transaction, List<Transaction>>(t => t.FindAll().ToList());

    public static ObjectId Add(Transaction transaction) =>
        Use<Transaction, ObjectId>((t) => t.Insert(transaction));

    public static void Edit(Transaction transaction) =>
        Use<Transaction>(t => t.Update(transaction));

    public static void Remove(ObjectId transactionId) =>
        Use<Transaction>(t => t.Delete(transactionId));
}
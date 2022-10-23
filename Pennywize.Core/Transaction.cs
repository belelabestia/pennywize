namespace Pennywize.Core;

using LiteDB;
using static Database;


public record Transaction(
    ObjectId? Id,
    decimal Amount,
    string Note,
    DateTime DateTime);

public static class Transactions
{
    public static List<Transaction> List() =>
        Use<Transaction, List<Transaction>>(t => t.FindAll().ToList());

    public static ObjectId Add(Transaction transaction) =>
        Use<Transaction, ObjectId>((t) => t.Insert(transaction));

    public static void Update(Transaction transaction) =>
        Use<Transaction>(t => t.Update(transaction));

    public static void Remove(ObjectId transactionId) =>
        Use<Transaction>(t => t.Delete(transactionId));
}
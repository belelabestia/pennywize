namespace Pennywize.CLI;

using LiteDB;
using C = Pennywize.Core;
using T = Pennywize.Core.Transactions;

public record Transaction(
    string? Id,
    decimal Amount,
    string Note,
    DateTime DateTime);

/// <summary>
/// This module contains 
/// </summary>
public static class Transactions
{
    /// <summary>
    /// Shows all the transactions.
    /// </summary>
    public static IEnumerable<Transaction> List() => T.List().Select(ToCliTransaction);

    /// <summary>
    /// Adds a new transaction.
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    public static Transaction Add(Transaction transaction)
    {
        var id = T.Add(transaction.ToCoreTransaction());
        return transaction with { Id = id.ToString() };
    }

    /// <summary>
    /// Updates an existing transaction.
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    public static Transaction Update(Transaction transaction)
    {
        T.Update(transaction.ToCoreTransaction());
        return transaction;
    }

    /// <summary>
    /// Deletes an existing transaction.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static void Remove(string id) => T.Remove(id.ToObjectId());

    /// <summary>
    /// Maps from CLI Transaction to Core Transaction.
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    static C.Transaction ToCoreTransaction(this Transaction transaction) => new C.Transaction(
        transaction.Id?.ToObjectId(),
        transaction.Amount,
        transaction.Note,
        transaction.DateTime);

    /// <summary>
    /// Maps from Core Transaction to CLI Transaction.
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    static Transaction ToCliTransaction(this C.Transaction transaction) => new Transaction(
        transaction.Id?.ToString(),
        transaction.Amount,
        transaction.Note,
        transaction.DateTime);

    /// <summary>
    /// Maps from string to ObjectId.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    static ObjectId ToObjectId(this string id) => new ObjectId(id);
}
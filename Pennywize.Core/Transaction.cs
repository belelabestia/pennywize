using LiteDB;
using static Pennywize.Core.Database;

namespace Pennywize.Core;

public record SavedTransaction(
    ObjectId _id,
    decimal Amount,
    string Note,
    DateTime DateTime);

public record BaseTransaction(
    decimal Amount,
    string Note,
    DateTime DateTime);

public static class Transactions
{
    public static List<SavedTransaction> List() =>
        Use<SavedTransaction, List<SavedTransaction>>(t => t.FindAll().ToList());

    public static ObjectId Add(BaseTransaction transaction) =>
        Use<SavedTransaction, ObjectId>(t => t.Insert(transaction.WithId()));

    public static void Edit(SavedTransaction transaction) =>
        Use<SavedTransaction>(t => t.Update(transaction));

    public static void Remove(ObjectId transactionId) =>
        Use<SavedTransaction>(t => t.Delete(transactionId));

    public static SavedTransaction WithId(this BaseTransaction t, ObjectId? objectId = null) => new(
        _id: objectId ?? ObjectId.NewObjectId(),
        Amount: t.Amount,
        Note: t.Note,
        DateTime: t.DateTime);
}
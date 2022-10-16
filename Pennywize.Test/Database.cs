using LiteDB;
using Pennywize.Core;

namespace Pennywize.Test;

public class Transactions_Test
{
    BaseTransaction t1 => new(
         Amount: 10,
         Note: "Some note",
         DateTime: DateTime.Now.ToSavedDateTime());

    BaseTransaction t2 => new(
        Amount: 11,
        Note: "Some other note",
        DateTime: DateTime.Now.AddDays(2).ToSavedDateTime());

    [Fact]
    public void ShouldListTransactions()
    {
        Database.Clear();

        var id1 = Transactions.Add(t1);
        var id2 = Transactions.Add(t2);

        var expected = new List<SavedTransaction>
        {
            t1.WithId(id1),
            t2.WithId(id2)
        };

        var actual = Transactions.List();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldAddTransaction()
    {
        Database.Clear();

        var id = Transactions.Add(t1);
        var res = Transactions.List();

        var expected = t1.WithId(id);
        var actual = res.First();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldEditTransaction()
    {
        Database.Clear();

        var id1 = Transactions.Add(t1);

        var expected = t1.WithId(id1) with { Note = "New updated note" };

        Transactions.Edit(expected);

        var actual = Transactions.List().Single();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldRemoveTransaction()
    {
        Database.Clear();

        var id1 = Transactions.Add(t1);
        var id2 = Transactions.Add(t2);

        Transactions.Remove(id1);

        var expected = new List<SavedTransaction>
        {
            t2.WithId(id2)
        };

        var actual = Transactions.List();

        Assert.Equal(expected, actual);
    }
}
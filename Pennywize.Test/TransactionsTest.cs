using Pennywize.Core;

public class TransactionsTest
{
    Transaction t1 = new(
        Amount: 10,
        Note: "Some note",
        DateTime: DateTime.Now.ToSavedDateTime());

    Transaction t2 = new(
        Amount: 11,
        Note: "Some other note",
        DateTime: DateTime.Now.AddDays(2).ToSavedDateTime());

    [Fact]
    public void ShouldListTransactions()
    {
        Database.Clear();

        var id1 = Transactions.Add(t1);
        var id2 = Transactions.Add(t2);

        var expected = new List<Transaction>
        {
            t1 with { Id = id1 },
            t2 with { Id = id2 }
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

        var expected = t1 with { Id = id };
        var actual = res.First();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldEditTransaction()
    {
        Database.Clear();

        var id1 = Transactions.Add(t1);

        var expected = t1 with { Id = id1 } with { Note = "New updated note" };

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

        var expected = new List<Transaction>
        {
            t2 with { Id = id2 }
        };

        var actual = Transactions.List();

        Assert.Equal(expected, actual);
    }
}
namespace Pennywize.CLI;

using System.CommandLine;
using System.CommandLine.Invocation;

/// <summary>
/// This module contains binders between System.CommandLine.Option tuples and the actual models.
/// </summary>
public static class Binders
{
    /// <summary>
    /// Binds a string parameter.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Func<InvocationContext, string> BindId(Option<string> id) => context => context.ParseResult.GetValueForOption(id)!;

    /// <summary>
    /// Binds a Transaction model.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="note"></param>
    /// <param name="dateTime"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Func<InvocationContext, Transaction> BindTransaction(
        Option<decimal> amount,
        Option<string> note,
        Option<DateTime> dateTime,
        Option<string>? id = null) => context => new Transaction(
            Id: id is null ? null : context.ParseResult.GetValueForOption(id),
            Amount: context.ParseResult.GetValueForOption(amount),
            Note: context.ParseResult.GetValueForOption(note)!,
            DateTime: context.ParseResult.GetValueForOption(dateTime));
}
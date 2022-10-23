namespace Pennywize.CLI;

public struct Void { }
public record struct Result<TOut, TErr>(bool Success, TOut? Output, TErr? Error);

public static class Result
{
    public static Result<_TOut, Void> Success<_TOut>(_TOut? output = default) => new(true, output, default);
    public static Result<Void, _TErr> Error<_TErr>(_TErr error) => new(false, default, error);

    public static Func<Result<TOut, TErr>> From<TOut, TErr>(Func<TOut> func, TErr? error = default) => () =>
    {
        try { var res = func(); return new(true, res, default); }
        catch { return new(false, default, error); }
    };

    public static Func<TIn, Result<TOut, TErr>> From<TIn, TOut, TErr>(Func<TIn, TOut> func, TErr? error = default) => input => From(() => func(input), error)();
    public static Func<TIn, Result<Void, TErr>> From<TIn, TErr>(Action<TIn> action, TErr? error = default) => input => From<Void, TErr>(() => { action(input); return new Void(); })();
}


using LiteDB;

namespace Pennywize.Core;

public static class Database
{
    public static void Clear() => File.Delete(Config.DbName);

    public static TRet Use<TCol, TRet>(Func<ILiteCollection<TCol>, TRet> func)
    {
        using var db = new LiteDatabase(Config.DbName);
        var col = db.GetCollection<TCol>();
        return func(col);
    }

    public static void Use<TCol>(Action<ILiteCollection<TCol>> action)
    {
        using var db = new LiteDatabase(Config.DbName);
        var col = db.GetCollection<TCol>();
        action(col);
    }
}
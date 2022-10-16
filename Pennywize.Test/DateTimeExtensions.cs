using static LiteDB.JsonSerializer;

public static class DateTimeExtensions
{
    public static DateTime ToSavedDateTime(this DateTime dateTime) =>
        Deserialize(Serialize(dateTime));
}
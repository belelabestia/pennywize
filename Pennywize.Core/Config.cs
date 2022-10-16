public record Config(string DatabaseName)
{
    public static Config Default => new(@"pwz.db");
}
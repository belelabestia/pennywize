using System.Text.Json;
using Pennywize.Core;

public abstract record Route(string Command);
public record NodeRoute(string Command, Route[] Children) : Route(Command);
public record LeafRoute(string Command, Action Action) : Route(Command);
public record LeafRoute<TParam>(string Command, Action<TParam> Action) : Route(Command);

public static class Routing
{
    public static Route[] Routes => new Route[]
    {
        new NodeRoute(
            Command: "transactions",
            Children: new Route[]
            {
                new LeafRoute(
                    Command: "list",
                    Action: () => { Console.WriteLine(JsonSerializer.Serialize(Transactions.List())); }),
                new LeafRoute<BaseTransaction>(
                    Command: "add",
                    Action: _ => {}),
                new LeafRoute<SavedTransaction>(
                    Command: "edit",
                    Action: _ => {}),
                new LeafRoute(
                    Command: "rm",
                    Action: () => {})
            }),
        new LeafRoute(
            Command: "",
            Action: () => {})
    };

    public static LeafRoute? GetRoute(this Route[] routes, string[] args)
    {
        var command = args.First();
        var rest = args.Skip(1).ToArray();
        var maybeRoute = routes.SingleOrDefault(r => r.Command == command);

        if (args.Count() == 0) return null;
        if (maybeRoute is LeafRoute leaf) return leaf;
        if (maybeRoute is NodeRoute node) return GetRoute(node.Children, rest);

        return null;
    }
}
using Pennywize.Core;

public class RoutingTest
{
    [Fact]
    public void ShouldFindRoute()
    {
        var args = new string[] { "transactions", "list" };
        var actionRef = () => { };

        var routes = new Route[]
        {
            new NodeRoute(
                Command: "transactions",
                Children: new Route[]
                {
                    new LeafRoute(
                        Command: "list",
                        Action: actionRef),
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

        var result = routes.GetRoute(args);

        Assert.IsType<LeafRoute>(result);
        Assert.Equal(args[1], result!.Command);
        Assert.Equal(actionRef, ((LeafRoute)result!).Action);
    }
}
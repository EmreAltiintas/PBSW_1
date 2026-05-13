using GrassShop.Tests.Drivers;
using Reqnroll.BoDi;

namespace GrassShop.Tests.Support;

[Binding]
public class Hooks(IObjectContainer container)
{
    [BeforeScenario]
    public void BeforeScenario() =>
        container.RegisterInstanceAs(new LawnMowerDriver());

    [AfterScenario]
    public void AfterScenario(LawnMowerDriver driver) =>
        driver.Dispose();
}

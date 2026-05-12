using GrassShop.Core.Services.LawnMowerService.Models;
using GrassShop.Tests.Drivers;
using Xunit;

namespace GrassShop.Tests.StepDefinitions;

[Binding]
public class LawnMowerStepDefinitions(LawnMowerDriver driver)
{
    private LawnMowerModel? result;
    private IEnumerable<LawnMowerModel>? allResults;
    private bool deleteResult;
    private int createdId;

    [Given("no lawn mowers exist")]
    public void GivenNoLawnMowersExist() { }

    [Given("a lawn mower with name {string} exists")]
    public async Task GivenALawnMowerWithNameExists(string name)
    {
        var created = await driver.Service.CreateLawnMowerAsync(new CreateLawnMowerArgs
        {
            Name = name,
            Brand = "TestBrand",
            Description = "Test description",
            Price = 999.99m,
            Stock = 5
        });
        createdId = created.Id;
    }

    [When("I create a lawn mower with name {string}, brand {string}, description {string}, price {decimal} and stock {int}")]
    public async Task WhenICreateALawnMower(string name, string brand, string description, decimal price, int stock)
    {
        result = await driver.Service.CreateLawnMowerAsync(new CreateLawnMowerArgs
        {
            Name = name,
            Brand = brand,
            Description = description,
            Price = price,
            Stock = stock
        });
    }

    [When("I get all lawn mowers")]
    public async Task WhenIGetAllLawnMowers()
    {
        allResults = await driver.Service.GetAllLawnMowersAsync();
    }

    [When("I get the lawn mower by its ID")]
    public async Task WhenIGetTheLawnMowerByItsId()
    {
        result = await driver.Service.GetLawnMowerByIdAsync(createdId);
    }

    [When("I get the lawn mower with ID {int}")]
    public async Task WhenIGetTheLawnMowerWithId(int id)
    {
        result = await driver.Service.GetLawnMowerByIdAsync(id);
    }

    [When("I update the lawn mower name to {string}")]
    public async Task WhenIUpdateTheLawnMowerNameTo(string newName)
    {
        result = await driver.Service.UpdateLawnMowerAsync(createdId, new UpdateLawnMowerArgs
        {
            Name = newName,
            Brand = "TestBrand",
            Description = "Test description",
            Price = 999.99m,
            Stock = 5
        });
    }

    [When("I update the lawn mower with ID {int}")]
    public async Task WhenIUpdateTheLawnMowerWithId(int id)
    {
        result = await driver.Service.UpdateLawnMowerAsync(id, new UpdateLawnMowerArgs
        {
            Name = "Any Name",
            Brand = "Any Brand",
            Description = "Any description",
            Price = 0m,
            Stock = 0
        });
    }

    [When("I delete the lawn mower by its ID")]
    public async Task WhenIDeleteTheLawnMowerByItsId()
    {
        deleteResult = await driver.Service.DeleteLawnMowerAsync(createdId);
    }

    [When("I delete the lawn mower with ID {int}")]
    public async Task WhenIDeleteTheLawnMowerWithId(int id)
    {
        deleteResult = await driver.Service.DeleteLawnMowerAsync(id);
    }

    [Then("the created lawn mower should have name {string}")]
    public void ThenTheCreatedLawnMowerShouldHaveName(string expectedName)
    {
        Assert.NotNull(result);
        Assert.Equal(expectedName, result.Name);
    }

    [Then("the created lawn mower should have price {decimal}")]
    public void ThenTheCreatedLawnMowerShouldHavePrice(decimal expectedPrice)
    {
        Assert.NotNull(result);
        Assert.Equal(expectedPrice, result.Price);
    }

    [Then("the result should be an empty list")]
    public void ThenTheResultShouldBeAnEmptyList()
    {
        Assert.NotNull(allResults);
        Assert.Empty(allResults);
    }

    [Then("the result should contain {int} lawn mowers")]
    public void ThenTheResultShouldContain(int expectedCount)
    {
        Assert.NotNull(allResults);
        Assert.Equal(expectedCount, allResults.Count());
    }

    [Then("the lawn mower should be found with name {string}")]
    public void ThenTheLawnMowerShouldBeFoundWithName(string expectedName)
    {
        Assert.NotNull(result);
        Assert.Equal(expectedName, result.Name);
    }

    [Then("the lawn mower should not be found")]
    public void ThenTheLawnMowerShouldNotBeFound()
    {
        Assert.Null(result);
    }

    [Then("the updated lawn mower should have name {string}")]
    public void ThenTheUpdatedLawnMowerShouldHaveName(string expectedName)
    {
        Assert.NotNull(result);
        Assert.Equal(expectedName, result.Name);
    }

    [Then("the update result should be null")]
    public void ThenTheUpdateResultShouldBeNull()
    {
        Assert.Null(result);
    }

    [Then("the deletion should succeed")]
    public void ThenTheDeletionShouldSucceed()
    {
        Assert.True(deleteResult);
    }

    [Then("the lawn mower should no longer exist")]
    public async Task ThenTheLawnMowerShouldNoLongerExist()
    {
        var mower = await driver.Service.GetLawnMowerByIdAsync(createdId);
        Assert.Null(mower);
    }

    [Then("the deletion should fail")]
    public void ThenTheDeletionShouldFail()
    {
        Assert.False(deleteResult);
    }
}

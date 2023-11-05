using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyShop.Controllers;
using MyShop.DAL;
using MyShop.Models;
using MyShop.ViewModels;

namespace TestProject.Controllers
{
    public class ItemControllerTests
    {
        // The fact decorator indicates that this is a test case and when we run the test project, xUnit will find and run methods marked with Fact
        [Fact]
        // The method below is async and will test the Table() action method, the name TestTable() is not important
        public async Task TestTable()
        {
            // Phase 1: Arrange. This sets up the environment for the test; creating objects, settings initial conditions, preparing inputs required
            // for testing. The goal is to create an environment that mimicks the conditions in which the code will be executed.
            var itemList = new List<Item>()
        {
            new Item
            {
            ItemId = 1,
            Name = "Fried Chicken Wing",
            Price = 20,
            Description = "Delicious spicy chicken wing",
            ImageUrl = "/images/chickenwing.jpg"
            },
            new Item
            {
            ItemId = 2,
            Name = "Brown Cheese",
            Price = 20,
            Description = "Typical Norwegian cheese",
            ImageUrl = "/images/brunost.jpg"
            }
        };
            // Creates mock instances of the IItemRepository interface, mock logger, and a mock ItemController
            var mockItemRepository = new Mock<IItemRepository>();
            // The code just below the comments here is VERY IMPORTANT! This mock instance is setup so that the REAL IItemRepository.GetAll() is NOT
            // called during the unit test. This setup ensures that when the GetAll() method is called on the mock repo, it will return the itemList
            // that we defined above. This allows us to isolate the behaviour of the repo from the actual database and test the logic of our controller
            // independently.
            mockItemRepository.Setup(repo => repo.GetAll()).ReturnsAsync(itemList);
            var mockLogger = new Mock<ILogger<ItemController>>();
            var itemController = new ItemController(mockItemRepository.Object, mockLogger.Object);

            // Phase 2: Act. Execute the code or method you are testing and the purpose of this step is to trigger the behaviour you want to test
            // and obtain the result of the operation. 

            // Below is the actual execution of calling the ItemController.Table() method
            var result = await itemController.Table();

            // Phase 3: Assert. Check if the result of the code execution matches your expectations. If the actual outcome aligns with the expected outcome,
            // the test passes, if not it fails and we need to investigate why.

            // Below checks whether or not we have succeeded, it is a POSITIVE TEST.
            var viewResult = Assert.IsType<ViewResult>(result); // verifies that the result from Table() is a ViewResult
            var itemListViewModel = Assert.IsAssignableFrom<ItemListViewModel>(viewResult.ViewData.Model); // Verifies that the model in ViewResult is of type ItemListViewModel
            Assert.Equal(2, itemListViewModel.Items.Count()); // Verifies whether the amount of items in itemListViewModel is exactly 2.
            Assert.Equal(itemList, itemListViewModel.Items); // Verifies that the items returned by the Table() match the items in itemlist
        }

        [Fact]
        public async Task TestCreateNotOk()
        {
            // arrange
            var testItem = new Item
            {
                ItemId = 1,
                Price = 20,
                Description = "Delicious spicy chicken wing",
                ImageUrl = "/images/chickenwing.jpg"
            };
            var mockItemRepository = new Mock<IItemRepository>();

            // This is setup so that when Create() of IItemRepository is called, right below the act comment,
            // it will always return false.
            mockItemRepository.Setup(repo => repo.Create(testItem)).ReturnsAsync(false);
            var mockLogger = new Mock<ILogger<ItemController>>();
            var itemController = new ItemController(mockItemRepository.Object, mockLogger.Object);

            // act
            var result = await itemController.Create(testItem);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result); // Verifies that the result from Create() is a ViewResult
            var viewItem = Assert.IsAssignableFrom<Item>(viewResult.ViewData.Model); // Verifies that the model returned is of type ViewItem    
            Assert.Equal(testItem, viewItem); // Verifies the returned item should be equal to testItem
        }
    }
}

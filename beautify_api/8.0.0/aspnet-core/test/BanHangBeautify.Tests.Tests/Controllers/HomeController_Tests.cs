using System.Threading.Tasks;
using SSOFT.SPA.Models.TokenAuth;
using SSOFT.SPA.Web.Controllers;
using Shouldly;
using Xunit;

namespace SSOFT.SPA.Web.Tests.Controllers
{
    public class HomeController_Tests: SPAWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}
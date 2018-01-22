using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using BaseCore.Models;
using BaseCoreTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Controllers;
using Server.Dtos;

namespace ServerTests.Controllers
{
    [TestClass]
    public class CompetitionControllerTest
    {
        private CompetitionController _competitionController = new CompetitionController();

        [TestInitialize]
        public async Task Initialize()
        {
            IntegrationTests it = new IntegrationTests();
            await it.LoadCsvs();
        }

        [TestMethod]
        public async Task GetPaged()
        {
            PageBindingModel pageBindingModel = new PageBindingModel{PageNumber = 0, ItemsOnPage = 100};
            PageViewModel<CompetitionDto> pageViewModel =
                await _competitionController.Get(pageBindingModel);

            Assert.IsTrue(pageViewModel.TotalCount > 0 && pageViewModel.Items.Length > 0);
        }

        [TestMethod]
        public async Task GetItem()
        {
            OkNegotiatedContentResult<CompetitionDto> dto = await _competitionController.Get(1) as OkNegotiatedContentResult<CompetitionDto>; 

            Assert.IsNotNull(dto.Content);
        }
    }
}

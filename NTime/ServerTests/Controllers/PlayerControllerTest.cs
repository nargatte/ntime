using System;
using System.Threading.Tasks;
using System.Web.Http.Results;
using BaseCore.DataBase;
using BaseCore.Models;
using BaseCoreTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Controllers;
using Server.Dtos;
using Server.Models;

namespace ServerTests.Controllers
{
    [TestClass]
    public class PlayerControllerTest
    {
        private PlayerController _playerController = new PlayerController();

        [TestInitialize]
        public async Task Initialize()
        {
            IntegrationTests it = new IntegrationTests();
            await it.LoadCsvs();
        }

        [TestMethod]
        public async Task PostTakeSimpleListFromCompetition()
        {
            PageBindingModel bindingModel = new PageBindingModel{ItemsOnPage = 100, PageNumber = 0};
            PlayerFilterOptionsBindingModel filterOptionsBindingModel = new PlayerFilterOptionsBindingModel();
            OkNegotiatedContentResult<PageViewModel<PlayerListViewDto>> pageViewModel =
                await _playerController.PostTakeSimpleListFromCompetition(1, bindingModel, filterOptionsBindingModel) as
                    OkNegotiatedContentResult<PageViewModel<PlayerListViewDto>>;
            Assert.IsTrue(pageViewModel.Content.Items.Length > 0 && pageViewModel.Content.TotalCount > 0);
        }

        //[TestMethod]
        //public async Task PostRegisterIntoCompetition()
        //{
        //    PlayerCompetitionRegisterDto competitionRegisterDto = new PlayerCompetitionRegisterDto
        //    {
        //        BirthDate = new DateTime(1995, 3, 5),
        //        Id = 0,
        //        CompetitionId = 1,
        //        DistanceId = 1,
        //        LastName = "Kowalski",
        //        FirstName = "Jan",
        //        SubcategoryId = 1,
        //        IsMale = true,
        //        PhoneNumber = "123 234 345",
        //        Team = "Testowy"
        //    };

        //    CreatedNegotiatedContentResult<PlayerCompetitionRegisterDto> result =
        //        await _playerController.PostRegisterIntoCompetition(1, competitionRegisterDto) as
        //            CreatedNegotiatedContentResult<PlayerCompetitionRegisterDto>;

        //    Assert.AreEqual(result.Content.FirstName, "Jan");

        //}
    }
}

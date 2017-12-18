using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using BaseCore.Dtos;
using BaseCore.Models;

namespace Server.Controllers
{
    public class CompetitionsController : ApiController
    {
        ContextProvider ContextProvider = new ContextProvider();

        // GET api/competition
        public async Task<PageViewModel<CompetitionDto>> Get(PageBindingModel pageBindingModel)
        {
            CompetitionRepository competitionRepository = new CompetitionRepository(ContextProvider);
            PageViewModel<Competition> pageViewModel = await competitionRepository.GetAllAsync(pageBindingModel);
            PageViewModel<CompetitionDto> pageViewModelDto = new PageViewModel<CompetitionDto>
            {
                TotalCount = pageViewModel.TotalCount,
                Items = pageViewModel.Items.Select(i => new CompetitionDto(i)).ToArray()
            };
            return pageViewModelDto;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using Server.Dtos;

namespace ViewCore.HttpClients
{
    public class HttpCompetitionClient : HttpClientBase
    {
        public HttpCompetitionClient(AccountInfo accountInfo, ConnectionInfo connectionInfo, string controllerName)
            : base(accountInfo, connectionInfo, controllerName)
        {
        }

        /// <summary>
        /// Downloads a set of competitions
        /// </summary>
        /// <param name="itemsOnPage">Number of downloaded items</param>
        /// <param name="pageNumber"> Page number starting from 0</param>
        /// <returns></returns>
        public async Task<IEnumerable<CompetitionDto>> GetPageAsync(int itemsOnPage, int pageNumber)
        {
            return await base.GetAsync<IEnumerable<CompetitionDto>>($"{base.GetPageQuery(itemsOnPage, pageNumber)}", addSlash: false);
        }

        public async Task<IEnumerable<CompetitionDto>> GetPageFromPlayerAccountAsync(PlayerAccount playerAccount, int itemsOnPage, int pageNumber)
        {
            return await base.GetAsync<IEnumerable<CompetitionDto>>(
                $"FromPlayerAccount/{playerAccount.Id.ToString()}{base.GetPageQuery(itemsOnPage, pageNumber)}");
        }


        public async Task<CompetitionDto> GetAsync(int id)
        {
            return await base.GetAsync<CompetitionDto>($"/{id.ToString()}");
        }

        public async Task UpdateAsync(Competition content)
        {
            var contentDto = new CompetitionDto(content);
            await base.PutAsync<CompetitionDto>($"/{contentDto.Id.ToString()}", contentDto);
        }

        public async Task<CompetitionDto> CreateAsync(Competition content)
        {
            var contentDto = new CompetitionDto(content);
            return await base.PostAsync<CompetitionDto, CompetitionDto>(string.Empty, contentDto);
        }

        public async Task ChangeCanOrganizerEditAsync(Competition content, bool canOrganizerEdit)
        {
            var contentDto = new CompetitionDto(content);
            await base.PostAsync($"/{contentDto.Id.ToString()}/OrganizerLock{!canOrganizerEdit}");
        }
    }
}

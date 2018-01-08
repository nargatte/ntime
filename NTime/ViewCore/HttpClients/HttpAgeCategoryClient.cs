using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using Server.Dtos;

namespace ViewCore.HttpClients
{
    public class HttpAgeCategoryClient : HttpForSpecificCompetitionClient<AgeCategory, AgeCategoryDto>
    {
        public HttpAgeCategoryClient(AccountInfo accountInfo, ConnectionInfo connectionInfo, string controllerName)
            : base(accountInfo, connectionInfo, controllerName)
        {
        }

        protected override AgeCategoryDto CreateDto(AgeCategory entity)
            => new AgeCategoryDto(entity);
    }
}

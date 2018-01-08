using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using Server.Dtos;

namespace ViewCore.HttpClients
{
    public class HttpExtraPlayerInfoClient : HttpForSpecificCompetitionClient<ExtraPlayerInfo, ExtraPlayerInfoDto>
    {
        public HttpExtraPlayerInfoClient(AccountInfo accountInfo, ConnectionInfo connectionInfo, string controllerName)
            : base(accountInfo, connectionInfo, controllerName)
        {
        }

        protected override ExtraPlayerInfoDto CreateDto(ExtraPlayerInfo entity)
            => new ExtraPlayerInfoDto(entity);
    }
}

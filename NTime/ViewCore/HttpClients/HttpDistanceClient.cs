using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using Server.Dtos;

namespace ViewCore.HttpClients
{
    public class HttpDistanceClient : HttpForSpecificCompetitionClient<Distance, DistanceDto>
    {
        protected HttpDistanceClient(AccountInfo accountInfo, ConnectionInfo connectionInfo, string controllerName)
            : base(accountInfo, connectionInfo, controllerName)
        {
        }

        protected override DistanceDto CreateDto(Distance entity) =>
            new DistanceDto(entity);
    }
}

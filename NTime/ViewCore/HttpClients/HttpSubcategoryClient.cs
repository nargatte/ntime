using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using Server.Dtos;

namespace ViewCore.HttpClients
{
    public class SubcategoryClient : HttpForSpecificCompetitionClient<Subcategory, SubcategoryDto>
    {
        public SubcategoryClient(AccountInfo accountInfo, ConnectionInfo connectionInfo, string controllerName)
            : base(accountInfo, connectionInfo, controllerName)
        {
        }

        protected override SubcategoryDto CreateDto(Subcategory entity)
            => new SubcategoryDto(entity);
    }
}

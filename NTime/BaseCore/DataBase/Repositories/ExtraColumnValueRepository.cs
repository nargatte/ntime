using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.DataBase.Repositories
{
    public class ExtraColumnValueRepository : Repository<ExtraColumnValue>
    {
        public ExtraColumnValueRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        protected override IQueryable<ExtraColumnValue> GetIncludeQuery(IQueryable<ExtraColumnValue> items)
        {
            return items.Include(item => item.Column);
        }
    }
}

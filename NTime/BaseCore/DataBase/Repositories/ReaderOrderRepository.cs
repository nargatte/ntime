﻿using System;
using System.Linq;

namespace BaseCore.DataBase
{
    public class ReaderOrderRepository : Repository<ReaderOrder>
    {
        public ReaderOrderRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        protected Distance Distance { get; set; }

        protected override IQueryable<ReaderOrder> GetAllQuery(IQueryable<ReaderOrder> items) =>
            items.Where(i => i.DistanceId == Distance.Id);

        protected override IQueryable<ReaderOrder> GetSortQuery(IQueryable<ReaderOrder> items) =>
            items.OrderBy(i => i.OrderNumber);

        protected override void CheckItem(ReaderOrder item)
        {
            if(item.DistanceId != Distance.Id) 
                throw new ArgumentException("Wrong DistanceId");
        }

        protected override void PrepareToAdd(ReaderOrder item)
        {
            item.DistanceId = Distance.Id;
            item.Distance = null;
        }
    }
}
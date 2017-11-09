using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace BaseCore.DataBase
{
    public class NTimeDBInitializer : DropCreateDatabaseIfModelChanges<NTimeDBContext>
    {
        protected override void Seed(NTimeDBContext context)
        {
            SeedEnumValues<CompetitionType, CompetitionTypeEnum>(context.CompetitionTypes, @enum => @enum);
            context.SaveChanges();
            base.Seed(context);
        }

        private static void SeedEnumValues<T, TEnum>(IDbSet<T> dbSet, Func<TEnum, T> converter)
            where T : class => Enum.GetValues(typeof(TEnum))
            .Cast<object>()
            .Select(value => converter((TEnum)value))
            .ToList()
            .ForEach(instance => dbSet.AddOrUpdate(instance));
    }
}
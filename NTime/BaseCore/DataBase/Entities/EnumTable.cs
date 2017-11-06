using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace BaseCore.DataBase
{
    public abstract class EnumTable<T> : IEntityId
        where T : struct, IConvertible
    {
        protected EnumTable()
        {
        }

        protected EnumTable(T @enum)
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");
            Id = @enum.ToInt32(null);
            Name = @enum.ToString(null);
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }
    }
}
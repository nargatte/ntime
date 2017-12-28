using System.Collections.Generic;
using System.Linq;

namespace BaseCore.Models
{
    public class PageViewModel<T>
        where T: class
    {
        public int TotalCount { get; set; }
        public T[] Items { get; set; }
    }
}
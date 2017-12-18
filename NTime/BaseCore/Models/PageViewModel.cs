using System.Collections.Generic;

namespace BaseCore.Models
{
    public class PageViewModel<T>
        where T: class
    {
        public int TotalCount { get; set; }
        public T[] Items { get; set; }
    }
}
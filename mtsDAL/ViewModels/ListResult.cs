using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtsDAL.ViewModels
{
    //Allows to send response in a list format
    public class ListResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtsDAL.ViewModels
{
    public class PaginationInfo
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public string NextPageText { get; set; } = ">";
        public string PrevPageText { get; set; } = "<";
        public string Text { get => $" <span>Total of <strong>{Total}</strong>, <strong>{PageSize}</strong> per page</span>"; }
    }
}

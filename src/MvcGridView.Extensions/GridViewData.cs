using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcGridView.Code
{
    public class GridViewData<T>
    {
        public PagedList<T> PagedList
        {
            get;
            set;
        }

        public T EditItem
        {
            get;
            set;
        }
    }
}

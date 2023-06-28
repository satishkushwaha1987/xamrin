using System;
using System.Collections.Generic;
using System.Text;

namespace CHBackOffice.ApiServices.Interfaces
{
    public interface IMapperItem<TRepo, TModel>
    {
        TModel ToModelItem(TRepo param);
    }
}

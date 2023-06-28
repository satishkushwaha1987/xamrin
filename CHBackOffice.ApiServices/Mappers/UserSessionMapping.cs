using CHBackOffice.ApiServices.ChsProxy;
using CHBackOffice.ApiServices.Interfaces;
using CHBackOffice.ApiServices.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CHBackOffice.ApiServices.Mappers
{
    public class UserSessionMapper : IMapperItem<UserSession,UserSessionModel>
    {
        public UserSessionModel ToModelItem(UserSession userSession)
        {
            return new UserSessionModel();
        }
    }
}

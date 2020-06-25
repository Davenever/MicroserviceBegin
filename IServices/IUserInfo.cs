using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IUserInfo
    {
        Task<CustomerUser> QueryUserInfoById(int id);
    }
}

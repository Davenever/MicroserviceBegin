using IServices;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserInfo : IUserInfo
    {

        public async Task<CustomerUser> QueryUserInfoById(int id)
        {
            var list = new List<CustomerUser>();
            for (int i = 1; i <= 10; i++)
            {
                list.Add(new CustomerUser
                {
                    id = i,
                    age = i + 15,
                    name = "小强" + i + "号",
                    dt = DateTime.Now
                });
            }
            return list.Where(m => m.id == id).FirstOrDefault();
        }
    }
}

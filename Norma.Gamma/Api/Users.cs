using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Norma.Gamma.Models;

namespace Norma.Gamma.Api
{
    public class Users : AbemaTvApiImpl
    {
        public Users(AbemaTv abemaTv) : base(abemaTv)
        {

        }

        public async Task<User> Verify(params Expression<Func<string, object>>[] parameters)
            => await AbemaTv.PostAsync<User>(EndPoints.Users, parameters);

        public async Task<User> Show(string user)
            => await AbemaTv.PostAsync<User>(string.Format(EndPoints.UsersShow, user), null);
    }
}
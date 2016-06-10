using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Norma.Gamma.Extensions;
using Norma.Gamma.Models;

namespace Norma.Gamma.Api
{
    public class Users : AbemaTvApiImpl
    {
        public Users(AbemaTv abemaTv) : base(abemaTv)
        {

        }

        public async Task<User> VerifyAsync(params Expression<Func<string, object>>[] parameters)
            => await AbemaTv.PostAsync<User>(EndPoints.Users, parameters).Stay();

        public User Verify(params Expression<Func<string, object>>[] parameters)
            => AbemaTv.Post<User>(EndPoints.Users, parameters);

        public async Task<User> ShowAsync(string user)
            => await AbemaTv.PostAsync<User>(string.Format(EndPoints.UsersShow, user), null).Stay();

        public User Show(string user)
            => AbemaTv.Post<User>(string.Format(EndPoints.UsersShow, user), null);
    }
}
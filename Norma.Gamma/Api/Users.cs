using System.Threading.Tasks;

using Norma.Gamma.Models;

namespace Norma.Gamma.Api
{
    internal class Users : AbemaTvApiImpl
    {
        public Users(AbemaTv abemaTv) : base(abemaTv)
        {

        }

        public async Task<User> Verify(string device, string appKeySecret)
        {
            return await AbemaTv
                .PostAsync<User>(EndPoints.Users, deviceId => device, applicationKeySecret => appKeySecret);
        }

        public async Task<User> Show(string user)
        {
            return await AbemaTv.PostAsync<User>(string.Format(EndPoints.UsersShow, user), null);
        }
    }
}
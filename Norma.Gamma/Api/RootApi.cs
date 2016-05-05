using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Norma.Gamma.Models;

namespace Norma.Gamma.Api
{
    internal class RootApi : AbemaTvApiImpl
    {
        public RootApi(AbemaTv abemaTv) : base(abemaTv)
        {

        }

        public async Task<ApiToken> Token(params Expression<Func<string, object>>[] parameters)
            => await AbemaTv.GetAsync<ApiToken>(EndPoints.Token, parameters);

        public async Task<Media> Media(params Expression<Func<string, object>>[] parameters)
            => await AbemaTv.GetAsync<Media>(EndPoints.Media, parameters);

        // Mime

        // Feed

        public async Task<SlotAudience> SlotAudience(params Expression<Func<string, object>>[] parameters)
            => await AbemaTv.GetAsync<SlotAudience>(EndPoints.SlotAudicence, parameters);

        // Move to Comment class?
        public async Task<Comments> Comments(string slotId, params Expression<Func<string, object>>[] parameters)
            => await AbemaTv.GetAsync<Comments>(string.Format(EndPoints.Comments, slotId), parameters);
    }
}
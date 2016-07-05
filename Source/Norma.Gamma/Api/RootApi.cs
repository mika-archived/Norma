using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Norma.Gamma.Extensions;
using Norma.Gamma.Models;

namespace Norma.Gamma.Api
{
    public class RootApi : AbemaTvApiImpl
    {
        public RootApi(AbemaTv abemaTv) : base(abemaTv)
        {

        }

        public async Task<ApiToken> TokenAsync(params Expression<Func<string, object>>[] parameters)
            => await AbemaTv.GetAsync<ApiToken>(EndPoints.Token, parameters).Stay();

        public ApiToken Token(params Expression<Func<string, object>>[] parameters)
            => AbemaTv.Get<ApiToken>(EndPoints.Token, parameters);

        public async Task<Media> MediaAsync(params Expression<Func<string, object>>[] parameters)
            => await AbemaTv.GetAsync<Media>(EndPoints.Media, parameters).Stay();

        public Media Media(params Expression<Func<string, object>>[] parameters)
            => AbemaTv.Get<Media>(EndPoints.Media, parameters);

        // Mime

        // Feed

        public async Task<Slot> SlotDetailAsync(string slotId, params Expression<Func<string, object>>[] parameters)
            => (await AbemaTv.GetAsync<SlotWrapper>(string.Format(EndPoints.SlotDetail, slotId), parameters))?.Slot;

        public Slot SlotDetail(string slotId, params Expression<Func<string, object>>[] parameters)
            => AbemaTv.Get<SlotWrapper>(string.Format(EndPoints.SlotDetail, slotId), parameters)?.Slot;

        public async Task<SlotAudience> SlotAudienceAsync(params Expression<Func<string, object>>[] parameters)
            => await AbemaTv.GetAsync<SlotAudience>(EndPoints.SlotAudicence, parameters).Stay();

        public SlotAudience SlotAudience(params Expression<Func<string, object>>[] parameters)
            => AbemaTv.Get<SlotAudience>(EndPoints.SlotAudicence, parameters);

        // Move to Comment class?
        public async Task<Comments> CommentsAsync(string slotId, params Expression<Func<string, object>>[] parameters)
            => await AbemaTv.GetAsync<Comments>(string.Format(EndPoints.Comments, slotId), parameters).Stay();

        public Comments Comments(string slotId, params Expression<Func<string, object>>[] parameters)
            => AbemaTv.Get<Comments>(string.Format(EndPoints.Comments, slotId), parameters);

        public async Task<Comment> CommentAsync(string slotId, params Expression<Func<string, object>>[] parameters)
            => await AbemaTv.PostAsync<Comment>(string.Format(EndPoints.Comments, slotId), parameters).Stay();

        public Comment Comment(string slotId, params Expression<Func<string, object>>[] parameters)
            => AbemaTv.Post<Comment>(string.Format(EndPoints.Comments, slotId), parameters);

        public class SlotWrapper
        {
            [JsonProperty("slot")]
            public Slot Slot { get; set; }
        }
    }
}
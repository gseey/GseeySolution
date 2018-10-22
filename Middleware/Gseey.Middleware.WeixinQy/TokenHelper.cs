using Senparc.Weixin.Work.Containers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.WeixinQy
{
    public class TokenHelper
    {
        /// <summary>
        /// 异步获取accesstoken
        /// </summary>
        /// <param name="corpId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static async Task<string> GetAccessTokenAsync(string corpId, int appId)
        {
            var configDto = await ChannelHelper.GetAgentConfigDTOAsync(appId);
            if (configDto != null && configDto.Id > 0)
            {
                var tokenResult = await AccessTokenContainer.TryGetTokenAsync(configDto.CorpId, configDto.CorpSercet);
                return tokenResult;
            }

            return string.Empty;
        }
    }
}

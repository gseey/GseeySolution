using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.Weixin.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.Weixin.Media
{
    public class MediaApi
    {
        public static async Task<object> UploadAsync(int channelId, UploadMediaFileType fileType, string filePath)
        {
            var configDto = WeixinConfigHelper.GetWeixinConfigDTO(channelId);

            var weixinUploadUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", configDto.AccessToken, fileType.ToString());

            var result = await HttpHelper.UploadFile<object>(weixinUploadUrl, filePath);
            return result;
        }
    }
}

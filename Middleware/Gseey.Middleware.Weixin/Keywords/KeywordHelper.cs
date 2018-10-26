using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.Weixin.Keywords
{
    /// <summary>
    /// 关键字回复帮助类
    /// </summary>
    public class KeywordHelper
    {
        /// <summary>
        /// 获取自定义关键词回复
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="channelId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static async Task<TResult> GetCustomKeywordsReply<TResult>(int channelId, string keyword) where TResult : class
        {
            //从缓存中读取当前关键词回复的语境

            //根据当前语境及关键词,匹配对应的回复
            return null;
        }
    }
}

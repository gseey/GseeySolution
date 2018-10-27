using Gseey.Framework.BaseDTO;
using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.Weixin.Enums;
using Gseey.Middleware.Weixin.Helpers;
using Gseey.Middleware.Weixin.Keywords.DBContext;
using Gseey.Middleware.Weixin.Message.Entities.Response;
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
        /// <param name="fromUser"></param>
        /// <param name="toUser"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static async Task<ExecuteResult<string>> GetCustomKeywordsReplyAsync<TResult>(int channelId, string fromUser, string toUser, string keyword)
        {
            var result = new ExecuteResult<string>
            {
                Success = false,
                ErrorCode = ExecuteResult.ErrorCodeEnum.Fail,
                ErrorMsg = "获取自定义关键词回复失败"
            };

            //从缓存中读取当前关键词回复的语境
            var redisKey = string.Format("KeywordContextCache_{0}", channelId);
            var cackeKeywordContextId = await RedisHelper.StringGetAsync<int>(redisKey);

            //根据当前语境及关键词,匹配对应的回复
            var keywordReplyEntity = await KeywordDBContext.GetKeywordReplyAsync(channelId, keyword, cackeKeywordContextId);
            if (keywordReplyEntity.KeywordContextId > 0)
            {
                var replyMsg = string.Empty;
                switch (keywordReplyEntity.RelpyType)
                {
                    case KeywordRelpyTypeEnum.Text:
                    default:
                        var content = keywordReplyEntity.ReplyMsg;
                        replyMsg = ReplyText(toUser, fromUser, content);
                        break;
                    case KeywordRelpyTypeEnum.Image:
                        var mediaId = keywordReplyEntity.ReplyMsg;
                        replyMsg = ReplyImage(toUser, fromUser, mediaId);
                        break;
                    case KeywordRelpyTypeEnum.News:
                        var news = keywordReplyEntity.ReplyMsg.FromJson<List<ResponseCommonArticlesDTO>>();
                        replyMsg = ReplyNews(toUser, fromUser, news);
                        break;
                }

                var encryptMsg = SignHelper.EncryptMsg(channelId, replyMsg);
                result.Data = encryptMsg;

                result.Success = true;
                result.ErrorMsg = "获取自定义关键词回复成功";
                result.ErrorCode = ExecuteResult.ErrorCodeEnum.Success;
            }
            return result;
        }

        #region 回复被动消息

        /// <summary>
        /// 回复文本消息
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string ReplyText(string toUserName, string fromUserName, string content)
        {
            return string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName>" +
                                                   "<FromUserName><![CDATA[{1}]]></FromUserName>" +
                                                   "<CreateTime>{2}</CreateTime>" +
                                                   "<MsgType><![CDATA[text]]></MsgType>" +
                                                   "<Content><![CDATA[{3}]]></Content></xml>",
                                                   toUserName, fromUserName, DateTime.Now.ToUnixTime(), content);
        }

        /// <summary>
        /// 回复多图文消息
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="newsList"></param>
        /// <returns></returns>
        private static string ReplyNews(string toUserName, string fromUserName, List<ResponseCommonArticlesDTO> newsList)
        {
            var builder = new StringBuilder();
            builder.AppendFormat(@"<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>{3}</ArticleCount><Articles>",
             toUserName, fromUserName,
             DateTime.Now.ToUnixTime(),
             newsList.Count
                );
            foreach (var newsItem in newsList)
            {
                builder.AppendFormat(@"<item><Title><![CDATA[{0}]]></Title><Description><![CDATA[{1}]]></Description><PicUrl><![CDATA[{2}]]></PicUrl><Url><![CDATA[{3}]]></Url></item>",
                   newsItem.Title,
                   newsItem.Description,
                   newsItem.PicUrl,
                   newsItem.Url
                 );
            }
            builder.Append("</Articles></xml>");
            return builder.ToString();
        }

        /// <summary>
        /// 回复图片消息
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="media_id">已经上传到微信服务器的图片media_id</param>
        /// <returns></returns>
        private static string ReplyImage(string toUserName, string fromUserName, string media_id)
        {
            return string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName>" +
                                                   "<FromUserName><![CDATA[{1}]]></FromUserName>" +
                                                   "<CreateTime>{2}</CreateTime>" +
                                                   "<MsgType><![CDATA[image]]></MsgType>" +
                                                   "<Image><MediaId><![CDATA[{3}]]></MediaId></Image></xml>",
                                                   toUserName,
                                                   fromUserName,
                                                   DateTime.Now.ToUnixTime(),
                                                   media_id);
        } 

        #endregion
    }
}

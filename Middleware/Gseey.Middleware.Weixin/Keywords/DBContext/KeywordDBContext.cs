using Gseey.Framework.Common.Helpers;
using Gseey.Framework.DataBase;
using Gseey.Framework.DataBase.DalBase;
using Gseey.Middleware.Weixin.Keywords.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gseey.Middleware.Weixin.Keywords.DBContext
{
    public class KeywordDBContext
    {
        private static DapperDALBase<ChannelKeywordRelationEntity> ChannelKeywrodRelationDAL { get; }
        private static DapperDALBase<KeywordReplyDetailEntity> KeywrodDAL { get; }


        /// <summary>
        /// 获取渠道关键词回复内容
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="keyword">关键词</param>
        /// <param name="preKeywordContextId">上一关键词id</param>
        /// <returns></returns>
        public static async Task<KeywordReplyDetailEntity> GetKeywordReplyAsync(int channelId, string keyword, int preKeywordContextId = -1)
        {
            try
            {
                var redisKey = string.Format("{0}_{1}_{2}", channelId, keyword, preKeywordContextId);
                var replyDetailCache = await RedisHelper.StringGetAsync<KeywordReplyDetailEntity>(redisKey);
                if (replyDetailCache == null || replyDetailCache.KeywordContextId <= 0)
                {
                    var sql = @"SELECT a.* FROM KeywordReplyDetail a
                                        JOIN ChannelKeywordRelation b ON a.KeywordContextId=b.KeywordContextId
                                        WHERE b.ChannelId=@ChannelId
                                        ORDER BY a.LastModifyTime DESC";
                    var keywordEntities = await DapperDBHelper.QueryAsync<KeywordReplyDetailEntity>(sql, new { ChannelId = channelId });
                    if (keywordEntities.Count() > 0)
                    {
                        replyDetailCache = keywordEntities.SingleOrDefault();
                        var setResult = await RedisHelper.StringSetAsync(redisKey, replyDetailCache, TimeSpan.FromHours(1.0));
                    }
                }
                return replyDetailCache;
            }
            catch (Exception ex)
            {
                ex.WriteExceptionLog("获取渠道关键词回复内容出错");
            }
            return new KeywordReplyDetailEntity();
        }
    }
}

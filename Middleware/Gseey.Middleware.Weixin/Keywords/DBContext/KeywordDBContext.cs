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
                return new KeywordReplyDetailEntity
                {
                    KeywordContextId = 1,
                    ReplyMsg = "test reply",
                };

                var sql = "";
                var keywordEntities = await DapperDBHelper.QueryAsync<KeywordReplyDetailEntity>(sql, new { a = 1 });
                var channelRelationEntities = await ChannelKeywrodRelationDAL.QueryListAsync(new { ChannelId = channelId });
                if (keywordEntities.Count() > 0)
                {
                    return keywordEntities.SingleOrDefault();
                }
            }
            catch (Exception)
            {

            }
            return new KeywordReplyDetailEntity();
        }
    }
}

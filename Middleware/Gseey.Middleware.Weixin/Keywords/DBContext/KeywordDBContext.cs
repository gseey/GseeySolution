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

        public static async Task<KeywordReplyDetailEntity> Test(int channelId)
        {
            var sql = "";
            var keywordEntities = await DapperDBHelper.QueryAsync<KeywordReplyDetailEntity>(sql, new { a = 1 });
            var channelRelationEntities = await ChannelKeywrodRelationDAL.QueryListAsync(new { ChannelId = channelId });
            if (keywordEntities.Count() > 0)
            {
                return keywordEntities.SingleOrDefault();
            }
            return null;
        }
    }
}

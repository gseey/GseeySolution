using Gseey.Framework.DataBase.Attributes;
using Gseey.Framework.DataBase.EntityBase;
using Gseey.Middleware.Weixin.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Keywords.Entities
{
    [Serializable]
    [Table(Name = "ChannelKeywordRelation")]
    public class ChannelKeywordRelationEntity : DapperEntityBase
    {
        /// <summary>
        /// 渠道id
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// 关键词语境id
        /// </summary>
        public int KeywordContextId { get; set; }

        /// <summary>
        /// 关键词状态
        /// </summary>
        public ChannelKeywordRelationStatusEnum ChannelKeywordRelationStatus { get; set; }
    }
}

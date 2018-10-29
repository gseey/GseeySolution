using Gseey.Framework.DataBase.Attributes;
using Gseey.Framework.DataBase.EntityBase;
using Gseey.Middleware.Weixin.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Keywords.Entities
{
    [Serializable]
    [Table(Name = "KeywordReplyDetail")]
    public class KeywordReplyDetailEntity : DapperEntityBase
    {
        /// <summary>
        /// 关键词语境id
        /// </summary>
        public int KeywordContextId { get; set; }

        /// <summary>
        /// 上一语境关键词id
        /// </summary>
        public int PreKeywordContextId { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifyTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public int ModifyUserId { get; set; }

        /// <summary>
        /// 回复类型
        /// </summary>
        public KeywordRelpyTypeEnum RelpyType { get; set; }

        /// <summary>
        /// 回复消息内容
        /// </summary>
        public string ReplyMsg { get; set; }

        /// <summary>
        /// 关键词状态
        /// </summary>
        public KeywordRelpyStatusEnum KeywordStatus { get; set; }
    }
}

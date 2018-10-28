using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Enums
{
    public enum ResponseWorkMsgTypeEnum
    {
        /// <summary>
        /// 文本回复
        /// </summary>
        Text = 10,

        /// <summary>
        /// 图片回复
        /// </summary>
        Image = 20,

        /// <summary>
        /// 语音回复
        /// </summary>
        Voice = 30,

        /// <summary>
        /// 视频回复
        /// </summary>
        Video = 40,

        /// <summary>
        /// 文件回复
        /// </summary>
        File = 50,

        /// <summary>
        /// 文本卡片回复
        /// </summary>
        Textcard = 60,

        /// <summary>
        /// 图文回复
        /// </summary>
        Mpnews = 70,
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Media
{

    /// <summary>
    /// 上传媒体文件类型（所有文件size必须大于5个字节）
    /// </summary>
    public enum UploadMediaFileType
    {
        /// <summary>
        /// 图片: 2MB，支持JPG,PNG格式
        /// </summary>
        image,
        /// <summary>
        /// 语音：2MB，播放长度不超过60s，支持AMR格式
        /// </summary>
        voice,
        /// <summary>
        /// 视频：10MB，支持MP4格式
        /// </summary>
        video,
        /// <summary>
        /// 普通文件：20MB
        /// </summary>
        file
    }
}

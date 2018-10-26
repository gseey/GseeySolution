using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.BaseDTOs
{
    public class ResponseBaseDTO
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }
    }
}

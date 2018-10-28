using Gseey.Middleware.Weixin.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Contact.DTOs.Member
{
    public class GetDepartmentMemberDetailListResponseDTO : ResponseBaseDTO
    {
        /// <summary>
        /// 成员列表
        /// </summary>
        public List<MemberDetailListResponseDTO> userlist { get; set; }
    }

    public class MemberDetailListResponseDTO : ResponseBaseDTO
    {
        /// <summary>
        /// 成员UserID。对应管理端的帐号
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 成员名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 手机号码，第三方仅通讯录套件可获取
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 成员所属部门id列表
        /// </summary>
        public int[] department { get; set; }

        /// <summary>
        /// 部门内的排序值，默认为0。数量必须和department一致，数值越大排序越前面。值范围是[0, 2^32)
        /// </summary>
        public int[] order { get; set; }

        /// <summary>
        /// 职务信息；第三方仅通讯录应用可获取
        /// </summary>
        public string position { get; set; }

        /// <summary>
        /// 性别。0表示未定义，1表示男性，2表示女性
        /// </summary>
        public string gender { get; set; }

        /// <summary>
        /// 邮箱，第三方仅通讯录应用可获取
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 标示是否为上级；第三方仅通讯录应用可获取
        /// </summary>
        public int isleader { get; set; }

        /// <summary>
        /// 头像url。注：如果要获取小图将url最后的”/0”改成”/100”即可。第三方仅通讯录应用可获取
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 座机。第三方仅通讯录应用可获取
        /// </summary>
        public string telephone { get; set; }

        /// <summary>
        /// 成员启用状态。1表示启用的成员，0表示被禁用。服务商调用接口不会返回此字段
        /// </summary>
        public int enable { get; set; }

        /// <summary>
        /// 别名；第三方仅通讯录应用可获取
        /// </summary>
        public string alias { get; set; }

        /// <summary>
        /// 激活状态: 1=已激活，2=已禁用，4=未激活 已激活代表已激活企业微信或已关注微工作台（原企业号）。未激活代表既未激活企业微信又未关注微工作台（原企业号）。
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 扩展属性，第三方仅通讯录套件可获取
        /// </summary>
        public Extattr extattr { get; set; }

        /// <summary>
        /// 员工个人二维码，扫描可添加为外部联系人；第三方仅通讯录应用可获取
        /// </summary>
        public string qr_code { get; set; }

        /// <summary>
        /// 成员对外属性，字段详情见对外属性；第三方仅通讯录应用可获取
        /// </summary>
        public string external_position { get; set; }

        /// <summary>
        /// 对外职务。 第三方仅通讯录应用可获取
        /// </summary>
        public External_Profile external_profile { get; set; }
    }

    public class Extattr
    {
        public Attr[] attrs { get; set; }
    }

    public class Attr
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class External_Profile
    {
        public External_Attr[] external_attr { get; set; }
    }

    public class External_Attr
    {
        public int type { get; set; }
        public string name { get; set; }
        public Text text { get; set; }
        public Web web { get; set; }
        public Miniprogram miniprogram { get; set; }
    }

    public class Text
    {
        public string value { get; set; }
    }

    public class Web
    {
        public string url { get; set; }
        public string title { get; set; }
    }

    public class Miniprogram
    {
        public string appid { get; set; }
        public string pagepath { get; set; }
        public string title { get; set; }
    }

}

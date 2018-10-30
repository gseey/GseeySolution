using Dapper;
using Gseey.ConsoleTest.AutofacDemo;
using Gseey.Framework.Common.Helpers;
using Gseey.Framework.DataBase;
using Gseey.Framework.DataBase.DalBase;
using Gseey.Framework.DataBase.EntityBase;
using Gseey.Middleware.Weixin.BaseDTOs;
using Gseey.Middleware.Weixin.Contact;
using Gseey.Middleware.Weixin.Enums;
using Gseey.Middleware.Weixin.Media;
using Gseey.Middleware.Weixin.Menu;
using Gseey.Middleware.Weixin.Menu.DTOs;
using Gseey.Middleware.Weixin.Message;
using Gseey.Middleware.Weixin.Message.Entities;
using Gseey.Middleware.Weixin.Message.Entities.Request;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Gseey.ConsoleTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //LogHelper.Warn("fsfsfsfsfsfsd");
            var ex = new Exception("犯得上发生发生");
            ex.WriteExceptionLog("fsfsdf");


            //var result = ContactApi.GetTagMemberAsync(3, 3).Result;

            //var userIdList = new List<string>();
            //result.userlist.ForEach(m =>
            //{
            //    userIdList.Add(m.userid);
            //});

            ////var r1 = ActiveMessageApi.SendWorkAgentContentMsgAsync(1, new RequestWorkTextCardMsgDTO
            ////{
            ////    UserIdList = userIdList,
            ////    TextCard = new RequestWorkTextCardItemMsgDTO
            ////    {
            ////        Btntxt = "加载更多",
            ////        Description = "test水水水水水水水水水水水水水水",
            ////        Title = "发生发生发射点",
            ////        Url = "https://www.cnblogs.com/jajian/p/9853592.html"
            ////    },
            ////    Safe = 1
            ////}).Result;

            //var r2 = ActiveMessageApi.SendWorkAgentMediaMsgAsync(1, new RequestWorkImageMsgDTO
            //{
            //    FilePath = @"E:\1.jpg",
            //    UserIdList = userIdList,
            //    PartyIdList = result.partylist,
            //}).Result;


            Console.ReadKey();
        }
    }

    public class person
    {
        public string name { get; set; }

        public int age { get; set; }

    }


    //[System.Serializable]
    public class Custom : DapperEntityBase
    {
        //[Key]
        public int CustomID { get; set; }

        public int CustomType { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string CompanyName { get; set; }

        public string Proposer { get; set; }

        public string Address { get; set; }

        public string BusinessLicense { get; set; }

        public int PayType { get; set; }

        public string AlipayNumber { get; set; }

        public string AlipayRealName { get; set; }

        public string AlipayPhone { get; set; }

        public int BankType { get; set; }

        public int BankProvince { get; set; }

        public int BankCity { get; set; }

        public int BankCounty { get; set; }

        public string BankSubbranch { get; set; }

        public string BankNumber { get; set; }

        public string BankPhone { get; set; }

        public int CustomState { get; set; }

        public DateTime CreateTime { get; set; }

        public int Balance { get; set; }

        /// <summary>
        /// 自媒体帐号类型（1：个人；2：公司）
        /// </summary>
        public int MediaAcountType { get; set; }

        /// <summary>
        /// 申请人身份证号
        /// </summary>
        public string ProposerIdCardNumber { get; set; }

        /// <summary>
        /// 申请人身份证正面
        /// </summary>
        public string ProposerIdCardFontPic { get; set; }

        /// <summary>
        /// 申请人身份证反面
        /// </summary>
        public string ProposerIdCardBackgroudPic { get; set; }

        /// <summary>
        /// 收益余额
        /// </summary>
        public int Income { get; set; }

        /// <summary>
        /// 客户归属
        /// </summary>
        public string CustomerOwnership { get; set; }

        public int MovieViewGroupAmount { get; set; }

        public string HostPartyName { get; set; }

        public int CustomUserType { get; set; }

        public int CustomRole { get; set; }

        public int CustomGrantUser { get; set; }

        public DateTime CustomGrantTime { get; set; }

        public string LicenseImgUrl { get; set; }

        public int CertificationStatus { get; set; }

        public int CertificationUserID { get; set; }

        public DateTime CertificationTime { get; set; }

        public string CertificationRefusedReason { get; set; }

        public int ManageCustomId { get; set; }

        public string WeixinOpenID { get; set; }

        public int WeixinBaseId { get; set; }

        public string HeadImgUrl { get; set; }
    }

}

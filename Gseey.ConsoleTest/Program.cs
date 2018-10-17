using Dapper;
using Gseey.Framework.Common.Helpers;
using Gseey.Framework.DataBase;
using System;
using System.Text;
using System.Threading;

namespace Gseey.ConsoleTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            LogHelper.RunLog("fsfsfsfd", folderName: "fsfsd");

            var p1 = DBHelper.GetAsync<Custom>(1);
            p1.Wait();
            var pw1 = p1.Result;

            var p2 = DBHelper.GetAsync<Custom>(2);
            p2.Wait();
            var pw2 = p2.Result;

            var l1 = DateTime.Now.ToUnixTime(true);
            var t1 = l1.FromUnixTime(true);

            var s1 = ConfigHelper.Get("GseeyConnections:DbConnectionString");
            var s2 = ConfigHelper.Get("GseeyWeixinConfig:QY:CorpId");
            var s3 = ConfigHelper.Get("GseeyWeixinConfig:QY:CorpSercet");


            //var t1 = EncrtpyHelper.RSAEncrypt("", "test");
            //var t2 = EncrtpyHelper.RSADecrypt("", t1);


            var h1 = HttpHelper.GetHtml("http://esf.fang.com/chushou/3_417109133.htm?channel=2,2&psid=1_1_70");
            var h2 = HttpHelper.GetHtmlAsync("https://www.cnblogs.com/sunxucool/p/4180375.html").Result;

            //var e1 = ConfigHelper.Get("test:t0");

            RedisHelper redisHelper = new RedisHelper(0);
            var redisKey = "test";
            var r1 = redisHelper.StringSet(redisKey, "2131232");
            var r2 = redisHelper.StringGet(redisKey);

            Console.WriteLine("Hello World!");

            Console.ReadKey();
        }
    }

    public class person
    {
        public string name { get; set; }

        public int age { get; set; }

    }


    [Serializable]
    public class Custom
    {
        [Key]
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

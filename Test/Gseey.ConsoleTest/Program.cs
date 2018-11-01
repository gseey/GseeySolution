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


            //var dbType = ConfigHelper.Get("Gseey:Connections:DbConnectionType");




            var result = ContactApi.GetTagMemberAsync(3, 3).Result;

            var userIdList = new List<string>();
            result.userlist.ForEach(m =>
            {
                userIdList.Add(m.userid);
            });

            var r1 = ActiveMessageApi.SendWorkAgentContentMsgAsync(1, new RequestWorkTextCardMsgDTO
            {
                UserIdList = userIdList,
                TextCard = new RequestWorkTextCardItemMsgDTO
                {
                    Btntxt = "加载更多",
                    Description = "test水水水水水水水水水水水水水水",
                    Title = "发生发生发射点",
                    Url = "https://www.cnblogs.com/jajian/p/9853592.html"
                },
            }).Result;

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

}

﻿namespace Gseey.ConsoleTest
{
    using Gseey.Middleware.Weixin.Contact;
    using Gseey.Middleware.Weixin.Message;
    using Gseey.Middleware.Weixin.Message.Entities.Request;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="Program" />
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The Main
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/></param>
        internal static void Main(string[] args)
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

    /// <summary>
    /// Defines the <see cref="person" />
    /// </summary>
    public class person
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the age
        /// </summary>
        public int age { get; set; }
    }
}

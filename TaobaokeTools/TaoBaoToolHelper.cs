using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace TaobaokeTools
{
    public class TaoBaoToolHelper
    {
        private static string AppKey
        {
            get
            {
                return "27606005";
            }
        }

        private static string AppSecret
        {
            get
            {
                return "451c44976dd5fa90760f933dfe95a016";
            }
        }

        private static string TaobaoUrl
        {
            get
            {
                return "https://eco.taobao.com/router/rest";
            }
        }

        private static long adzone_id
        {
            get
            {
                return 113418056;
            }
        }

        private static string UserId { get { return "124360172"; } }

        private static ITopClient GetTaoClientSinglone()
        {
            var topClient = new DefaultTopClient(TaobaoUrl, AppKey, AppSecret);
            return topClient;
        }

        public static string GetCouponList(string cat = "", string keyword = "", int pageIndex = 1, int pageSize = 20)
        {
            var topClient = GetTaoClientSinglone();
            TbkDgItemCouponGetRequest req = new TbkDgItemCouponGetRequest();
            req.AdzoneId = adzone_id;
            req.Platform = 1L;
            req.Cat = cat;
            req.PageSize = pageSize;
            req.Q = keyword;
            req.PageNo = pageIndex;
            TbkDgItemCouponGetResponse rsp = topClient.Execute(req);


            rsp.Results.ForEach(dgItem =>
            {
                var result = GetTaoCode(dgItem.Title, dgItem.CouponClickUrl, dgItem.PictUrl);
            });

            return rsp.Body;
        }

        public static string GetTaoCode(string title, string couponUrl, string logoUrl)
        {
            var topClient = GetTaoClientSinglone();
            TbkTpwdCreateRequest req = new TbkTpwdCreateRequest();
            req.UserId = UserId;
            req.Text = title;
            req.Url = couponUrl;
            req.Logo = logoUrl;
            req.Ext = "{}";
            TbkTpwdCreateResponse rsp = topClient.Execute(req);
            return rsp.Body;
        }

        
    }
}

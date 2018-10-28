using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.Weixin.BaseDTOs;
using Gseey.Middleware.Weixin.Contact.DTOs.Department;
using Gseey.Middleware.Weixin.Contact.DTOs.Member;
using Gseey.Middleware.Weixin.Contact.DTOs.Tag;
using Gseey.Middleware.Weixin.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.Weixin.Contact
{
    public class ContactApi
    {
        #region 私有方法

        /// <summary>
        /// 验证渠道信息
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="configDto"></param>
        /// <returns></returns>
        public static bool ValidateChannel(int channelId, out WeixinConfigDTO configDto)
        {
            configDto = WeixinConfigHelper.GetWeixinConfigDTO(channelId);
            var result = configDto.WxType == Enums.WeixinType.WxWork;
            return result;
        }

        #endregion

        #region 部门管理

        /// <summary>
        /// 创建部门
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="name">部门名称。长度限制为1~32个字符，字符不能包括\:?”<>｜</param>
        /// <param name="parentId">父部门id，32位整型</param>
        /// <param name="order">在父部门中的次序值。order值大的排序靠前。有效的值范围是[0, 2^32)</param>
        /// <param name="id">部门id，32位整型，指定时必须大于1。若不填该参数，将自动生成id</param>
        /// <returns></returns>
        public static async Task<CreateDepartmentResponseDTO> CreateDepartmentAsync(int channelId, string name, long parentId, int order = 1, long? id = null)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var createDepartmentUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/department/create?access_token={0}", configDto.AccessToken);

                var postData = new { name, parentid = parentId, order, id };
                var result = await HttpHelper.PostDataAsync<CreateDepartmentResponseDTO, object>(createDepartmentUrl, postData);
                return result;
            }
            else
            {
                return new CreateDepartmentResponseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行部门管理"
                };
            }
        }

        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="name">部门名称。长度限制为1~32个字符，字符不能包括\:?”<>｜</param>
        /// <param name="parentId">父部门id，32位整型</param>
        /// <param name="order">在父部门中的次序值。order值大的排序靠前。有效的值范围是[0, 2^32)</param>
        /// <param name="id">部门id，32位整型，指定时必须大于1。若不填该参数，将自动生成id</param>
        /// <returns></returns>
        public static async Task<ResponseBaseDTO> UpdateDepartmentAsync(int channelId, string name, long parentId, int order = 1, long? id = null)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var updateDepartmentUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/department/update?access_token={0}", configDto.AccessToken);

                var postData = new { name, parentid = parentId, order, id };
                var result = await HttpHelper.PostDataAsync<CreateDepartmentResponseDTO, object>(updateDepartmentUrl, postData);
                return result;
            }
            else
            {
                return new ResponseBaseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行部门管理"
                };
            }
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="id">部门id，32位整型，指定时必须大于1</param>
        /// <returns></returns>
        public static async Task<ResponseBaseDTO> DeleteDepartmentAsync(int channelId, int id)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var deleteDepartmentUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/department/delete?access_token={0}&id={1}", configDto.AccessToken, id);

                var result = await HttpHelper.GetHtmlAsync<CreateDepartmentResponseDTO>(deleteDepartmentUrl);
                return result;
            }
            else
            {
                return new ResponseBaseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行部门管理"
                };
            }
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="id">部门id，32位整型，指定时必须大于1</param>
        /// <returns></returns>
        public static async Task<DepartmentListResponseDTO> GetDepartmentListAsync(int channelId, int id)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var getDepartmentListUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/department/list?access_token={0}&id={1}", configDto.AccessToken, id);

                var result = await HttpHelper.GetHtmlAsync<DepartmentListResponseDTO>(getDepartmentListUrl);
                return result;
            }
            else
            {
                return new DepartmentListResponseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行部门管理"
                };
            }
        }

        #endregion

        #region 标签管理

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="tagName">标签名称，长度限制为32个字（汉字或英文字母），标签不可与其他标签重名。</param>
        /// <param name="tagId">标签ID</param>
        /// <returns></returns>
        public static async Task<AddOrDeleteTagMemberResponseDTO> CreateTagAsync(int channelId, string tagName, int tagId)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var createTagUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/tag/create?access_token={0}", configDto.AccessToken);

                var postData = new { tagname = tagName, tagid = tagId };
                var result = await HttpHelper.PostDataAsync<AddOrDeleteTagMemberResponseDTO, object>(createTagUrl, postData);
                return result;
            }
            else
            {
                return new AddOrDeleteTagMemberResponseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行标签管理"
                };
            }
        }

        /// <summary>
        /// 更新标签名字
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="tagName">标签名称，长度限制为32个字（汉字或英文字母），标签不可与其他标签重名。</param>
        /// <param name="tagId">标签ID</param>
        /// <returns></returns>
        public static async Task<ResponseBaseDTO> UpdateTagNameAsync(int channelId, string tagName, int tagId)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var updateTagNameUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/tag/update?access_token={0}", configDto.AccessToken);

                var postData = new { tagname = tagName, tagid = tagId };
                var result = await HttpHelper.PostDataAsync<ResponseBaseDTO, object>(updateTagNameUrl, postData);
                return result;
            }
            else
            {
                return new ResponseBaseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行标签管理"
                };
            }
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="tagId">标签ID</param>
        /// <returns></returns>
        public static async Task<ResponseBaseDTO> DeleteTagAsync(int channelId, int tagId)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var deleteTagUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/tag/delete?access_token={0}&tagid={1}", configDto.AccessToken, tagId);

                var result = await HttpHelper.GetHtmlAsync<ResponseBaseDTO>(deleteTagUrl);
                return result;
            }
            else
            {
                return new ResponseBaseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行标签管理"
                };
            }
        }

        /// <summary>
        /// 获取标签成员
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="tagId">标签ID</param>
        /// <returns></returns>
        public static async Task<GetTagMemberResponseDTO> GetTagMemberAsync(int channelId, int tagId)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var deleteTagUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/tag/get?access_token={0}&tagid={1}", configDto.AccessToken, tagId);

                var result = await HttpHelper.GetHtmlAsync<GetTagMemberResponseDTO>(deleteTagUrl);
                return result;
            }
            else
            {
                return new GetTagMemberResponseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行标签管理"
                };
            }
        }

        /// <summary>
        /// 增加标签成员
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="tagId">标签ID</param>
        /// <param name="userList">企业成员ID列表，注意：userlist、partylist不能同时为空，单次请求长度不超过1000</param>
        /// <param name="partyList">企业部门ID列表，注意：userlist、partylist不能同时为空，单次请求长度不超过100</param>
        /// <returns></returns>
        public static async Task<AddOrDeleteTagMemberResponseDTO> AddTagMemberAsync(int channelId, int tagId, List<string> userList = null, List<string> partyList = null)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var addTagUsersUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/tag/addtagusers?access_token={0}", configDto.AccessToken);

                var postData = new { tagid = tagId, userlist = string.Join("|", userList.ToArray()), partylist = string.Join("|", partyList.ToArray()) };
                var result = await HttpHelper.PostDataAsync<AddOrDeleteTagMemberResponseDTO, object>(addTagUsersUrl, postData);
                return result;
            }
            else
            {
                return new AddOrDeleteTagMemberResponseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行标签管理"
                };
            }
        }

        /// <summary>
        /// 删除标签成员
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="tagId">标签ID</param>
        /// <param name="userList">企业成员ID列表，注意：userlist、partylist不能同时为空，单次请求长度不超过1000</param>
        /// <param name="partyList">企业部门ID列表，注意：userlist、partylist不能同时为空，单次请求长度不超过100</param>
        /// <returns></returns>
        public static async Task<AddOrDeleteTagMemberResponseDTO> DeleteTagMemberAsync(int channelId, int tagId, List<string> userList = null, List<string> partyList = null)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var delTagUsersUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/tag/deltagusers?access_token={0}", configDto.AccessToken);

                var postData = new { tagid = tagId, userlist = string.Join("|", userList.ToArray()), partylist = string.Join("|", partyList.ToArray()) };
                var result = await HttpHelper.PostDataAsync<AddOrDeleteTagMemberResponseDTO, object>(delTagUsersUrl, postData);
                return result;
            }
            else
            {
                return new AddOrDeleteTagMemberResponseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行标签管理"
                };
            }
        }


        /// <summary>
        /// 获取标签成员
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="tagId">标签ID</param>
        /// <returns></returns>
        public static async Task<GetTagListResponseDTO> GetTagListAsync(int channelId)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var tagListUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/tag/list?access_token={0}", configDto.AccessToken);

                var result = await HttpHelper.GetHtmlAsync<GetTagListResponseDTO>(tagListUrl);
                return result;
            }
            else
            {
                return new GetTagListResponseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行标签管理"
                };
            }
        }
        #endregion

        #region 成员管理

        /// <summary>
        /// 创建成员
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="memberDetail">成员详情</param>
        /// <returns></returns>
        public static async Task<ResponseBaseDTO> CreateMemberAsync(int channelId, MemberDetailListResponseDTO memberDetail)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var createMemberUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/create?access_token={0}", configDto.AccessToken);

                var result = await HttpHelper.PostDataAsync<ResponseBaseDTO, MemberDetailListResponseDTO>(createMemberUrl, memberDetail);
                return result;
            }
            else
            {
                return new ResponseBaseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行用户管理"
                };
            }
        }

        /// <summary>
        /// 读取成员
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="userid">成员id</param>
        /// <returns></returns>
        public static async Task<MemberDetailListResponseDTO> GetMemberAsync(int channelId, string userid)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var getMemberUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={0}&userid={1}", configDto.AccessToken, userid);

                var result = await HttpHelper.GetHtmlAsync<MemberDetailListResponseDTO>(getMemberUrl);
                return result;
            }
            else
            {
                return new MemberDetailListResponseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行用户管理"
                };
            }
        }

        /// <summary>
        /// 更新成员
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="memberDetail">成员详情</param>
        /// <returns></returns>
        public static async Task<ResponseBaseDTO> UpdateMemberAsync(int channelId, MemberDetailListResponseDTO memberDetail)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var updateMemberUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/create?access_token={0}", configDto.AccessToken);

                var result = await HttpHelper.PostDataAsync<ResponseBaseDTO, MemberDetailListResponseDTO>(updateMemberUrl, memberDetail);
                return result;
            }
            else
            {
                return new ResponseBaseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行用户管理"
                };
            }
        }

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="userid">成员id</param>
        /// <returns></returns>
        public static async Task<ResponseBaseDTO> DelteMemberAsync(int channelId, string userid)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var deleteMemberUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/delete?access_token={0}&userid={1}", configDto.AccessToken, userid);

                var result = await HttpHelper.GetHtmlAsync<ResponseBaseDTO>(deleteMemberUrl);
                return result;
            }
            else
            {
                return new ResponseBaseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行用户管理"
                };
            }
        }

        /// <summary>
        /// 批量删除成员
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="userid">成员id</param>
        /// <returns></returns>
        public static async Task<ResponseBaseDTO> BatchDelteMemberAsync(int channelId, List<string> useridlist)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var batchDeleteMemberUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/batchdelete?access_token={0}", configDto.AccessToken);

                var result = await HttpHelper.PostDataAsync<ResponseBaseDTO, List<string>>(batchDeleteMemberUrl, useridlist);
                return result;
            }
            else
            {
                return new ResponseBaseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行用户管理"
                };
            }
        }

        /// <summary>
        /// 获取部门成员
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="department_id">获取的部门id</param>
        /// <param name="fetch_child">1/0：是否递归获取子部门下面的成员</param>
        /// <returns></returns>
        public static async Task<GetDepartmentMemberListResponseDTO> GetDepartmentMemberList(int channelId, int department_id, int? fetch_child = null)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var getMemberListUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/simplelist?access_token={0}&department_id={1}&fetch_child={2}", configDto.AccessToken, department_id, fetch_child);

                var result = await HttpHelper.GetHtmlAsync<GetDepartmentMemberListResponseDTO>(getMemberListUrl);
                return result;
            }
            else
            {
                return new GetDepartmentMemberListResponseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行用户管理"
                };
            }
        }

        /// <summary>
        /// 获取部门成员详情
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="department_id">获取的部门id</param>
        /// <param name="fetch_child">1/0：是否递归获取子部门下面的成员</param>
        /// <returns></returns>
        public static async Task<GetDepartmentMemberDetailListResponseDTO> GetDepartmentMemberDetailList(int channelId, int department_id, int? fetch_child = null)
        {
            var validateResult = ValidateChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                var getMemberListUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/list?access_token={0}&department_id={1}&fetch_child={2}", configDto.AccessToken, department_id, fetch_child);

                var result = await HttpHelper.GetHtmlAsync<GetDepartmentMemberDetailListResponseDTO>(getMemberListUrl);
                return result;
            }
            else
            {
                return new GetDepartmentMemberDetailListResponseDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行用户管理"
                };
            }
        }
        #endregion
    }
}

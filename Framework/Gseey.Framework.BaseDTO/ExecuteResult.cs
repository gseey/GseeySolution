using System;

namespace Gseey.Framework.BaseDTO
{

    public class ExecuteResult
    {
        public void SetResult(bool result)
        {
            if (result)
                ErrorCode = ErrorCodeEnum.Success;
            else
                ErrorCode = ErrorCodeEnum.Fail;
            Success = result;
        }
        /// <summary>
        /// 执行结果错误编码
        /// </summary>
        public ErrorCodeEnum ErrorCode { get; set; }

        /// <summary>
        /// 执行结果返回值
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public bool Success { get; set; }


        /// <summary>
        /// 执行结果错误编码
        /// </summary>
        public enum ErrorCodeEnum
        {
            /// <summary>
            /// 执行成功
            /// </summary>
            Success = 0,

            /// <summary>
            /// 执行失败
            /// </summary>
            Fail = -99,
        }

        public ExecuteResult()
        {
            ErrorCode = ErrorCodeEnum.Fail;
            ErrorMsg = string.Empty;
        }
    }

    public class ExecuteResult<T> : ExecuteResult
    {
        public T Data { get; set; }

        public ExecuteResult()
        {
            Data = default(T);
        }
    }
}

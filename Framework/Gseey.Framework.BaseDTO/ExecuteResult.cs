namespace Gseey.Framework.BaseDTO
{
    /// <summary>
    /// Defines the <see cref="ExecuteResult" />
    /// </summary>
    public class ExecuteResult
    {
        /// <summary>
        /// The SetResult
        /// </summary>
        /// <param name="result">The result<see cref="bool"/></param>
        public void SetResult(bool result)
        {
            if (result)
                ErrorCode = ErrorCodeEnum.Success;
            else
                ErrorCode = ErrorCodeEnum.Fail;
            Success = result;
        }

        /// <summary>
        /// Gets or sets the ErrorCode
        /// 执行结果错误编码
        /// </summary>
        public ErrorCodeEnum ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the ErrorMsg
        /// 执行结果返回值
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Success
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteResult"/> class.
        /// </summary>
        public ExecuteResult()
        {
            ErrorCode = ErrorCodeEnum.Fail;
            ErrorMsg = string.Empty;
        }
    }

    /// <summary>
    /// Defines the <see cref="ExecuteResult{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExecuteResult<T> : ExecuteResult
    {
        /// <summary>
        /// Gets or sets the Data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteResult{T}"/> class.
        /// </summary>
        public ExecuteResult()
        {
            Data = default(T);
        }
    }
}

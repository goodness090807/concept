namespace Concept.Core.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? ErrorCode { get; }
        public string? Error { get; }

        protected Result(bool isSuccess, string? errorCode, string? error)
        {
            IsSuccess = isSuccess;
            ErrorCode = errorCode;
            Error = error;
        }

        // 成功結果（無回傳值）
        public static Result Success() => new Result(true, null, null);

        // 失敗結果
        public static Result Failure(string errorCode, string error) => new Result(false, errorCode, error);
    }

    public class Result<T> : Result
    {
        public T? Data { get; }

        protected Result(bool isSuccess, string? errorCode, string? error, T? data) : base(isSuccess, errorCode, error)
        {
            Data = data;
        }

        // 成功結果（有回傳值）
        public static Result<T> Success(T data) => new Result<T>(true, null, null, data);

        // 失敗結果
        public new static Result<T> Failure(string errorCode, string error) => new Result<T>(false, errorCode, error, default);
    }
}

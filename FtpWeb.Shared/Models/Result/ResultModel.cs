namespace FtpWeb.Shared.Models.Result;

public class ResultModel<T> : ResultBase
{
    protected ResultModel() { }

    public ResultModel(T result)
    {
        Result = result;
        Success = true;
    }

    public ResultModel(bool success, params string[] errors)
    {
        Success = success;
        Errors = errors;
    }

    public T? Result { get; set; }
}

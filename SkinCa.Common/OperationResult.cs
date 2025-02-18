namespace SkinCa.Common;

public  class OperationResult<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    
    public static OperationResult<T> SuccessResult(T data,string? message = "Operation completed successfully.")
    {
        return new OperationResult<T> { Success = true,Data = data, Message = message };
    }
    public static OperationResult<T> FailureResult(T data,string? message = "Operation failed.")
    {
        return new OperationResult<T> { Success = false,Data = data, Message = message };
    }
    
}
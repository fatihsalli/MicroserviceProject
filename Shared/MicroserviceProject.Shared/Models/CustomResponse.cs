using System.Text.Json.Serialization;

namespace MicroserviceProject.Shared.Models;

public class CustomResponse<T>
{
    public T Data { get; set; }
    
    [JsonIgnore]
    public int StatusCode { get; set; }
    
    public List<string> Errors { get; set; }
    
    //Static factory method => Factory design pattern
    public static CustomResponse<T> Success(int statusCode, T data)
    {
        return new CustomResponse<T> { StatusCode = statusCode, Data = data};
    }
    
    public static CustomResponse<T> Success(int statusCode)
    {
        return new CustomResponse<T> { StatusCode = statusCode };
    }

    public static CustomResponse<T> Fail(int statusCode, List<string> errors)
    {
        return new CustomResponse<T> { StatusCode = statusCode, Errors = errors };
    }

    public static CustomResponse<T> Fail(int statusCode, string error)
    {
        return new CustomResponse<T> { StatusCode = statusCode, Errors = new List<string> { error } };
    }
}
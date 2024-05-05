using System.Text.Json.Serialization;

namespace BootcampApi.DTOs
{
    public record ResponseModelDto<T>
    {
        public T Data { get; set; }

        [JsonIgnore]
        public bool IsSuccess { get; set; }

        public List<string>? FailMessages { get; set; }


        public static ResponseModelDto<T> Success(T data)
        {
            return new ResponseModelDto<T>
            {
                Data = data,
                IsSuccess = true,
            };
        }

        public static ResponseModelDto<T> SuccessWithNoData()
        {
            return new ResponseModelDto<T>
            {
                IsSuccess = true,
            };
        }

        public static ResponseModelDto<T> Fail(List<string> messages)
        {
            return new ResponseModelDto<T>
            {
                FailMessages = messages,
                IsSuccess = false,
            };
        }

        public static ResponseModelDto<T> Fail(string message)
        {
            return new ResponseModelDto<T>
            {
                IsSuccess = false,
                FailMessages = new List<string> { message }
            };
        }
    }

    public struct NoContent;
}

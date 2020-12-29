using System;
using Newtonsoft.Json;

namespace GuideMaker.Core.Models
{
    [JsonObject]
    public class Result<T> where T: class
    {
        [JsonProperty("value", Required = Required.AllowNull)]
        private readonly T value;

        [JsonProperty("error", Required = Required.AllowNull)]
        private readonly string error;

        private Result(T value, string error)
        {
            this.value = value;
            this.error = error;
        }

        public T GetValue()
        {
            if (value is null)
                throw new ResultException($"Result is not successful. Error: {error}");

            return value;
        }

        public string GetError()
        {
            if (error is null)
                throw new ResultException("Result is successful");

            return error;
        }

        public static Result<T> Success(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            return new Result<T>(value, null);
        }

        public static Result<T> Error(string error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));
            return new Result<T>(null, error);
        }

        public bool IsSuccessfully() => value != null;
    }

    public class ResultException: Exception
    {
        public ResultException(string message): base(message)
        {
            
        }
    }
}

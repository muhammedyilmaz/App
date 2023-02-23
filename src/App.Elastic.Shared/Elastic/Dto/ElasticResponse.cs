using System;

namespace App.Elastic.Dto
{
    public class ElasticResponse<T> : IElasticResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public T Data { get; set; }

        public ElasticResponse() { }
        public ElasticResponse(bool isSuccess, string message, Exception exception, T data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Exception = exception;
            Data = data;
        }
        public ElasticResponse(bool isSuccess, string message, T data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }
        public ElasticResponse(bool isSuccess, T data)
        {
            IsSuccess = isSuccess;
            Data = data;
        }

        public ElasticResponse(bool isSuccess, string message, Exception exception)
        {
            IsSuccess = isSuccess;
            Message = message;
            Exception = exception;
        }
        public ElasticResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
        public ElasticResponse(Exception exception)
        {
            Exception = exception;
        }
        public ElasticResponse(Exception exception, string message)
        {
            Exception = exception;
            Message = message;
        }
        public ElasticResponse(string message)
        {
            Message = message;
        }
    }
}

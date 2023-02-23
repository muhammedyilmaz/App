using System;

namespace App.Elastic.Dto
{
    public interface IElasticResponse
    {
        bool IsSuccess { get; }
        string Message { get; }
        Exception Exception { get; }
    }
}

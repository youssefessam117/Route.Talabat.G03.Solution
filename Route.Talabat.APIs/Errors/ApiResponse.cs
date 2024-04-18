﻿
namespace Route.Talabat.APIs.Errors
{
	public class ApiResponse
	{
        public int StatusCode { get; set; }

		public string Message { get; set; }

		public ApiResponse(int statusCode , string? message=null)
		{
			StatusCode = statusCode;
			message = message ?? GetDefaultMessageForStatusCode(statusCode);

		}

		private string? GetDefaultMessageForStatusCode(int statusCode)
		{
			return statusCode switch
			{
				400 => "Bad Request, you have made",
				401 => "Authorized, you are not",
				404 => "Resource was not found",
				500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate. Hate leads to career change",
				_   =>  null,
			};
		}
	}
}

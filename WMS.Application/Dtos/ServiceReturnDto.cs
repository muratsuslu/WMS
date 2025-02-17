using WMS.Application.Wrappers;

namespace WMS.Application.Dtos
{
	public class ServiceReturnDto<T>
	{
		public ServiceReturnDto(bool success, string? message = null, T? data = default)
		{
			IsSuccess = success;
			Message = message;
			Data = data;
		}
		public static ServiceReturnDto<T> SuccessResponse(T data, string? message = null)
		{
			return new ServiceReturnDto<T>(true, message, data);
		}

		public static ServiceReturnDto<T> NoResultResponse(string? message = null)
		{
			return new ServiceReturnDto<T>(true, message);
		}

		public static ServiceReturnDto<T> ErrorResponse(string message)
		{
			return new ServiceReturnDto<T>(false, message);
		}
		public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
    }
    
}

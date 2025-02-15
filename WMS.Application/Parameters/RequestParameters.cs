namespace WMS.Application.Parameters
{
	public class RequestParameters
	{
		public int PageSize { get; set; } = 10;
		public int PageNumber { get; set; } = 0;
		public string OrderBy { get; set; } = "Created";
		public bool Desc { get; set; } = false;
    }
}

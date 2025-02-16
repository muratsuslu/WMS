namespace WMS.Application.Dtos
{
	public class ProductInsertDto
	{
		public string Name { get; set; }
	}

	public class ProductUpdateDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}

	public class ProductDeleteDto
	{
		public Guid Id { get; set; }
	}
}

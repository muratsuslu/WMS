namespace WMS.Application.Dtos
{
    public class CustomerInsertDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
    }

    public class CustomerUpdateDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }

    public class CustomerDeleteDto
    {
        public Guid Id { get; set; }
    }
}

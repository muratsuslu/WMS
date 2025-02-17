using WMS.Domain.Entities;

namespace WMS.Application.Dtos
{
	public class SkuLineHelperDto
	{
        public SkuLineHelperDto(Sku sku, Line line)
        {
            this.Sku = sku;
            this.Line = line;
        }
        public Sku Sku { get; set; }
        public Line Line { get; set; }
    }
}

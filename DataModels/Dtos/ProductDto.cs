namespace PosApi.DataModels.Dtos
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string ProductTypesId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductBarcode { get; set; }
        public double ProductSalePrice { get; set; }
        public string UnitName { get; set; }
        public string ProductImagePath { get; set; }
    }

    public class CreateUpdateProductDto : ProductDto
    {
        public IFormFile FileImage { get; set; }
    }

    public class ProductListDto
    {
        public string ProductsId { get; set; }
        public string ProductTypesId { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double ProductSalePrice { get; set; }
        public string UnitName { get; set; }
        public string ProductTypeImagePath { get; set; }
        public string ProductImagePath { get; set; }
    }
}
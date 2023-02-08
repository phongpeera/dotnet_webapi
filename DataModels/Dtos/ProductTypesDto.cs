namespace PosApi.DataModels.Dtos
{
    public class ProductTypeDto
    {
        public string Id { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductTypeImagePath { get; set; }
    }

    public class CreateUpdateProductTypeDto : ProductTypeDto
    {
        public IFormFile FileImage { get; set; }
    }
}
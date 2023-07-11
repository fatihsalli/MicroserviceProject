namespace MicroserviceProject.Services.Order.Application.Dtos;

public class AddressDto
{
    public string Province { get; set; }
    public string Disctrict { get; set; }
    public string Street { get; set; }
    public string Zip { get; set; }
    public string Line { get; set; }
}
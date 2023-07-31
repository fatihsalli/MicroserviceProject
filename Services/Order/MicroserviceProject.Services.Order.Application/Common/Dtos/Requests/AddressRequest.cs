namespace MicroserviceProject.Services.Order.Application.Common.Dtos.Requests;

public class AddressRequest
{
    public string Province { get; set; }
    public string District { get; set; }
    public string Street { get; set; }
    public string Zip { get; set; }
    public string Line { get; set; }
}
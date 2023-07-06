namespace MicroserviceProject.Shared.Models;

public class Address
{
    public string Id { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string AddressDetail { get; set; }
    public bool IsInvoiceAddress { get; set; }
    public bool IsRegularAddress { get; set; }
}
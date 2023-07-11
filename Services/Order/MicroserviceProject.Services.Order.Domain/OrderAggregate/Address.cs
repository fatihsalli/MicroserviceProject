using MicroserviceProject.Services.Order.Domain.Core;

namespace MicroserviceProject.Services.Order.Domain.OrderAggregate;

// Value Object
public class Address : ValueObject
{
    public string Province { get;private set; }
    public string Disctrict { get;private set; }
    public string Street { get;private set; }
    public string Zip { get;private set; }
    public string Line { get;private set; }

    // Dışarıdan state'inin değiştirilmemesi için private yaptık constructor'da kendimiz tanımlıyoruz.
    public Address(string province, string disctrict, string street, string zip, string line)
    {
        Province = province;
        Disctrict = disctrict;
        Street = street;
        Zip = zip;
        Line = line;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Province;
        yield return Disctrict;
        yield return Street;
        yield return Zip;
        yield return Line;
    }
}
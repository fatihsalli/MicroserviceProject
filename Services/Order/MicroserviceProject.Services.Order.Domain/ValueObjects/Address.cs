using MicroserviceProject.Services.Order.Domain.Common;

namespace MicroserviceProject.Services.Order.Domain.ValueObjects;

// Value Object
public class Address : ValueObject
{
    public string Province { get;private set; }
    public string District { get;private set; }
    public string Street { get;private set; }
    public string Zip { get;private set; }
    public string Line { get;private set; }

    // Dışarıdan state'inin değiştirilmemesi için private yaptık constructor'da kendimiz tanımlıyoruz.
    public Address(string province, string district, string street, string zip, string line)
    {
        Province = province;
        District = district;
        Street = street;
        Zip = zip;
        Line = line;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Province;
        yield return District;
        yield return Street;
        yield return Zip;
        yield return Line;
    }
}
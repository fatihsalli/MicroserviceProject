using MicroserviceProject.Services.Order.Domain.Common;

namespace MicroserviceProject.Services.Order.Domain.ValueObjects;

// Value Object
public class Address : ValueObject
{
    // Dışarıdan state'inin değiştirilmemesi için private yaptık constructor'da kendimiz tanımlıyoruz.
    public Address(string province, string district, string street, string zip, string line)
    {
        Province = province;
        District = district;
        Street = street;
        Zip = zip;
        Line = line;
    }

    public string Province { get; }
    public string District { get; }
    public string Street { get; }
    public string Zip { get; }
    public string Line { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Province;
        yield return District;
        yield return Street;
        yield return Zip;
        yield return Line;
    }
}
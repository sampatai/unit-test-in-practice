

using System.Xml.Linq;

namespace UnitTestInPractice.Domain.ValueObjects;
public class AssessmentTakerDetails : ValueObject
{
    protected AssessmentTakerDetails()
    {

    }
    public string FullName { get; private set; }
    public string Occupation { get; private set; }
    public string Address { get; private set; }
    public AssessmentTakerDetails(string fullName, string occupation, string address)
    {
        Guard.Against.NullOrEmpty(fullName, nameof(fullName));
        Guard.Against.NullOrEmpty(occupation, nameof(occupation));

        Guard.Against.NullOrEmpty(address, nameof(address));
        this.FullName = fullName;
        this.Occupation = occupation;
        this.Address = address;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FullName;
        yield return Occupation;
        yield return Address;

    }
}


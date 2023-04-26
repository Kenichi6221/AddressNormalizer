namespace AddressNormalizer.Domain;

public class ExpandedAddress
{
    public string Value { get; }

    public ExpandedAddress(string value)
    {
        Value = value;
    }
}
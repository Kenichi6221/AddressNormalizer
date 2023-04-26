using AddressNormalizer.Domain;
using AddressNormalizer.Domain.Common;

namespace AddressNormalizer.Application;

public interface INormalizerLibpostal
{
    Response<IEnumerable<ExpandedAddress>> ExpandAddress(ExpandedAddressQuery query);
    Response<IEnumerable<ParsedAddress>> ParseAddress(ParseAddressQuery query);
}
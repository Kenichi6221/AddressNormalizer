using AddressNormalizer.Application;
using AddressNormalizer.Domain;
using AddressNormalizer.Domain.Common;
using LibPostalNet;
using static AddressNormalizer.Domain.Common.ResponseHelper;

namespace AddressNormalizer.Infrastructure;

public class NormalizerLibpostal : INormalizerLibpostal, ILibpostalLoader
{
    private bool _wasDataLoaded;

    public NormalizerLibpostal()
    {
        _wasDataLoaded = false;
    }

    public Response<IEnumerable<ExpandedAddress>> ExpandAddress(ExpandedAddressQuery query)
    {
        VerifyData();
        var options = libpostal.LibpostalGetDefaultOptions();

        var expanded = TryRequest(
            () => libpostal.LibpostalExpandAddress(query.Address, options),
            nameof(ExpandAddress),
            null);

        if (!expanded.SuccessCall)
        {
            return Fail<IEnumerable<ExpandedAddress>>($"{expanded.Error}");
        }

        if (expanded.Value == null)
        {
            return Success(Enumerable.Empty<ExpandedAddress>());
        }

        return Success(expanded.Value.Expansions.Select(x => new ExpandedAddress(x)));
    }

    public Response<IEnumerable<ParsedAddress>> ParseAddress(ParseAddressQuery query)
    {
        VerifyData();
        var options = new LibpostalAddressParserOptions();

        var parsedAddress = TryRequest(
            () => libpostal.LibpostalParseAddress(query.Address, options),
            nameof(ParseAddress),
            null);

        if (!parsedAddress.SuccessCall)
        {
            return  Fail<IEnumerable<ParsedAddress>>($"{parsedAddress.Error}");
        }

        if (parsedAddress.Value == null)
        {
            return Success(Enumerable.Empty<ParsedAddress>());
        }

        return Success(parsedAddress.Value.Results.Select(x => new ParsedAddress(x.Key, x.Value)));
    }

    public void VerifyData()
    {
        if (!_wasDataLoaded)
        {
            var dataPath = @"../opt/libpostal";
            libpostal.LibpostalSetupDatadir(dataPath);
            libpostal.LibpostalSetupParserDatadir(dataPath);
            libpostal.LibpostalSetupLanguageClassifierDatadir(dataPath);
            _wasDataLoaded = true;
        }
    }

    public void Teardown()
    {
        libpostal.LibpostalTeardown();
        libpostal.LibpostalTeardownParser();
        libpostal.LibpostalTeardownLanguageClassifier();
        _wasDataLoaded = false;
    }

    private Response<R> TryRequest<R>(Func<R> func, string endpointName, R defaultResult)
    {
        try
        {
            var result = func();

            if (result == null)
            {
                return Success(defaultResult);
            }

            return Success(result);
        }
        catch (Exception ex)
        {
            return Fail<R>($"Failed to {endpointName} {ex.Message}");
        }
    }
}
using AddressNormalizer.Application;
using AddressNormalizer.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AddressNormalizer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LibpostalWrapperController: ControllerBase
{
    private readonly INormalizerLibpostal _libpostal;

    public LibpostalWrapperController(INormalizerLibpostal libpostal)
    {
        _libpostal = libpostal ?? throw new ArgumentNullException(nameof(libpostal));
    }

    [HttpPost, Route("expand")]
    public ActionResult<IEnumerable<ExpandedAddress>> GetExpandedAddress(ExpandedAddressQuery query)
    {
        var expandedAddresses =  _libpostal.ExpandAddress(query);

        if (!expandedAddresses.SuccessCall)
        {
            return StatusCode(500, expandedAddresses.Error);
        }
        
        if (expandedAddresses.Value!=null && !expandedAddresses.Value.Any())
        {
            return NotFound();
        }

        return Ok(expandedAddresses.Value);
    }
    
    [HttpPost, Route("parse")]
    public ActionResult<IEnumerable<ParsedAddress>>  GetParseAddress(ParseAddressQuery query)
    {
        var parsedAddress = _libpostal.ParseAddress(query);

        if (!parsedAddress.SuccessCall)
        {
            return StatusCode(500, parsedAddress.Error);
        }
        
        if (parsedAddress.Value!=null && !parsedAddress.Value.Any())
        {
            return NotFound();
        }
        
        return Ok(parsedAddress.Value);
    }
}
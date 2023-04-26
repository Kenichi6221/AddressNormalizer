namespace AddressNormalizer.Application;

public interface ILibpostalLoader
{
    void VerifyData();
    
    void Teardown();
}
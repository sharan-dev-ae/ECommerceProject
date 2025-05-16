using CompanyService.Models;

namespace CompanyService.Interfaces
{
    public interface ICompanyFactory
    {
        Company Create(string name, string streetAddress, string city, string state, string postalAddress, string zip, string contactNumber);
    }
}

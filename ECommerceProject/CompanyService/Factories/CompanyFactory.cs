using CompanyService.Interfaces;
using CompanyService.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CompanyService.Factories
{
    public class CompanyFactory : ICompanyFactory
    {
        public Company Create(string name, string streetAddress, string city, string state, string postalAddress, string zip, string contactNumber)
        {
            return new Company
            {
                Id = Guid.NewGuid(),
                Name = name,
                StreetAddress = streetAddress,
                City = city,
                State = state,
                PostalAddress = postalAddress,
                Zip = zip,
                ContactNumber = contactNumber
            };
        }
    }
}

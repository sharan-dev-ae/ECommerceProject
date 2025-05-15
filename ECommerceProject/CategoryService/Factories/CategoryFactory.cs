using CategoryService.Interfaces;
using CategoryService.Models;

namespace CategoryService.Factories
{
    public class CategoryFactory : ICategoryFactory
    {
        public Category Create(string name, string type)
        {
            return new Category
            {
                Id = Guid.NewGuid(),
                Name = name,
                Type = type
            };
        }
    }
}

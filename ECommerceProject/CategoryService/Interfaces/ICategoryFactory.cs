using CategoryService.Models;

namespace CategoryService.Interfaces
{
    public interface ICategoryFactory
    {
        Category Create(string name, string type);
    }
}

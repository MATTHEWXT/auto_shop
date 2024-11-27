using HieLie.Application.Models.Request;
using HieLie.Domain.Entities;

namespace HieLie.Application.Interfaces
{
    public interface ICategoryService
    {
        Task CreateAsync(CreateCategoryReq req);
        Task<IList<Category>> GetAll();
    }
}
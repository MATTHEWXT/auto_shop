using HieLie.Application.Interfaces;
using HieLie.Application.Models.Request;
using HieLie.Domain.Core.Repositories;
using HieLie.Domain.Core.Specifications;
using HieLie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HieLie.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(CreateCategoryReq req)
        {
            var isExistCategory = await _unitOfWork.Repository<Category>()
                .FirstOrDefaultAsync(CategorySpecification
                .GetCategoryByName(req.Name));

            if (isExistCategory != null)
            {
                throw new InvalidOperationException();
            }

            Category category = new Category(req.Name);
            category.Validate();
            

            await _unitOfWork.Repository<Category>().AddAsync(category);

        }

        public async Task<IList<Category>> GetAll()
        {
            return await _unitOfWork.Repository<Category>().GetAllAsync();
        }

    }
}

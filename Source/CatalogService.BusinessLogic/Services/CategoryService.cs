﻿using AutoMapper;
using CatalogService.BusinessLogic.Services.Interfaces;
using CatalogService.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.BusinessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<MODELS.Category> categoryRepository;
        private readonly IMapper mapper;

        public CategoryService(IRepository<MODELS.Category> categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<DTO.Category> GetByIdAsync(int categoryId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var categoryModel = await this.categoryRepository.GetByIdAsync(categoryId, cancellationToken);
            var result = this.mapper.Map<DTO.Category>(categoryModel);

            return result;
        }

        public async Task<List<DTO.Category>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var categoryModels = await this.categoryRepository.GetAll().ToListAsync(cancellationToken);
            var result = this.mapper.Map<List<DTO.Category>>(categoryModels);

            return result;
        }

        public async Task CreateAsync(DTO.Category category, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(category);

            var categoryModel = this.mapper.Map<MODELS.Category>(category);
            await this.categoryRepository.CreateAsync(categoryModel, cancellationToken);
        }

        public async Task UpdateAsync(DTO.Category category, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(category);

            var categoryModel = this.mapper.Map<MODELS.Category>(category);
            await this.categoryRepository.UpdateAsync(categoryModel, cancellationToken);
        }

        public async Task DeleteAsync(int categoryId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.categoryRepository.DeleteAsync(categoryId, cancellationToken);
        }
    }
}
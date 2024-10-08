﻿using Microsoft.EntityFrameworkCore;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;

namespace TrueBalances.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TrueBalancesDbContext _context;

        public CategoryRepository(TrueBalancesDbContext context)
        {
            _context = context;
        }
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.Where(c => c.Id != 4).ToListAsync();
        }

        public async Task<int> AddAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                // Mettre toute les valeurs CategoryId des Expenses concernée à 4 (valeur pour "Catégorie supprimée")
                var expensesToUpdate = _context.Expenses.Where(e => e.CategoryId == category.Id).ToList();
                expensesToUpdate.ForEach(e => e.CategoryId = 4);

                _context.Categories.Remove(category);
                return await _context.SaveChangesAsync();
            }

            return -1;
        }

        public async Task<Category> GetCategoryWithExpensesByIdAsync(int id, int groupId)
        {
            return await _context.Categories
            .Include(c => c.Expenses.Where(e => e.GroupId == groupId))
            .ThenInclude(e => e.User)
            .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}

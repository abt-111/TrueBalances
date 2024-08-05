using Microsoft.EntityFrameworkCore;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;

namespace TrueBalances.Repositories.DbRepositories
{
    public class CategoryRepository : IGenericRepository<Category>
    {
        private readonly UserContext _context;

        public CategoryRepository(UserContext context)
        {
            _context = context;
        }
        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
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
                _context.Categories.Remove(category);
                return await _context.SaveChangesAsync();
            }

            return -1;
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(e => e.Id == id);
        }
    }

}


//public async Task<List<Category>> GetAllCategoriesAsync()
//{
//    return await _context.Categories.ToListAsync();
//}

//public async Task<Category?> GetCategoryByIdAsync(int id)
//{
//    return await _context.Categories.FindAsync(id);
//}

//public async Task AddCategoryAsync(Category category)
//{
//    _context.Categories.Add(category);
//    await _context.SaveChangesAsync();
//}

//public async Task UpdateCategoryAsync(Category category)
//{
//    _context.Categories.Update(category);
//    await _context.SaveChangesAsync();
//}

//public async Task DeleteCategoryAsync(int id)
//{
//    var category = await _context.Categories.FindAsync(id);
//    if (category != null)
//    {
//        _context.Categories.Remove(category);
//        await _context.SaveChangesAsync();
//    }
//}

//public async Task<bool> CategoryExistsAsync(int id)
//{
//    return await _context.Categories.AnyAsync(e => e.Id == id);
//}
//    }


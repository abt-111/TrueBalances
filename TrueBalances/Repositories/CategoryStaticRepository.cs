using TrueBalances.Models;


namespace TrueBalances.Repositories
{
    public static class CategoryStaticRepository
    {
        private static List<Category> _categories = new List<Category>()
        {
            new Category()
            {
                Id = 1,
                Name = "Voyage",
            },
            new Category()
            {
                Id = 2,
                Name = "Couple",
            },
            new Category()
            {
                Id = 3,
                Name = "Co-voiturage",
            }
        };
        //Get Id Unique
        private static int GetUniqueId() => _categories.Count > 0 ? _categories.Max(x => x.Id) + 1 : 1;

        /// Get all Categories
        private static List<Category> GetCategories() => _categories;

        /// Get one specific category with his id
        public static Category? GetStudentById(int idCategory) => _categories.Find(x => x.Id == idCategory);

        public static void Addcategory(string name)
        {
            var idNewCategory = GetUniqueId();
            var newCategory = new Category()
            {
                Id = idNewCategory,
                Name = name,
                
            };
            _categories.Add(newCategory);
        }

        public static void UpdateCategory(Category CategoryToUpdate)
        {
            var category = _categories.Find(x => x.Id == CategoryToUpdate.Id);

            if (category is null) return;

            category.Name = CategoryToUpdate.Name;

        }

        public static bool DeleteCategory(int categoryId)
        {
            var category = _categories.Find(x => x.Id == categoryId);

            if (category is null) return false;

            _categories.Remove(category);
            return true;
        }



    }
}

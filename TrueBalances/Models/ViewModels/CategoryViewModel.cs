namespace TrueBalances.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int GroupId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
    }
}

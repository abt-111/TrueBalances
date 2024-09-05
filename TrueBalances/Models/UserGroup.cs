namespace TrueBalances.Models
{
    public class UserGroup
    {
        public int Id { get; set; }

        // Clés étrangères
        public string CustomUserId { get; set; }
        public int GroupId { get; set; }

        // Propriétés de navigation
        public CustomUser CustomUser { get; set; }
        public Group Group { get; set; }

        //public ApplicationUser User { get; set; }
    }
}

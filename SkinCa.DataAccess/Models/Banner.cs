namespace SkinCa.DataAccess
{
    public class Banner:Entity<int>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public byte[] Image { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
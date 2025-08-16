namespace SkinCa.DataAccess
{
    public class ApplicationUserChat
    {
        public string UserId {  get; set; }
        public int ChatId {  get; set; }
        public bool IsDeleted { get; set; }
        public ApplicationUser User { get; set; }
        public virtual Chat Chat { get; set; }
    }
}

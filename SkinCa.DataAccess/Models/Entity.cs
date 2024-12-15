namespace SkinCa.DataAccess;

public abstract class Entity<T>
{
    public T Id { get; set; }
    DateTime Created{ get; set; }
    DateTime LastModified{ get; set; }
    
}
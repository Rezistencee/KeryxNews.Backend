namespace KeryxNews.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string FullName { get; private set; }
    public string AvatarUrl { get; private set; }
    
    public User(Guid id, string email, string fullName, string avatarUrl)
    {
        Id = id;
        Email = email;
        FullName = fullName;
        AvatarUrl = avatarUrl;
    } 
    
    public User(string email, string fullName, string avatarUrl)
    {
        Email = email;
        FullName = fullName;
        AvatarUrl = avatarUrl;
    }
}
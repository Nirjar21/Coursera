using System.Collections.Generic;
using System.Linq;

namespace UserManagementAPI.Models;

public interface IUserRepository
{
    List<User> GetAll();
    User? GetById(int id);
    User Add(User user);
    User? Update(int id, User user);
    bool Delete(int id);
}

public class UserRepository : IUserRepository
{
    private static List<User> _users = new List<User>();
    private static int _nextId = 1;

    public List<User> GetAll() => _users;

    public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);

    public User Add(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
        return user;
    }

    public User? Update(int id, User user)
    {
        var existing = GetById(id);
        if (existing == null) return null;
        
        existing.Name = user.Name;
        existing.Email = user.Email;
        return existing;
    }

    public bool Delete(int id)
    {
        var user = GetById(id);
        if (user == null) return false;
        return _users.Remove(user);
    }
}
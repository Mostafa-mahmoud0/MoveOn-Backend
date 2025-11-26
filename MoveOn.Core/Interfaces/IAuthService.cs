using MoveOn.Core.Entities;
using System.Threading.Tasks;

namespace MoveOn.Core.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(string email, string password, string firstName, string lastName);
    Task<string> LoginAsync(string email, string password);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByEmailAsync(string email);
}
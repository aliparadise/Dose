using System.Threading.Tasks;
using Dose.Auth.Models;


namespace Dose.Auth
{
    public interface IFirebaseAuthService
    {
        Task<FirebaseUser> Login(Credentials credentials);
        Task<FirebaseUser> Register(Registration registration);
    }
}
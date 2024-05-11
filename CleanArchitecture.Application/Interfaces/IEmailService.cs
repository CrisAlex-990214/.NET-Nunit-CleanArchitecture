using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Interfaces
{
    public interface IEmailService
    {
        bool SendConfirmationEmail(User user);
    }
}

using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application
{
    public class UserService
    {
        private readonly IDatabaseService dbService;
        private readonly IEmailService emailService;

        public UserService(IDatabaseService dbService, IEmailService emailService)
        {
            this.dbService = dbService;
            this.emailService = emailService;
        }

        public int RegisterUser(CreateUserDto dto)
        {
            //1. Validate dto
            if (string.IsNullOrEmpty(dto.Name)) throw new Exception("ValidationError");

            //2. Map the dto into a user entity
            var user = new User { Name = dto.Name };

            //3. Store the user in the db
            var id = dbService.CreateUser(user);

            //4. Send confirmation email
            var sent = emailService.SendConfirmationEmail(user);
            if (!sent) throw new Exception("ConfirmationEmailError");

            return id;
        }
    }
}

using FluentValidation;

namespace Concept.API.Controllers.User.Requests
{
    public class RegisterRequest
    {
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 確認密碼
        /// </summary>
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class RegisterUserRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("請輸入使用者名稱");
            RuleFor(x => x.Email).NotEmpty().WithMessage("請輸入Email");
            RuleFor(x => x.Password).NotEmpty().WithMessage("請輸入密碼");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("請輸入確認密碼");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email格式不正確");

            RuleFor(x => x.Password)
                .Equal(x => x.ConfirmPassword)
                .WithMessage("密碼與確認密碼不相同");
        }
    }
}

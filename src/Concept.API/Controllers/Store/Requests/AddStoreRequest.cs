using FluentValidation;

namespace Concept.API.Controllers.Store.Requests
{
    public class AddStoreRequest
    {
        /// <summary>
        /// 商店名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }

    public class AddStoreRequestValidator : AbstractValidator<AddStoreRequest>
    {
        public AddStoreRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("請輸入商店名稱");
        }
    }
}
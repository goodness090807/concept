using Concept.Core.Entities.Role.Enums;
using FluentValidation;

namespace Concept.API.Controllers.Store.Requests
{
    public class GrantStoreRoleRequest
    {
        public IEnumerable<Roles> Roles { get; set; } = Enumerable.Empty<Roles>();

        public class GrantStoreRoleRequestValidator : AbstractValidator<GrantStoreRoleRequest>
        {
            public GrantStoreRoleRequestValidator()
            {
                RuleFor(x => x.Roles).NotEmpty().WithMessage("至少需要一個角色");
                RuleForEach(x => x.Roles).IsInEnum().WithMessage("角色無效");
            }
        }
    }
}

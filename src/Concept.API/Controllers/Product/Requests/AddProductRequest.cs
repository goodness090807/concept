using FluentValidation;

namespace Concept.API.Controllers.Product.Requests
{
    public class AddProductRequest
    {
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// 商品價格
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// 商品庫存
        /// </summary>
        public int Stock { get; set; }
    }

    public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
    {
        public AddProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("商品名稱不能為空")
                .MaximumLength(100).WithMessage("商品名稱最多100個字元");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("商品描述最多1000個字元");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("價格不能為負數");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("庫存不能為負數");
        }
    }
}

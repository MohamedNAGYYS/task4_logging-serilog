using MediatR;
using Microsoft.Extensions.Logging;
using Task3.Application.Common.Interfaces;
using Task3.Application.Products.DTOs;

namespace Task3.Application.Products.Queries
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<GetProductByIdHandler> _logger;

        public GetProductByIdHandler(IProductRepository repository, ILogger<GetProductByIdHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken ct)
        {
            _logger.LogInformation("Fetching product with Id {ProductId}", request.Id);

            var product = await _repository.GetByIdAsync(request.Id);

            if (product == null)
            {
                _logger.LogWarning("Product with Id {ProductId} was not found", request.Id);
                return null;
            }

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }
    }
}
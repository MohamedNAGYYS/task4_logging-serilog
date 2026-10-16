using MediatR;
using Microsoft.Extensions.Logging;
using Task3.Application.Common.Interfaces;
using Task3.Domain.Entities;

namespace Task3.Application.Products.Commands
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _repository;

        // ilogger
        private readonly ILogger<CreateProductHandler> _logger;

        public CreateProductHandler(IProductRepository repository, ILogger<CreateProductHandler> logger)
        {
            _repository = repository;
            _logger =  logger;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken ct)
        {

            // info
            _logger.LogInformation("Creating product: {ProductName} with price {Price}", request.Name, request.Price);
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price
            };

            var id = await _repository.AddAsync(product);

            _logger.LogInformation("Product {ProductId} created successfully", id);

            return id;
        }
    }
}
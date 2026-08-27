using FintechPlatform.Contracts.Protos;
using LoanService.Application.Common;

namespace LoanService.Infrastructure.Services
{
    public class GrpcBillingServiceChannel : IBillingServiceChannel
    {
        private readonly WalletGrpcService.WalletGrpcServiceClient _client;

        public GrpcBillingServiceChannel(WalletGrpcService.WalletGrpcServiceClient client)
        {
            _client = client;
        }

        public async Task<bool> IsWalletActiveAsync(Guid userId, CancellationToken cancellationToken)
        {
            var request = new CheckWalletRequest
            {
                UserId = userId.ToString()
            };

            try
            {
                var response = await _client.CheckWalletExistsAsync(request, cancellationToken: cancellationToken);

                return response.Exists;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}

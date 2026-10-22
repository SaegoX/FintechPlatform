using BillingService.Application.Common;
using FintechPlatform.Contracts.Protos;
using Grpc.Core;

namespace BillingService.Infrastructure.Services
{
    public class WalletGrpcServer : WalletGrpcService.WalletGrpcServiceBase
    {
        private readonly IWalletRepository _walletRepository;

        public WalletGrpcServer(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public override async Task<CheckWalletResponse> CheckWalletExists(
            CheckWalletRequest request,
            ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Невалидный идентификатора пользователя"));
            }

            var wallet = await _walletRepository.GetByUserIdAsync(userId, context.CancellationToken);

            return new CheckWalletResponse
            {
                Exists = wallet != null,
                Currency = wallet?.Currency ?? string.Empty,
                Balance = (double)(wallet?.Balance ?? 0m),
            };
        }
    }
}

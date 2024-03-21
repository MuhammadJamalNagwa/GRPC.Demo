using Grpc.Core;
using GRPC.Service.JWT;
using GRPC.Service.Protos;

namespace GRPC.Service.Services;

public class AuthenticationService : IAuthentication.IAuthenticationBase
{
    public override async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, ServerCallContext context)
    {
        AuthenticationResponse token = JwtAuthenticationManager.Authenticate(request);

        if (token is null)
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid User Credentials"));

        return token;
    }
}

using Domain;
using Mapster;

namespace Application.Authentication;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserDTO, UserResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Password, src => src.Password)
            .Map(dest => dest.Token, src => src.Token)
            .IgnoreNonMapped(true);
    }
}
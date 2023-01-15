// using Application.SPI;
// using Domain;

// namespace Infrastructure.Services
// {
//     public class AuthenticationService : IAuthenticationService
//     {
//         private readonly IJWTTokenGenerator _jwtTokenGenerator;

//         public AuthenticationService(IJWTTokenGenerator jwtTokenGenerator)
//         {
//             _jwtTokenGenerator = jwtTokenGenerator;
//         }

//         public async Task<UserDTO> Login(LoginDTO loginDTO)
//         {
//             return new UserDTO(Guid.NewGuid(), "John", "Doe", loginDTO.Email, loginDTO.Password, "token");
//         }

//         public async Task<UserDTO> Register(UserDTO userDTO)
//         {
//             // Create token
//             var token = _jwtTokenGenerator.CreateToken(userDTO);
//             var user = new UserDTO(Guid.NewGuid(), userDTO.FirstName, userDTO.LastName, userDTO.Email, userDTO.Password, token);

//             return user;
//         }
//     }
// }
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using Rafiq.Application.Dtos;
//using Rafiq.Domain.Common;
//using Rafiq.Domain.Entities;
//using Rafiq.Domain.Enums;
//using Rafiq.Domain.Settings;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;

//namespace Rafiq.Infrastructure.Services.Auth
//{
//    public class AuthService : IAuthService
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly JwtSettings _jwtSettings;

//        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings)
//        {
//            _userManager = userManager;
//            _jwtSettings = jwtSettings.Value;
//        }

//        // ------------------------ REGISTER ------------------------
//        public async Task<ResponseDto> RegisterAsync(RegisterRequestDto model)
//        {
//            if (await _userManager.FindByEmailAsync(model.Email) is not null)
//                return new ResponseDto { Message = "Email is already registered!" };

//            if (await _userManager.FindByNameAsync(model.Username) is not null)
//                return new ResponseDto { Message = "Username is already registered!" };

//            ApplicationUser user;

//            if (model.UserType.Equals("Family", StringComparison.OrdinalIgnoreCase))
//            {
//                user = new FamilyProfile
//                {
//                    UserName = model.Username,
//                    Email = model.Email,
//                    FirstName = model.FirstName,
//                    LastName = model.LastName,
//                    Relationship = Relationship.Father,
//                    EmergencyContactName = model.EmergencyContactName ?? string.Empty,
//                    EmergencyContactPhone = model.EmergencyContactPhone ?? string.Empty,
//                    Role = UserRole.Family
//                };
//            }
//            else if (model.UserType.Equals("Specialist", StringComparison.OrdinalIgnoreCase))
//            {
//                user = new SpecialistProfile
//                {
//                    UserName = model.Username,
//                    Email = model.Email,
//                    FirstName = model.FirstName,
//                    LastName = model.LastName,
//                    Credentials = model.Credentials ?? string.Empty,
//                    Specialty = model.Specialty ?? string.Empty,
//                    Organization = model.Organization ?? string.Empty,
//                    Role = UserRole.Specialist
//                };
//            }
//            else
//            {
//                return new ResponseDto { Message = "Invalid User Type" };
//            }

//            var result = await _userManager.CreateAsync(user, model.Password);

//            if (!result.Succeeded)
//                return new ResponseDto { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };

//            await _userManager.AddToRoleAsync(user, model.UserType);

//            var jwtSecurityToken = await CreateJwtToken(user);

//            var refreshToken = GenerateRefreshToken();
//            user.RefreshTokens?.Add(refreshToken);
//            await _userManager.UpdateAsync(user);

//            return new ResponseDto
//            {
//                Email = user.Email ?? string.Empty,
//                ExpiresOn = jwtSecurityToken.ValidTo,
//                IsAuthenticated = true,
//                Roles = new List<string> { model.UserType },
//                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
//                Username = user.UserName ?? string.Empty,
//                RefreshToken = refreshToken.Token,
//                RefreshTokenExpiration = refreshToken.ExpiresOn
//            };
//        }

//        // ------------------------ LOGIN ------------------------
//        public async Task<ResponseDto> LoginAsync(LoginRequestDto model)
//        {
//            var authModel = new ResponseDto();
//            var user = await _userManager.FindByEmailAsync(model.Email);

//            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
//            {
//                authModel.Message = "Email or Password is incorrect!";
//                return authModel;
//            }

//            var jwtSecurityToken = await CreateJwtToken(user);
//            var roles = await _userManager.GetRolesAsync(user);

//            authModel.IsAuthenticated = true;
//            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
//            authModel.Email = user.Email ?? string.Empty;
//            authModel.Username = user.UserName ?? string.Empty;
//            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
//            authModel.Roles = roles.ToList();

//            if (user.RefreshTokens?.Any(t => t.IsActive) == true)
//            {
//                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
//                if (activeRefreshToken != null)
//                {
//                    authModel.RefreshToken = activeRefreshToken.Token;
//                    authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
//                }
//            }
//            else
//            {
//                var refreshToken = GenerateRefreshToken();
//                authModel.RefreshToken = refreshToken.Token;
//                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
//                user.RefreshTokens?.Add(refreshToken);
//                await _userManager.UpdateAsync(user);
//            }

//            return authModel;
//        }

//        // ------------------------ CREATE JWT ------------------------
//        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
//        {
//            var userClaims = await _userManager.GetClaimsAsync(user);
//            var roles = await _userManager.GetRolesAsync(user);

//            var claims = new List<Claim>
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
//                new Claim("uid", user.Id ?? string.Empty)
//            };

//            claims.AddRange(userClaims);
//            claims.AddRange(roles.Select(role => new Claim("roles", role)));

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: _jwtSettings.Issuer,
//                audience: _jwtSettings.Audience,
//                claims: claims,
//                expires: DateTime.UtcNow.AddDays(_jwtSettings.DurationInDays),
//                signingCredentials: creds
//            );

//            return token;
//        }

//        public async Task<ResponseDto> RefreshTokenAsync(string? token)
//        {
//            var authModel = new ResponseDto();

//            if (string.IsNullOrEmpty(token))
//            {
//                authModel.Message = "Invalid token";
//                return authModel;
//            }

//            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens != null && u.RefreshTokens.Any(t => t.Token == token));

//            if (user == null)
//            {
//                authModel.Message = "Invalid token";
//                return authModel;
//            }

//            var refreshToken = user.RefreshTokens?.Single(t => t.Token == token);

//            if (refreshToken == null || !refreshToken.IsActive)
//            {
//                authModel.Message = "Inactive token";
//                return authModel;
//            }

//            refreshToken.RevokedOn = DateTime.UtcNow;

//            var newRefreshToken = GenerateRefreshToken();
//            user.RefreshTokens?.Add(newRefreshToken);
//            await _userManager.UpdateAsync(user);

//            var jwtToken = await CreateJwtToken(user);
//            authModel.IsAuthenticated = true;
//            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
//            authModel.Email = user.Email ?? string.Empty;
//            authModel.Username = user.UserName ?? string.Empty;
//            var roles = await _userManager.GetRolesAsync(user);
//            authModel.Roles = roles.ToList();
//            authModel.RefreshToken = newRefreshToken.Token;
//            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

//            return authModel;
//        }

//        public async Task<bool> RevokeTokenAsync(string? token)
//        {
//            if (string.IsNullOrEmpty(token))
//                return false;

//            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens != null && u.RefreshTokens.Any(t => t.Token == token));

//            if (user == null)
//                return false;

//            var refreshToken = user.RefreshTokens?.Single(t => t.Token == token);

//            if (refreshToken == null || !refreshToken.IsActive)
//                return false;

//            refreshToken.RevokedOn = DateTime.UtcNow;

//            await _userManager.UpdateAsync(user);

//            return true;
//        }

//        private RefreshToken GenerateRefreshToken()
//        {
//            var randomNumber = new byte[32];

//            using var generator = RandomNumberGenerator.Create();
//            generator.GetBytes(randomNumber);

//            return new RefreshToken
//            {
//                Token = Convert.ToBase64String(randomNumber),
//                ExpiresOn = DateTime.UtcNow.AddDays(10),
//                CreatedOn = DateTime.UtcNow
//            };
//        }
//    }
//}
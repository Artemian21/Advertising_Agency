using Advertising_Agency.BusinessLogic.Interfaces;
using Advertising_Agency.DataAccess.Entities;
using Advertising_Agency.DataAccess.Interfaces;
using Advertising_Agency.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Advertising_Agency.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<UserDto> RegisterAsync(UserRegistrationDto registrationDto)
        {
            if (await _userRepository.ExistsByUsernameAsync(registrationDto.Username))
                throw new Exception("Username is already taken");

            if (await _userRepository.ExistsByEmailAsync(registrationDto.Email))
                throw new Exception("Email is already registered");

            var user = _mapper.Map<User>(registrationDto);
            user.PasswordHash = _passwordHasher.HashPassword(registrationDto.Password);

            await _userRepository.AddAsync(user);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<string> AuthenticateAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, loginDto.Password))
                throw new Exception("Invalid username or password");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

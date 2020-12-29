using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuideMaker.Configuration;
using GuideMaker.Core.Models;
using GuideMaker.Exceptions;
using GuideMaker.Helpers;
using GuideMaker.Repository.Filters;
using GuideMaker.Repository.Repositories;
using Microsoft.Extensions.Options;
using DbUser = GuideMaker.Repository.Models.User;

namespace GuideMaker.Services
{
    public interface IUserService
    {
        Task<bool> ExistsAsync(string id);
        Task<Result<TokenInformation>> SignInAsync(UserAuthData userAuthData);

        Task<Result<TokenInformation>> SignUpAsync(UserAuthData userAuthData);

        Task<Result<User>> GetByTokenAsync(string token);

        Task<Result<User>> GetAsync(string id);
    }

    internal sealed class UserService: IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IOptions<AuthConfiguration> authConfigurationOptions;

        public UserService(IUserRepository userRepository, IOptions<AuthConfiguration> authConfigurationOptions)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.authConfigurationOptions = authConfigurationOptions ?? throw new ArgumentNullException(nameof(authConfigurationOptions));
        }

        public Task<bool> ExistsAsync(string id)
        {
            return userRepository.ExistsAsync(id);
        }

        public async Task<Result<TokenInformation>> SignInAsync(UserAuthData userAuthData)
        {
            if (!await userRepository.ExistsAsync(userAuthData.Login))
                throw new NotFoundException($"User with login '{userAuthData.Login}' not found");

            var repositoryUser = await userRepository.GetAsync(userAuthData.Login);

            var actualPasswordHash = PasswordHelper.GetHash(userAuthData.Password, authConfigurationOptions.Value.Salt);

            if (actualPasswordHash == repositoryUser.PasswordHash)
                return Result<TokenInformation>.Success(new TokenInformation
                {
                    Token = repositoryUser.Token
                });

            throw new UnauthorizedException("Login or password is invalid");
        }

        public async Task<Result<TokenInformation>> SignUpAsync(UserAuthData userAuthData)
        {
            if (await userRepository.ExistsAsync(userAuthData.Login))
                throw new ConflictException($"User with login '{userAuthData.Login}' already exists");

            var passwordHash = PasswordHelper.GetHash(userAuthData.Password, authConfigurationOptions.Value.Salt);

            var user = new DbUser
            {
                Id = userAuthData.Login,
                PasswordHash = passwordHash,
                Token = Guid.NewGuid().ToString()
            };

            await userRepository.SaveAsync(user);

            return Result<TokenInformation>.Success(new TokenInformation
            {
                Token = user.Token
            });
        }

        public async Task<Result<User>> GetByTokenAsync(string token)
        {
            var filter = new ComparisonFilter("token", token, ComparisonOperator.Equal);
            var dbUsers = await userRepository.SearchAsync(filter, take: 1, skip: 0);
            if (dbUsers.Length < 1)
                throw new UnauthorizedException("Token is invalid");
            if (dbUsers.Length > 1)
                throw new Exception($"Found {dbUsers.Length} with same token. {string.Join(", ", dbUsers.Select(x => x.Id))}");

            return Result<User>.Success(new User
            {
                    Id = dbUsers.First().Id
            });
        }

        public async Task<Result<User>> GetAsync(string id)
        {
            if (!await userRepository.ExistsAsync(id))
                throw new NotFoundException($"User with id '{id}' not found");

            var dbUser = await userRepository.GetAsync(id);
            return Result<User>.Success(new User
            {
                Id = dbUser.Id
            });
        }
    }
}

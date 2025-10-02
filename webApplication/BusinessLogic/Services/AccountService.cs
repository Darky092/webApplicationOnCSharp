using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Authorization;
using BusinessLogic.Helpers;
using BusinessLogic.Models.Accounts;
using Domain.Interfaces;
using Domain.Models;
using MapsterMapper;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;
using System.Security.Cryptography;
using Domain.Entities;

namespace BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IJwtUtils _JwtUtils;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public AccountService(
            IRepositoryWrapper repositoryWrapper, IJwtUtils jwtUtils, IMapper mapper, AppSettings appSettings, IEmailService emailService)
        {
            _repositoryWrapper = repositoryWrapper;
            _JwtUtils = jwtUtils;
            _mapper = mapper;
            _appSettings = appSettings;
            _emailService = emailService;
        }

        private void removeOldRefreshTokens(user account) 
        {
            account.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var account = await _repositoryWrapper.user.GetByEmailWithToken(model.email);

            //validate
            if (account == null || !account.IsVerified || !BCrypt.Net.BCrypt.Verify(model.passwordhash, account.passwordhash))
                throw new AppException("Email or password is incorrect");

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = _JwtUtils.GenerateJwtToken(account);
            var refreshToken = await _JwtUtils.GenerateRefreshToken(ipAddress);
            account.RefreshTokens.Add(refreshToken);

            //remove old refresh tokens from account
            removeOldRefreshTokens(account);

            //save changes to db
            await _repositoryWrapper.user.Update(account);
            await _repositoryWrapper.Save();

            var response = _mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = refreshToken.Token;
            return response;
        }

        public async Task<AccountResponse> Create(CreateRequest model)
        {
            //validate
            if ((await _repositoryWrapper.user.FindByCondition(x => x.email == model.email)).Count > 0)
                throw new AppException($"Email '{model.email}' is already reqistered");

            //man model to new account object
            var account = _mapper.Map<user>(model);
            account.Created = DateTime.UtcNow;
            account.Verifaied = DateTime.UtcNow;

            //hash password
            account.passwordhash = BCrypt.Net.BCrypt.HashPassword(model.passwordhash);

            //save account
            await _repositoryWrapper.user.Update(account);
            await _repositoryWrapper.Save();

            return _mapper.Map<AccountResponse>(account);
        }

        public async Task Delete(int id)
        {
            var account = await getAccount(id);
            await _repositoryWrapper.user.Delete(account);
            await _repositoryWrapper.Save();
        }


        private async Task<string> generateresetToken() 
        {
            // token is a cryptographically strong random sequence of values
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            // ensure token is unique by checking against db
            var tokenIsUnique = (await _repositoryWrapper.user.FindByCondition(x => x.ResetToken == token)).Count == 0;
            if (!tokenIsUnique)
                return await generateresetToken();

            return token;
        }

        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = (await _repositoryWrapper.user.FindByCondition(x => x.email == model.email)).FirstOrDefault();

            //always return ok response to pervent email enumerators
            if (account == null) return;

            //create reset token that expires after 1 day
            account.ResetToken = await generateresetToken();
            account.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            await _repositoryWrapper.user.Update(account);
            await _repositoryWrapper.Save();
        }

        private async Task<user> getAccountByRefreshToken(string token)
        {
            var account = (await _repositoryWrapper.user.FindByCondition(u => u.RefreshTokens.Any(t => t.Token == token))).SingleOrDefault();
            if (account == null) throw new AppException("Invalid token");
            return account;
        }

        private async Task<RefreshToken> rotateRefreshToken(RefreshToken refreshToken, string ipAddress) 
        {
            var newRefreshToken = await _JwtUtils.GenerateRefreshToken(ipAddress);
            revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedBytoken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplaceByToken = replacedBytoken;
        }

        private void revokeDescendantRefreshToken(RefreshToken refreshToken, user account, string ipAddress, string reason)
        {
            // recursively traverse the rafresh token chain and ensure all descandants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplaceByToken)) 
            {
                var childToken = account.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplaceByToken);
                if (childToken.IsActive)
                    revokeRefreshToken(childToken, ipAddress, reason);
                else
                    revokeDescendantRefreshToken(childToken, account, ipAddress, reason);
            }
        }

        public async Task<IEnumerable<AccountResponse>> GetAll()
        {
            var accounts = await _repositoryWrapper.user.FindAll();
            return _mapper.Map<IList<AccountResponse>>(accounts);
        }

        public async Task<AccountResponse> GetById(int id)
        {
            var account = await getAccount(id);
            return _mapper.Map<AccountResponse>(account);
        }

        public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            var account = await getAccountByRefreshToken(token);
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsRevoked)
            {
                //revoke all descendant tokens in case this has been compromised
                revokeDescendantRefreshToken(refreshToken, account, ipAddress, $"Attempted reuse of revoked ancestos token: {token}");
                await _repositoryWrapper.user.Update(account);
                await _repositoryWrapper.Save();
            }
            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");
            //replace old refresh tokens from account
            var newRefreshToken = await rotateRefreshToken(refreshToken, ipAddress);
            account.RefreshTokens.Add(newRefreshToken);

            //remove old refresh token from account
            removeOldRefreshTokens(account);

            //save changes to db
            await _repositoryWrapper.user.Update(account);
            await _repositoryWrapper.Save();

            //generate new jwt
            var jwtToken = _JwtUtils.GenerateJwtToken(account);

            //return data in authenticate response object
            var response = _mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = newRefreshToken.Token;
            return response;
        }

        private async Task<string> generateVerificationToken()
        {
            //token is a cryptografically strong random sequence of values
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            //ensure token is unique by checking against db
            var tokenIsUnique = (await _repositoryWrapper.user.FindByCondition(x => x.VerificationToken == token)).Count == 0;
            if (!tokenIsUnique)
                return await generateVerificationToken();
            return token;
        }

        public async Task Register(RegisterRequest model, string origin)
        {
            if ((await _repositoryWrapper.user.FindByCondition(x => x.email == model.email)).Count > 0)
            {
                return;
            }

            var account = _mapper.Map<user>(model);

            var isFirstAccount = (await _repositoryWrapper.user.FindAll()).Count == 0;
            var assignedRole = isFirstAccount ? RoleT.Admin : RoleT.User; 

            account.RoleT = assignedRole;
             

            account.Created = DateTime.UtcNow;
            account.Verifaied = DateTime.UtcNow;
            account.VerificationToken = await generateVerificationToken();
            account.passwordhash = BCrypt.Net.BCrypt.HashPassword(model.passwordhash);

            await _repositoryWrapper.user.Create(account);
            await _repositoryWrapper.Save();
        }

        private async Task<user> getAccountByResetToken(string token)
        {
            var account = (await _repositoryWrapper.user.FindByCondition(x=>
                x.ResetToken == token && x.ResetTokenExpires > DateTime.UtcNow)).SingleOrDefault();
            if (account == null) throw new AppException("Invalid token");
            return account;
        }


        public async Task ResetPassword(ResetPasswordRequest model)
        {
            var account = await getAccountByResetToken(model.Token);

            //update password and remove reser token
            account.passwordhash = BCrypt.Net.BCrypt.HashPassword(model.passwordhash);
            account.PasswordReset = DateTime.UtcNow;
            account.ResetToken = null;
            account.ResetTokenExpires = null;

            await _repositoryWrapper.user.Update(account);
            await _repositoryWrapper.Save();
        }

        public async Task RevokeToken(string token, string ipAddress)
        {
            var account = await getAccountByRefreshToken(token);
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");

            //revore token and save
            revokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
            await _repositoryWrapper.user.Update(account);
            await _repositoryWrapper.Save();
        }

        private async Task<user> getAccount(int id)
        {
            var account = (await _repositoryWrapper.user.FindByCondition(x => x.userid == id)).FirstOrDefault();
            if (account == null) throw new KeyNotFoundException("Account not found");
            return account;
        }

        public async Task<AccountResponse> Update(int id, UpdateRequest model)
        {
            var account = await getAccount(id);

            //validate
            if (account.email != model.email && (await _repositoryWrapper.user.FindByCondition(x => x.email == model.email)).Count > 0)
                throw new AppException($"Email '{model.email}' is already registered");

            //hash password if it was entered
            if (!string.IsNullOrEmpty(model.passwordhash))
                account.passwordhash = BCrypt.Net.BCrypt.HashPassword(model.passwordhash);

            //copy model to account and save
            _mapper.Map(model, account);
            account.Updated = DateTime.UtcNow;

            await _repositoryWrapper.user.Update(account);
            await _repositoryWrapper.Save();


            return _mapper.Map<AccountResponse>(account);
        }

        public async Task ValidateResetToken(ValidateResetTokenRequest model)
        {
            await getAccountByResetToken(model.Token);
        }

        public async Task VerifyEmail(string token)
        {
            var account = (await _repositoryWrapper.user.FindByCondition(x => x.VerificationToken == token)).FirstOrDefault();

            if (account == null)
                throw new AppException("Verification failed");

            account.Verifaied = DateTime.UtcNow;
            account.VerificationToken = null;

            await _repositoryWrapper.user.Update(account);
            await _repositoryWrapper.Save();
        }
    }
}

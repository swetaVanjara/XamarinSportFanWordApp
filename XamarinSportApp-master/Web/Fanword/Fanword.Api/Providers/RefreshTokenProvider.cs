using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using Fanword.Data.Context;
using Fanword.Data.IdentityConfig.RefreshTokens;
using Fanword.Data.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.Infrastructure;

namespace Fanword.Api.Providers
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshToken = Guid.NewGuid().ToString("n");
            var entities = new ApplicationDbContext();

            var repo = new ApplicationRepository(entities);

            var rToken = new RefreshToken()
            {
                DateCreatedUtc = DateTime.UtcNow,
                IssuedUtc = DateTime.UtcNow,
                ExpirationDateUtc = DateTime.UtcNow.AddYears(1),
                IsActive = true,
                Id = GetHash(refreshToken),
                ApplicationUserId = context.Ticket.Identity.GetUserId(),
            };
            context.Ticket.Properties.IssuedUtc = rToken.DateCreatedUtc;
            context.Ticket.Properties.ExpiresUtc = rToken.ExpirationDateUtc;
            //rToken.ProtectedTicket = context.SerializeTicket();
            TicketSerializer serializer = new TicketSerializer();

            rToken.ProtectedTicket = System.Text.Encoding.Default.GetString(serializer.Serialize(context.Ticket));
            await repo.RefreshTokens.AddOrUpdateAndSaveAsync(rToken);
            context.SetToken(refreshToken);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var entities = new ApplicationDbContext();
            var repo = new ApplicationRepository(entities);
            var hashedToken = GetHash(context.Token);
            var refreshToken = await repo.RefreshTokens.GetByIdAsync(hashedToken);
            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                //context.DeserializeTicket(refreshToken.ProtectedTicket);
                TicketSerializer serializer = new TicketSerializer();
                context.SetTicket(serializer.Deserialize(System.Text.Encoding.Default.GetBytes(refreshToken.ProtectedTicket)));
                //await repo.RefreshTokens.DeleteAndSaveAsync(hashedToken);
            }
        }

        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

    }
}
using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
namespace Fanword.Data.IdentityConfig.Services {
    public class SmsService : IIdentityMessageService {
        public Task SendAsync(IdentityMessage message){
            throw new NotImplementedException();
        }
    }
}

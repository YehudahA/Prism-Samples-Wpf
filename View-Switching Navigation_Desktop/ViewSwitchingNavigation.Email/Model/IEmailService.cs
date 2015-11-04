

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViewSwitchingNavigation.Email.Model
{
    public interface IEmailService
    {
        Task<IEnumerable<EmailDocument>> GetEmailDocumentsAsync();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true is sent, false if exception occoured</returns>
        Task<bool> SendEmailDocumentAsync(EmailDocument email);

        EmailDocument GetEmailDocument(Guid id);
    }
}

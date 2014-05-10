using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Session
{
    public interface ISessionService
    {
        Session Create();
        Session Update(Session session);

        Session Get();
    }
}

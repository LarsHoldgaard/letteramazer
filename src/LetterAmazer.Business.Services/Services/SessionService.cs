using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LetterAmazer.Business.Services.Domain.Session;

namespace LetterAmazer.Business.Services.Services
{
    public class SessionService:ISessionService
    {
        public Session Create()
        {
            var session = new Session();
            session.Id = Guid.NewGuid();

            StoreSession(session);

            return session;
        }

        public Session Update(Session session)
        {
            StoreSession(session);
            return session;
        }

        public Session Get()
        {
            return HttpContext.Current.Session["session"] as Session;
        }

        private void StoreSession(Session session)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session["session"] = session;
            }
        }
    }
}

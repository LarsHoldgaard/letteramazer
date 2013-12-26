using System.Data.Entity;

namespace LetterAmazer.Business.Services.Data
{
    public class DBFactory : Disposable, IDBFactory
    {
        private LetterAmazerContext dataContext;
        public DBFactory()
        {
            Database.SetInitializer<LetterAmazerContext>(null);
        }

        public LetterAmazerContext Get()
        {
            return dataContext ?? (dataContext = new LetterAmazerContext());
        }

        protected override void DisposeCore()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
            }
        }
    }
}

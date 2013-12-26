
namespace LetterAmazer.Business.Services.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDBFactory databaseFactory;
        private LetterAmazerContext dataContext;

        public UnitOfWork(IDBFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
            this.dataContext = this.databaseFactory.Get();
        }

        public void Commit()
        {
            this.dataContext.SaveChanges();
        }
    }
}

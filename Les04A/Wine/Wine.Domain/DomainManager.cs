using Wine.Domain.Model;
using Wine.Domain.Repository;

namespace Wine.Domain
{
    public class DomainManager
    {
        private readonly IWineRepository _repository;
        public DomainManager(IWineRepository repository)
        {
            _repository = repository;
        }

        public int GetTotalSaleCount()
        {
            List<Bottle> soldBottles = _repository.GetAllSoldBottles();
            return soldBottles.Count;
        }
    }
}

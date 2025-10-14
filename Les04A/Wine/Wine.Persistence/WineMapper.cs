using Wine.Domain.Model;
using Wine.Domain.Repository;

namespace Wine.Persistence
{
    public class WineMapper : IWineRepository
    {
        private List<Bottle> _bottles = [new("ne wijn", 1992), new("nen andere wijn", 1997)];
        public List<Bottle> GetAllSoldBottles()
        {
            return _bottles;
        }
    }
}

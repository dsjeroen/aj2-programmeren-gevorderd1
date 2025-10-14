using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wine.Domain.Model;

namespace Wine.Domain.Repository
{
    public interface IWineRepository
    {
        public List<Bottle> GetAllSoldBottles();
    }
}

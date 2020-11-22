using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSpawn.DAO
{
   public interface IConnectionFactory<T>
    {
        void Save(string val);
        void DeleteById(int id);
        void Update(T obj);
        T GetById(int id);
        IEnumerable<T> RetrieveList(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IRepository<T>
    {
        public List<T> List(string? keyword);

        public T? Get(string id);

        public void Save(T entity);

        public void Delete(string id);
    }
}

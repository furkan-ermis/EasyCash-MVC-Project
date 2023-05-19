using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCashIdentityProject.BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    {
        void TDelete(T t);
        void TUpdate(T t);
        void TInsert(T t);
        T TGetByID(int id);
        List<T> TGetList();
    }
}

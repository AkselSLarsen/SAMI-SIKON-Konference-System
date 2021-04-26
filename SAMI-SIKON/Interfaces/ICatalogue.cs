using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Interfaces {
    public interface ICatalogue<T> {

        void Create(T t);

        T Read(int i);

        void Update(T pre, T post);

        T Delete(int i);

        List<T> ReadAll();

    }
}

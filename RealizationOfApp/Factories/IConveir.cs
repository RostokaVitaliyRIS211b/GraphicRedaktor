using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp.Factories
{
    public interface IConveir<in T>
    {
        public void ProcessObj(T obj);
    }
}

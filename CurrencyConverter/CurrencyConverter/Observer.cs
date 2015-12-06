using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    public abstract class Observer
    {
        public Observer()
        {

        }

        abstract public void update(Object data);
    }
}

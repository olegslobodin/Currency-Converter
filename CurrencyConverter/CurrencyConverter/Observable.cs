using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    public class Observable
    {
        public Observable()
        {
            observers = new List<Observer>();
        }

        List<Observer> observers;

        public void addObserver(Observer o)
        {
            observers.Add(o);
        }

        public void removeObserver(Observer o)
        {
            observers.Remove(o);
        }

        protected void notifyObservers(Object data)
        {
            foreach (Observer o in observers)
            {
                o.update(data);
            }
        }
    }
}

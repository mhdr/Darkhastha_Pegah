using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Darkhast.Lib
{
    class Darkhastha2Collecttion : ObservableCollection<Darkhastha2>
    {
        public Darkhastha2Collecttion(IQueryable<Darkhastha> darkhastha)
        {
            foreach (var d in darkhastha)
            {
                Darkhastha2 newDarkhast = new Darkhastha2(d);
                Add(newDarkhast);
            }
        }

        public Darkhastha2Collecttion(ParallelQuery<Darkhastha> darkhastha)
        {
            foreach (var d in darkhastha)
            {
                Darkhastha2 newDarkhast = new Darkhastha2(d);
                Add(newDarkhast);
            }
        }
    }
}

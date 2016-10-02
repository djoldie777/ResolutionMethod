using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace resolution_method
{
    public class Disjunct
    {
        private Disjunct firstPrev, secondPrev;
        public List<string> list;

        public Disjunct()
        {
            firstPrev = null;
            secondPrev = null;
            list = new List<string>();
        }

        public Disjunct(Disjunct firstPrev, Disjunct secondPrev, List<string> list)
        {
            this.firstPrev = firstPrev;
            this.secondPrev = secondPrev;
            this.list = new List<string>(list);
        }

        public Disjunct FirstPrev
        {
            get { return firstPrev; }
        }

        public Disjunct SecondPrev
        {
            get { return secondPrev; }
        }

        public override string ToString()
        {
            string str = "";

            for (int i = 0; i < list.Count; i++)
                if (i != list.Count - 1)
                    str += list[i] + " or ";
                else
                    str += list[i];

            return str;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace resolution_method
{
    public class CompareDisjunct : IEqualityComparer<Disjunct>
    {
        public bool Equals(Disjunct d1, Disjunct d2)
        {
            if (d1.list.Count != d2.list.Count)
                return false;

            for (int i = 0; i < d1.list.Count; i++)
            {
                if (d1.list[i] != d2.list[i])
                    return false;
            }

            return true;
        }

        public int GetHashCode(Disjunct d)
        {
            int sum = 0;
            int prod = 1;

            for (int i = 0; i < d.list.Count; i++)
            {
                sum += d.list[i].GetHashCode();
                prod *= d.list[i].GetHashCode();
            }

            return prod ^ sum;
        }
    }

    class Resolution
    {
        public static bool CanUseResolution(Disjunct d1, Disjunct d2)
        {
            for (int i = 0; i < d1.list.Count; i++)
                if (d2.list.Contains("-" + d1.list[i]) || d2.list.Contains(d1.list[i].Replace("-", "")) && d1.list[i].Contains("-"))
                    return true;

            return false;
        }

        public static List<Disjunct> GetResolvent(Disjunct d1, Disjunct d2)
        {
            List<Disjunct> res = new List<Disjunct>();
            List<string> list = new List<string>();

            for (int i = 0; i < d1.list.Count; i++)
            {
                if (d2.list.Contains("-" + d1.list[i]))
                {
                    if (d2.list.Count == 1 && d1.list.Count == 1)
                    {
                        res.Add(new Disjunct(d1, d2, list));
                        return res;
                    }
                    List<string> l1 = new List<string>(d1.list);
                    List<string> l2 = new List<string>(d2.list);
                    l1.Remove(d1.list[i]);
                    l2.Remove("-" + d1.list[i]);
                    l1.AddRange(l2);
                    list = l1.Distinct().ToList();
                    res.Add(new Disjunct(d1, d2, list));
                }
                else if (d1.list[i].Contains("-") && d2.list.Contains(d1.list[i].Replace("-", "")))
                {
                    if (d2.list.Count == 1 && d1.list.Count == 1)
                    {
                        res.Add(new Disjunct(d1, d2, list));
                        return res;
                    }
                    List<string> l1 = new List<string>(d1.list);
                    List<string> l2 = new List<string>(d2.list);
                    l1.Remove(d1.list[i]);
                    l2.Remove(d1.list[i].Replace("-", ""));
                    l1.AddRange(l2);
                    list = l1.Distinct().ToList();
                    res.Add(new Disjunct(d1, d2, list));
                }
            }

            return res;
        }

        public static Tuple<List<Disjunct>, bool> DoResolution(List<List<string>> list)
        {
            List<Disjunct> res = new List<Disjunct>();

            for (int i = 0; i < list.Count; i++)
                res.Add(new Disjunct(null, null, list[i]));

            while (true)
            {
                List<Disjunct> resolvents;
                int countOfResolvents = 0;

                for (int i = 0; i < res.Count; i++)
                {
                    for (int j = i + 1; j < res.Count; j++)
                    {
                        if (CanUseResolution(res[i], res[j]))
                        {
                            resolvents = GetResolvent(res[i], res[j]);

                            for (int k = 0; k < resolvents.Count; k++)
                            {
                                if (!res.Contains(resolvents[k], new CompareDisjunct()))
                                {
                                    res.Add(resolvents[k]);
                                    countOfResolvents++;
                                }

                                if (resolvents[k].list.Count == 0)
                                    return new Tuple<List<Disjunct>, bool>(res, true); 
                            }
                        }
                    }
                }

                if (countOfResolvents == 0)
                    return new Tuple<List<Disjunct>, bool>(res, false);
            }
        }
    }
}

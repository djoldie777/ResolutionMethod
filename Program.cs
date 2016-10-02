using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace resolution_method
{
    class Program
    {
        private static bool DigitChecking(string text)
        {
            int n = 0;

            for (int i = 0; i < text.Length; i++)
                if (Int32.TryParse(text[i].ToString(), out n))
                    return true;
            
            return false;
        }

        private static bool BracketChecking(string text)
        {
            Dictionary<char, char> types = new Dictionary<char, char>();

            types.Add(')', '(');
            types.Add(']', '[');
            types.Add('}', '{');

            List<char> openingBrackets = types.Values.ToList<char>();
            List<char> closingBrackets = types.Keys.ToList<char>();

            Stack<char> st = new Stack<char>();

            for (int i = 0; i < text.Length; i++)
            {
                if (openingBrackets.Contains(text[i]))
                {
                    st.Push(text[i]);
                }
                if (closingBrackets.Contains(text[i]))
                {
                    if (st.Count == 0)
                        return false;
                    char tmp = st.Pop();
                    if (!tmp.Equals(types[text[i]]))
                        return false;
                }
            }

            if (st.Count != 0)
                return false;
            else
                return true;
        }

        private static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);

            if (pos < 0)
            {
                return text;
            }

            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        private static Expression ExpressionBuilding(string str)
        {
            List<Expression> exprList = new List<Expression>();
            int openBracketIndex = -1;
            int closeBracketIndex = -1;
            int digit = 0;

            for (int i = 0; i < str.Length; i++)
                if (str[i] == '(')
                    openBracketIndex = i;

            while (openBracketIndex != -1)
            {
                for (int i = openBracketIndex + 1; i < str.Length; i++)
                    if (str[i] == ')')
                    {
                        closeBracketIndex = i;
                        break;
                    }

                int length = closeBracketIndex - openBracketIndex + 1;
                string subStr = str.Substring(openBracketIndex, length);

                subStr = subStr.Replace("(", "").Replace(")", "");

                string oper = "";
                Literal firstLit = null;
                Literal secondLit = null;
                Expression firstExp = null;
                Expression secondExp = null;

                string[] splitters = { " " };
                string[] s = subStr.Split(splitters, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < s.Length; i++)
                {
                    if ((s[i] == Const.And) || (s[i] == Const.Or) || (s[i] == Const.Impl) || (s[i] == Const.Eq))
                    {
                        oper = s[i];
                        continue;
                    }

                    if (oper == "")
                    {
                        if (Int32.TryParse(s[i], out digit))
                        {
                            firstExp = new Expression(exprList[digit]);
                        }
                        else
                        {
                            if (s[i].Length == 1)
                                firstLit = new Literal(s[i], false);
                            else
                                firstLit = new Literal(s[i], true);
                        }
                    }
                    else
                    {
                        if (Int32.TryParse(s[i], out digit))
                        {
                            secondExp = new Expression(exprList[digit]);
                        }
                        else
                        {
                            if (s[i].Length == 1)
                                secondLit = new Literal(s[i], false);
                            else
                                secondLit = new Literal(s[i], true);
                        }
                    }
                }

                if ((firstLit != null) && (secondLit != null))
                {
                    exprList.Add(new Expression(firstLit, secondLit, oper));
                    firstLit = secondLit = null;
                }
                else if ((firstLit != null) && (secondExp != null))
                {
                    exprList.Add(new Expression(firstLit, secondExp, oper));
                    firstLit = null;
                    secondExp = null;
                }
                else if ((firstExp != null) && (secondLit != null))
                {
                    exprList.Add(new Expression(firstExp, secondLit, oper));
                    firstExp = null;
                    secondLit = null;
                }
                else
                {
                    exprList.Add(new Expression(firstExp, secondExp, oper));
                    firstExp = secondExp = null;
                }

                int index = exprList.Count - 1;

                str = ReplaceFirst(str, "(" + subStr + ")", index.ToString());

                if (str.Length == 1)
                    return new Expression(exprList[index]);

                for (int i = 0; i < str.Length; i++)
                    if (str[i] == '(')
                        openBracketIndex = i;
            }

            return null;
        }

        private static Literal LiteralBuilding(string str)
        {
            str = str.Replace("(", "").Replace(")", "");

            if (str.Length == 1)
                return new Literal(str, false);
            else
            {
                str = str.Remove(0, 1);
                return new Literal(str, true);
            }
        }

        private static void PrintWithIndent(string str, int indent)
        {
            Console.WriteLine("{0}{1}", new string(' ', indent * 2), str);
        }

        private static void Output(Disjunct d, int indent)
        {
            PrintWithIndent(d.ToString(), indent);

            if (d.FirstPrev != null)
            {
                Output(d.FirstPrev, indent + 1);
                Output(d.SecondPrev, indent + 1);
            }
        }

        static void Main(string[] args)
        {
            // F1 = x -> (y or z); F2 = z -> w; F3 = -w; F = x -> y
            Expression res = null;
            List<string> axioms = new List<string>();
            int axiomCount = 0;
            int num = 0;

            Literal l = null;
            Literal fl = null;
            Expression e = null;
            Expression fe = null;
            
            Console.WriteLine("Input count of axioms:");

            while (!Int32.TryParse(Console.ReadLine(), out axiomCount))
                Console.WriteLine("\nYou should input correct number. Try again:");
            Console.WriteLine();

            for (int i = 0; i < axiomCount; i++)
            {
                Console.WriteLine("Input " + (i + 1) + " axiom:");
                string s = Console.ReadLine();

                while (DigitChecking(s) || (!BracketChecking(s)))
                {
                    if (DigitChecking(s))
                        Console.WriteLine("\nThere is no way for axiom to contain numbers. Try again:");
                    else
                        Console.WriteLine("\nYou made a mistake with brackets. Try again:");

                    s = Console.ReadLine();
                }

                s = "(" + s + ")";
                axioms.Add(s);
                Console.WriteLine();
            }

            Console.WriteLine("Input number of axiom you'd like to prove:");

            string proveNum = Console.ReadLine();

            while ((!Int32.TryParse(proveNum, out num)) || (num < 1) || (num > axiomCount))
            {
                if (!Int32.TryParse(proveNum, out num))
                    Console.WriteLine("\nYou should input correct number. Try again:");
                else
                    Console.WriteLine("\nYour number is out of range. You should choose one from 1 to " + axiomCount + ". Try again:");

                proveNum = Console.ReadLine();
            }
            Console.WriteLine();

            for (int i = 0; i < axioms.Count; i++)
            {
                if (i == num - 1)
                {
                    if (axioms[i].Length <= 4)
                    {
                        l = LiteralBuilding(axioms[i]);
                        l.NegateLiteral();
                    }
                    else
                    {
                        e = ExpressionBuilding(axioms[i]);
                        e.NegateExpr();
                    }
                }
                else
                {
                    if (axioms[i].Length <= 4)
                        l = LiteralBuilding(axioms[i]);
                    else
                        e = ExpressionBuilding(axioms[i]);
                }

                if (i == 0)
                {
                    if (l != null)
                        fl = new Literal(l.Value, l.Negation);
                    else
                        fe = new Expression(e);
                }
                else if (i == 1)
                {
                    if ((fl != null) && (l != null))
                    {
                        res = new Expression(fl, l, Const.And);
                        fl = l = null;
                    }
                    else if ((fe != null) && (l != null))
                    {
                        res = new Expression(fe, l, Const.And);
                        fe = null;
                        l = null;
                    }
                    else if ((fl != null) && (e != null))
                    {
                        res = new Expression(fl, e, Const.And);
                        fl = null;
                        e = null;
                    }
                    else
                    {
                        res = new Expression(fe, e, Const.And);
                        fe = e = null;
                    }
                }
                else
                {
                    if (l != null)
                    {
                        res = new Expression(res, l, Const.And);
                        l = null;
                    }
                    else
                    {
                        res = new Expression(res, e, Const.And);
                        e = null;
                    }
                }
            }

            Console.WriteLine("Expression : " + res.ToString() + "\n");

            CNF cnf = new CNF(res);

            Tuple<List<List<string>>, string> tuple = cnf.Conversion(res);

            Console.WriteLine("CNF : " + tuple.Item2 + "\n");

            List<List<string>> list = tuple.Item1;

            Tuple<List<Disjunct>, bool> result = Resolution.DoResolution(list);

            if (result.Item2)
            {
                Console.WriteLine("Solution :");

                Disjunct lastDisjunct = result.Item1[result.Item1.Count - 1];

                Output(lastDisjunct, 0);

                Console.WriteLine();
                Console.WriteLine("Conclusion : The set of disjuncts is contradictory.\n");
            }
            else
                Console.WriteLine("Conclusion : The set of disjuncts is not contradictory.\n");
        }
    }
}

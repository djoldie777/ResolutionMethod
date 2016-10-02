using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace resolution_method
{
    public class CompareList : IEqualityComparer<List<string>>
    {
        public bool Equals(List<string> l1, List<string> l2)
        {
            if (l1.Count != l2.Count)
                return false;

            for (int i = 0; i < l1.Count; i++)
            {
                if (l1[i] != l2[i])
                    return false;
            }

            return true;
        }

        public int GetHashCode(List<string> l)
        {
            int sum = 0;
            int prod = 1;

            for (int i = 0; i < l.Count; i++)
            {
                sum += l[i].GetHashCode();
                prod *= l[i].GetHashCode();
            }

            return prod ^ sum;
        }
    }

    class CNF
    {
        public CNF(Expression expression)
        {
            RemoveEquivalence(expression);
            RemoveImplication(expression);
            RemoveWithAbsorption(expression);
        }

        private void RemoveEquivalence(Expression expression)
        {
            if (expression.FirstPart != null)
                RemoveEquivalence(expression.FirstPart);

            if (expression.SecondPart != null)
                RemoveEquivalence(expression.SecondPart);

            if (expression.Operation == Const.Eq)
            {
                Expression conjunct1, conjunct2;
                if (expression.FirstLiteral != null && expression.SecondLiteral != null)
                {
                    conjunct1 = new Expression(expression.FirstLiteral, expression.SecondLiteral, Const.Impl);
                    conjunct2 = new Expression(expression.SecondLiteral, expression.FirstLiteral, Const.Impl);
                }
                else if (expression.FirstPart != null && expression.SecondLiteral != null)
                {
                    conjunct1 = new Expression(expression.FirstPart, expression.SecondLiteral, Const.Impl);
                    conjunct2 = new Expression(expression.SecondLiteral, expression.FirstPart, Const.Impl);
                }
                else if (expression.FirstLiteral != null && expression.SecondPart != null)
                {
                    conjunct1 = new Expression(expression.FirstLiteral, expression.SecondPart, Const.Impl);
                    conjunct2 = new Expression(expression.SecondPart, expression.FirstLiteral, Const.Impl);
                }
                else
                {
                    conjunct1 = new Expression(expression.FirstPart, expression.SecondPart, Const.Impl);
                    conjunct2 = new Expression(expression.SecondPart, expression.FirstPart, Const.Impl);
                }

                if (expression.FirstLiteral != null)
                    expression.FirstLiteral = null;
                expression.FirstPart = conjunct1;

                if (expression.SecondLiteral != null)
                    expression.SecondLiteral = null;
                expression.SecondPart = conjunct2;

                expression.Operation = Const.And;
            }
        }

        private void RemoveImplication(Expression expression)
        {
            if (expression.FirstPart != null)
                RemoveImplication(expression.FirstPart);

            if (expression.SecondPart != null)
                RemoveImplication(expression.SecondPart);

            if (expression.Operation == Const.Impl)
            {
                if (expression.FirstLiteral != null)
                    expression.FirstLiteral.NegateLiteral();

                if (expression.FirstPart != null)
                    expression.FirstPart.NegateExpr();

                expression.Operation = Const.Or;
            }
        }
        
        private void RemoveWithAbsorption(Expression expression)
        {
            if (expression.FirstPart != null)
                RemoveWithAbsorption(expression.FirstPart);
            
            if (expression.SecondPart != null)
                RemoveWithAbsorption(expression.SecondPart);

            if (expression.FirstPart != null && expression.SecondPart != null)
            {
                if ((expression.Operation == Const.Or) && (expression.FirstPart.Operation == Const.And) && (expression.SecondPart.Operation == Const.And))
                {
                    Expression conjunct1;
                    if (expression.FirstPart.FirstLiteral != null && expression.SecondPart.FirstLiteral != null)
                        conjunct1 = new Expression(expression.FirstPart.FirstLiteral, expression.SecondPart.FirstLiteral, Const.Or);
                    else if (expression.FirstPart.FirstPart != null && expression.SecondPart.FirstLiteral != null)
                        conjunct1 = new Expression(expression.FirstPart.FirstPart, expression.SecondPart.FirstLiteral, Const.Or);
                    else if (expression.FirstPart.FirstLiteral != null && expression.SecondPart.FirstPart != null)
                        conjunct1 = new Expression(expression.FirstPart.FirstLiteral, expression.SecondPart.FirstPart, Const.Or);
                    else
                        conjunct1 = new Expression(expression.FirstPart.FirstPart, expression.SecondPart.FirstPart, Const.Or);
                    RemoveWithAbsorption(conjunct1);

                    Expression conjunct2;
                    if (expression.FirstPart.FirstLiteral != null && expression.SecondPart.SecondLiteral != null)
                        conjunct2 = new Expression(expression.FirstPart.FirstLiteral, expression.SecondPart.SecondLiteral, Const.Or);
                    else if (expression.FirstPart.FirstPart != null && expression.SecondPart.SecondLiteral != null)
                        conjunct2 = new Expression(expression.FirstPart.FirstPart, expression.SecondPart.SecondLiteral, Const.Or);
                    else if (expression.FirstPart.FirstLiteral != null && expression.SecondPart.SecondPart != null)
                        conjunct2 = new Expression(expression.FirstPart.FirstLiteral, expression.SecondPart.SecondPart, Const.Or);
                    else
                        conjunct2 = new Expression(expression.FirstPart.FirstPart, expression.SecondPart.SecondPart, Const.Or);
                    RemoveWithAbsorption(conjunct2);

                    Expression conjunct3;
                    if (expression.FirstPart.SecondLiteral != null && expression.SecondPart.FirstLiteral != null)
                        conjunct3 = new Expression(expression.FirstPart.SecondLiteral, expression.SecondPart.FirstLiteral, Const.Or);
                    else if (expression.FirstPart.SecondPart != null && expression.SecondPart.FirstLiteral != null)
                        conjunct3 = new Expression(expression.FirstPart.SecondPart, expression.SecondPart.FirstLiteral, Const.Or);
                    else if (expression.FirstPart.SecondLiteral != null && expression.SecondPart.FirstPart != null)
                        conjunct3 = new Expression(expression.FirstPart.SecondLiteral, expression.SecondPart.FirstPart, Const.Or);
                    else
                        conjunct3 = new Expression(expression.FirstPart.SecondPart, expression.SecondPart.FirstPart, Const.Or);
                    RemoveWithAbsorption(conjunct3);

                    Expression conjunct4;
                    if (expression.FirstPart.SecondLiteral != null && expression.SecondPart.SecondLiteral != null)
                        conjunct4 = new Expression(expression.FirstPart.SecondLiteral, expression.SecondPart.SecondLiteral, Const.Or);
                    else if (expression.FirstPart.SecondPart != null && expression.SecondPart.SecondLiteral != null)
                        conjunct4 = new Expression(expression.FirstPart.SecondPart, expression.SecondPart.SecondLiteral, Const.Or);
                    else if (expression.FirstPart.SecondLiteral != null && expression.SecondPart.SecondPart != null)
                        conjunct4 = new Expression(expression.FirstPart.SecondLiteral, expression.SecondPart.SecondPart, Const.Or);
                    else
                        conjunct4 = new Expression(expression.FirstPart.SecondPart, expression.SecondPart.SecondPart, Const.Or);
                    RemoveWithAbsorption(conjunct4);

                    if (expression.FirstPart.FirstLiteral != null)
                        expression.FirstPart.FirstLiteral = null;
                    expression.FirstPart.FirstPart = conjunct1;

                    if (expression.FirstPart.SecondLiteral != null)
                        expression.FirstPart.SecondLiteral = null;
                    expression.FirstPart.SecondPart = conjunct2;

                    if (expression.SecondPart.FirstLiteral != null)
                        expression.SecondPart.FirstLiteral = null;
                    expression.SecondPart.FirstPart = conjunct3;

                    if (expression.SecondPart.SecondLiteral != null)
                        expression.SecondPart.SecondLiteral = null;
                    expression.SecondPart.SecondPart = conjunct4;

                    expression.Operation = Const.And;
                }
                // (_ and _) or (_ or _)
                else if ((expression.Operation == Const.Or) && (expression.FirstPart.Operation == Const.And) && (expression.SecondPart.Operation == Const.Or))
                {
                    Expression conjunct1;
                    if (expression.FirstPart.FirstLiteral != null)
                        conjunct1 = new Expression(expression.FirstPart.FirstLiteral, expression.SecondPart, Const.Or);
                    else
                        conjunct1 = new Expression(expression.FirstPart.FirstPart, expression.SecondPart, Const.Or);
                    RemoveWithAbsorption(conjunct1);

                    Expression conjunct2;
                    if (expression.FirstPart.SecondLiteral != null)
                        conjunct2 = new Expression(expression.FirstPart.SecondLiteral, expression.SecondPart, Const.Or);
                    else
                        conjunct2 = new Expression(expression.FirstPart.SecondPart, expression.SecondPart, Const.Or);
                    RemoveWithAbsorption(conjunct2);

                    expression.FirstPart = conjunct1;
                    expression.SecondPart = conjunct2;

                    expression.Operation = Const.And;
                }
                // (_ or _) or (_ and _)
                else if ((expression.Operation == Const.Or) && (expression.FirstPart.Operation == Const.Or) && (expression.SecondPart.Operation == Const.And))
                {
                    Expression conjunct1;
                    if (expression.SecondPart.FirstLiteral != null)
                        conjunct1 = new Expression(expression.FirstPart, expression.SecondPart.FirstLiteral, Const.Or);
                    else
                        conjunct1 = new Expression(expression.FirstPart, expression.SecondPart.FirstPart, Const.Or);
                    RemoveWithAbsorption(conjunct1);

                    Expression conjunct2;
                    if (expression.SecondPart.SecondLiteral != null)
                        conjunct2 = new Expression(expression.FirstPart, expression.SecondPart.SecondLiteral, Const.Or);
                    else
                        conjunct2 = new Expression(expression.FirstPart, expression.SecondPart.SecondPart, Const.Or);
                    RemoveWithAbsorption(conjunct2);

                    expression.FirstPart = conjunct1;
                    expression.SecondPart = conjunct2;

                    expression.Operation = Const.And;
                }
            }
            else if (expression.FirstPart != null)
            {
                if (expression.Operation == Const.Or && expression.FirstPart.Operation == Const.And)
                {
                    Expression conjunct1;
                    if (expression.FirstPart.FirstLiteral != null)
                        conjunct1 = new Expression(expression.FirstPart.FirstLiteral, expression.SecondLiteral, Const.Or);
                    else
                        conjunct1 = new Expression(expression.FirstPart.FirstPart, expression.SecondLiteral, Const.Or);
                    RemoveWithAbsorption(conjunct1);
                    
                    Expression conjunct2;
                    if (expression.FirstPart.SecondLiteral != null)
                        conjunct2 = new Expression(expression.FirstPart.SecondLiteral, expression.SecondLiteral, Const.Or);
                    else
                        conjunct2 = new Expression(expression.FirstPart.SecondPart, expression.SecondLiteral, Const.Or);
                    RemoveWithAbsorption(conjunct2);

                    expression.FirstPart = conjunct1;

                    if (expression.SecondLiteral != null)
                        expression.SecondLiteral = null;
                    expression.SecondPart = conjunct2;

                    expression.Operation = Const.And;
                }
            }
            else if (expression.SecondPart != null)
            {
                if (expression.Operation == Const.Or && expression.SecondPart.Operation == Const.And)
                {
                    Expression conjunct1;
                    if (expression.SecondPart.FirstLiteral != null)
                        conjunct1 = new Expression(expression.FirstLiteral, expression.SecondPart.FirstLiteral, Const.Or);
                    else
                        conjunct1 = new Expression(expression.FirstLiteral, expression.SecondPart.FirstPart, Const.Or);
                    RemoveWithAbsorption(conjunct1);

                    Expression conjunct2;
                    if (expression.SecondPart.SecondLiteral != null)
                        conjunct2 = new Expression(expression.FirstLiteral, expression.SecondPart.SecondLiteral, Const.Or);
                    else
                        conjunct2 = new Expression(expression.FirstLiteral, expression.SecondPart.SecondPart, Const.Or);
                    RemoveWithAbsorption(conjunct2);

                    if (expression.FirstLiteral != null)
                        expression.FirstLiteral = null;
                    expression.FirstPart = conjunct1;

                    expression.SecondPart = conjunct2;

                    expression.Operation = Const.And;
                }
            }
        }

        public Tuple<List<List<string>>, string> Conversion(Expression expression)
        {
            List<List<string>> res = new List<List<string>>();
            string str = "";

            string[] splitters1 = { "and" };
            string[] splitters2 = { "or" };

            string[] s = expression.ToString().Split(splitters1, StringSplitOptions.RemoveEmptyEntries);

            int countOfUniqueSElems = s.Length;

            for (int i = 0; i < s.Length; i++)
            {
                List<string> l = new List<string>();
                string tmpStr = "";

                s[i] = s[i].Replace("(", "").Replace(")", "").Replace(" ", "");

                List<string> list = s[i].Split(splitters2, StringSplitOptions.RemoveEmptyEntries).ToList();

                int countOfUniqueListElems = list.Count;

                for (int j = 0; j < list.Count; j++)
                {
                    if (list.Contains("-" + list[j]) || list[j].Contains("-") && (list.Contains(list[j].Replace("-", ""))))
                        break;

                    if (l.Contains(list[j]))
                    {
                        countOfUniqueListElems--;
                        continue;
                    }

                    l.Add(list[j]);

                    if (j < countOfUniqueListElems - 1)
                        if (j == 0)
                            tmpStr += "(" + list[j] + " or ";
                        else
                            tmpStr += list[j] + " or ";
                    else
                        if (countOfUniqueListElems == 1)
                            tmpStr += "(" + list[j] + ")";
                        else
                            tmpStr += list[j] + ")";
                }

                if (l.Count != 0)
                {
                    if (res.Contains(l, new CompareList()))
                    {
                        countOfUniqueSElems--;
                        continue;
                    }

                    res.Add(l);

                    if (i < countOfUniqueSElems - 1)
                        str += tmpStr + " and ";
                    else
                        str += tmpStr;
                }
            }

            return new Tuple<List<List<string>>,string>(res, str);
        }
    }
}

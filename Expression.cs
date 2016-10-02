using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace resolution_method
{
    class Expression
    {
        private Literal firstLiteral;
        private Literal secondLiteral;

        private Expression firstPart;
        private Expression secondPart;

        private string operation;

        public Expression(Literal firstLiteral, Literal secondLiteral, string operation)
        {
            this.firstLiteral = new Literal(firstLiteral.Value, firstLiteral.Negation);
            this.secondLiteral = new Literal(secondLiteral.Value, secondLiteral.Negation);
            this.operation = operation;
        }

        public Expression(Expression expression)
        {
            if (expression.firstLiteral != null)
                firstLiteral = new Literal(expression.firstLiteral.Value, expression.firstLiteral.Negation);
            if (expression.secondLiteral != null)
                secondLiteral = new Literal(expression.secondLiteral.Value, expression.secondLiteral.Negation);
            if (expression.firstPart != null)
                firstPart = new Expression(expression.firstPart);
            if (expression.secondPart != null)
                secondPart = new Expression(expression.secondPart);
            operation = expression.operation;
        }

        public Expression(Expression firstPart, Literal secondLiteral, string operation)
        {
            this.firstPart = new Expression(firstPart);
            this.secondLiteral = new Literal(secondLiteral.Value, secondLiteral.Negation);
            this.operation = operation;
        }

        public Expression(Literal firstLiteral, Expression secondPart, string operation)
        {
            this.firstLiteral = new Literal(firstLiteral.Value, firstLiteral.Negation);
            this.secondPart = new Expression(secondPart);
            this.operation = operation;
        }

        public Expression(Expression firstPart, Expression secondPart, string operation)
        {
            this.firstPart = new Expression(firstPart);
            this.secondPart = new Expression(secondPart);
            this.operation = operation;
        }

        public Literal FirstLiteral
        {
            get { return firstLiteral; }
            set { firstLiteral = value; }
        }

        public Literal SecondLiteral
        {
            get { return secondLiteral; }
            set { secondLiteral = value; }
        }

        public Expression FirstPart
        {
            get { return firstPart; }
            set { firstPart = value; }
        }

        public Expression SecondPart
        {
            get { return secondPart; }
            set { secondPart = value; }
        }

        public string Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        public void NegateExpr()
        {
            switch (operation)
            {
                case (Const.And):
                    if (firstLiteral != null)
                        firstLiteral.NegateLiteral();
                    if (secondLiteral != null)
                        secondLiteral.NegateLiteral();
                    if (secondPart != null)
                        secondPart.NegateExpr();
                    if (firstPart != null)
                        firstPart.NegateExpr();
                    operation = Const.Or;
                    break;
                case (Const.Or):
                    if (firstLiteral != null)
                        firstLiteral.NegateLiteral();
                    if (secondLiteral != null)
                        secondLiteral.NegateLiteral();
                    if (firstPart != null)
                        firstPart.NegateExpr();
                    if (secondPart != null)
                        secondPart.NegateExpr();
                    operation = Const.And;
                    break;
                case (Const.Impl):
                    if (secondLiteral != null)
                        secondLiteral.NegateLiteral();
                    if (secondPart != null)
                        secondPart.NegateExpr();
                    operation = Const.And;
                    break;
                case (Const.Eq):
                    if (firstPart != null && secondPart != null)
                    {
                        Expression sp = new Expression(firstPart);
                        firstPart = new Expression(firstPart, secondPart, Const.Impl);
                        firstPart.NegateExpr();
                        secondPart = new Expression(secondPart, sp, Const.Impl);
                        secondPart.NegateExpr();
                    }
                    if (firstLiteral != null && secondLiteral != null)
                    {
                        firstPart = new Expression(firstLiteral, secondLiteral, Const.Impl);
                        firstPart.NegateExpr();
                        secondPart = new Expression(secondLiteral, firstLiteral, Const.Impl);
                        secondPart.NegateExpr();
                        firstLiteral = null;
                        secondLiteral = null;
                    }
                    if (firstLiteral != null && secondPart != null)
                    {
                        firstPart = new Expression(firstLiteral, secondPart, Const.Impl);
                        firstPart.NegateExpr();
                        secondPart = new Expression(secondPart, firstLiteral, Const.Impl);
                        secondPart.NegateExpr();
                        firstLiteral = null;
                    }
                    if (firstPart != null && secondLiteral != null)
                    {
                        secondPart = new Expression(secondLiteral, firstPart, Const.Impl);
                        secondPart.NegateExpr();
                        firstPart = new Expression(firstPart, secondLiteral, Const.Impl);
                        firstPart.NegateExpr();
                        secondLiteral = null;
                    }
                    operation = Const.Or;
                    break;
            }
        }

        public override string ToString()
        {
            return (firstLiteral == null ? "(" + firstPart.ToString() + ")" : firstLiteral.ToString()) + " " + operation
                + " " + (secondLiteral == null ? "(" + secondPart.ToString() + ")" : secondLiteral.ToString());  
        }
    }
}

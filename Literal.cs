using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace resolution_method
{
    class Literal
    {
        private string literal;
        private bool negation;

        public Literal()
        {
            literal = "";
            negation = false;
        }

        public Literal(string literal, bool negate)
        {
            this.literal = literal;
            this.negation = negate;
        }

        public string Value
        {
            get { return literal; }
        }

        public bool Negation
        {
            get { return negation; }
            set { negation = value; }
        }

        public void NegateLiteral()
        {
            negation = negation ? false : true;
        }

        public override string ToString()
        {
            return negation == true ? Const.Neg + literal : literal; 
        }
    }
}

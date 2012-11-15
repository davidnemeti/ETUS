using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace UnitSystemLanguage
{
    public class UnitSystemGrammar : Grammar
    {
        public UnitSystemGrammar()
        {
            var module = new NonTerminal("module");
            var namespace_usage = new NonTerminal("namespace usage");
            var namespace_usages = new NonTerminal("namespace usages");
            var @namespace = new NonTerminal("namespace");
            var definitions = new NonTerminal("definitions");
            var quantity_definition = new NonTerminal("quantity_definition");
            var unit_definition = new NonTerminal("unit_definition");

            var quantity_name = new IdentifierTerminal("quantity_name");
            var unit_name = new IdentifierTerminal("unit_name");

            var conversions = new NonTerminal("conversions");
            var conversion = new NonTerminal("conversion");
            var simple_conversion = new NonTerminal("simple_conversion");
            var complex_conversion = new NonTerminal("complex_conversion");
            var simple_conversion_op = new NonTerminal("simple_conversion_op");
            var complex_conversion_op = new NonTerminal("complex_conversion_op");
            var expression = new NonTerminal("expression");
            var binary_expression = new NonTerminal("binary_expression");
            var unary_expression = new NonTerminal("unary_expression");
            var expression_with_units = new NonTerminal("expression_with_units");
            var literal = new NumberLiteral("number");
            var constant = new ConstantTerminal("constant");
            var external_variable = new IdentifierTerminal("external_variable");
            var unit_variable = new NonTerminal("unit_variable");

            constant.Add("PI", Math.PI);
            constant.Add("π", Math.PI);

            var USE = ToTerm("use");
            var DEFINE = ToTerm("define");
            var NAMESPACE = ToTerm("namespace");
            var QUANTITY = ToTerm("quantity");
            var UNIT = ToTerm("unit");
            var OF = ToTerm("of");
            var SIMPLE_MUTUAL_CONVERSION_OP = ToTerm("<->");
            var SIMPLE_TO_THIS_CONVERSION_OP = ToTerm("<-");
            var SIMPLE_TO_THAT_CONVERSION_OP = ToTerm("->");
            var COMPLEX_MUTUAL_CONVERSION_OP = ToTerm("<=>");
            var COMPLEX_TO_THIS_CONVERSION_OP = ToTerm("<=");
            var COMPLEX_TO_THAT_CONVERSION_OP = ToTerm("=>");

            var ADD_OP = ToTerm("+");
            var SUB_OP = ToTerm("-");
            var NEG_OP = ToTerm("-");
            var MUL_OP = ToTerm("*");
            var DIV_OP = ToTerm("/");
            var POW_OP = ToTerm("^");
            var EQUAL_OP = ToTerm("=");

            var LEFT_PAREN = ToTerm("(");
            var RIGHT_PAREN = ToTerm(")");
            var LEFT_BRACKET = ToTerm("[");
            var RIGHT_BRACKET = ToTerm("]");

            RegisterOperators(10, Associativity.Right, EQUAL_OP);
            RegisterOperators(20, ADD_OP, SUB_OP);
            RegisterOperators(30, MUL_OP, DIV_OP);
            RegisterOperators(40, NEG_OP);
            RegisterOperators(50, Associativity.Right, POW_OP);

            RegisterBracePair(LEFT_PAREN.Text, RIGHT_PAREN.Text);
            RegisterBracePair(LEFT_BRACKET.Text, RIGHT_BRACKET.Text);

            this.Root = module;

            module.Rule = MakeStarRule(namespace_usages, namespace_usage) +
                @namespace +
                MakePlusRule(definitions, quantity_definition | unit_definition);

            quantity_definition.Rule = DEFINE + QUANTITY + quantity_name;
            unit_definition.Rule = DEFINE + UNIT + unit_name + OF + quantity_name + MakeStarRule(conversions, conversion);

            conversion.Rule = simple_conversion | complex_conversion;

            simple_conversion.Rule = simple_conversion_op + expression + unit_name;
            complex_conversion.Rule = complex_conversion_op + (expression_with_units | expression_with_units + EQUAL_OP + unit_variable);

            expression.Rule = literal | constant | external_variable | unit_variable | binary_expression | unary_expression;
            binary_expression.Rule = expression + 
            unary_expression.Rule = LEFT_PAREN + expression + RIGHT_PAREN;

            unit_variable.Rule = LEFT_BRACKET + unit_name + RIGHT_BRACKET;
        }
    }
}

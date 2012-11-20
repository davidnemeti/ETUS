﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony;
using Irony.Ast;
using Irony.Parsing;

namespace ETUS.Grammar
{
    public class UDLGrammar : Irony.Parsing.Grammar
    {
        public UDLGrammar()
        {
            var module = new NonTerminal("module");
            var namespace_usage = new NonTerminal("namespace_usage");
            var namespace_usages = new NonTerminal("namespace_usages");
            var namespace_declaration = new NonTerminal("namespace");
            var definitions = new NonTerminal("definitions");
            var definition = new NonTerminal("definition");
            var quantity_definition = new NonTerminal("quantity_definition");
            var unit_definition = new NonTerminal("unit_definition");

            IdentifierTerminal identifier = new IdentifierTerminal("identifier");
            NonTerminal qualified_identifier = new NonTerminal("qualified_identifier");

            NonTerminal unit_name = new NonTerminal("unit_name");
            NonTerminal quantity_name = new NonTerminal("quantity_name");
            NonTerminal namespace_name = new NonTerminal("namespace_name");

            NonTerminal conversions = new NonTerminal("conversions");
            NonTerminal conversion = new NonTerminal("conversion");
            NonTerminal simple_conversion = new NonTerminal("simple_conversion");
            NonTerminal complex_conversion = new NonTerminal("complex_conversion");
            NonTerminal simple_conversion_op = new NonTerminal("simple_conversion_op");
            NonTerminal complex_conversion_op = new NonTerminal("complex_conversion_op");
            NonTerminal unit_expression = new NonTerminal("unit_expression");
            NonTerminal binary_unit_expression = new NonTerminal("binary_unit_expression");
            NonTerminal unary_unit_expression = new NonTerminal("unary_unit_expression");
            NonTerminal complex_conversion_expression = new NonTerminal("complex_conversion_expression");
            NonTerminal expression = new NonTerminal("expression");
            NonTerminal binary_expression = new NonTerminal("binary_expression");
            NonTerminal unary_expression = new NonTerminal("unary_expression");
            NonTerminal expression_with_units = new NonTerminal("expression_with_units");
            NonTerminal binary_expression_with_units = new NonTerminal("binary_expression_with_units");
            NonTerminal unary_expression_with_units = new NonTerminal("unary_expression_with_units");
            NonTerminal unit_variable = new NonTerminal("unit_variable");
            NonTerminal binary_operator = new NonTerminal("binary_operator");
            NonTerminal unary_operator = new NonTerminal("unary_operator");

            NumberLiteral number = new NumberLiteral("number");
            ConstantTerminal constant = new ConstantTerminal("constant");
            NonTerminal external_variable = new NonTerminal("external_variable");

            KeyTerm dot = ToTerm(".");

            KeyTerm USE = ToTerm("use");
            KeyTerm DECLARE = ToTerm("declare");
            KeyTerm DEFINE = ToTerm("define");
            KeyTerm NAMESPACE = ToTerm("namespace");
            KeyTerm QUANTITY = ToTerm("quantity");
            KeyTerm UNIT = ToTerm("unit");
            KeyTerm OF = ToTerm("of");
            KeyTerm SIMPLE_MUTUAL_CONVERSION_OP = ToTerm("<=>");
            KeyTerm SIMPLE_TO_THIS_CONVERSION_OP = ToTerm("<=");
            KeyTerm SIMPLE_TO_THAT_CONVERSION_OP = ToTerm("=>");
            KeyTerm COMPLEX_MUTUAL_CONVERSION_OP = ToTerm("<:>");
            KeyTerm COMPLEX_TO_THIS_CONVERSION_OP = ToTerm("<:");
            KeyTerm COMPLEX_TO_THAT_CONVERSION_OP = ToTerm(":>");

            KeyTerm EXTERNAL_VARIABLE_PREFIX = ToTerm("::");

            KeyTerm ADD_OP = ToTerm("+");
            KeyTerm SUB_OP = ToTerm("-");
            KeyTerm POS_OP = ToTerm("+");
            KeyTerm NEG_OP = ToTerm("-");
            KeyTerm MUL_OP = ToTerm("*");
            KeyTerm DIV_OP = ToTerm("/");
            KeyTerm POW_OP = ToTerm("^");
            KeyTerm EQUAL_STATEMENT = ToTerm("=");

            KeyTerm LEFT_PAREN = ToTerm("(");
            KeyTerm RIGHT_PAREN = ToTerm(")");
            KeyTerm LEFT_BRACKET = ToTerm("[");
            KeyTerm RIGHT_BRACKET = ToTerm("]");

            RegisterOperators(20, ADD_OP, SUB_OP);
            RegisterOperators(30, MUL_OP, DIV_OP);
            RegisterOperators(40, NEG_OP, POS_OP);
            RegisterOperators(50, Associativity.Right, POW_OP);

            RegisterBracePair(LEFT_PAREN.Text, RIGHT_PAREN.Text);
            RegisterBracePair(LEFT_BRACKET.Text, RIGHT_BRACKET.Text);

            #region Constants

            constant.Add("PI", Math.PI);
            constant.Add("π", Math.PI);

            #endregion

            #region Rules

            this.Root = module;

            module.Rule = namespace_usages + namespace_declaration + definitions;

            namespace_usages.Rule = MakeStarRule(namespace_usages, namespace_usage);
            definitions.Rule = MakePlusRule(definitions, definition);
            definition.Rule = quantity_definition | unit_definition;

            namespace_usage.Rule = USE + NAMESPACE + namespace_name;
            namespace_declaration.Rule = DECLARE + NAMESPACE + namespace_name;

            quantity_definition.Rule = DEFINE + QUANTITY + quantity_name;
            unit_definition.Rule = DEFINE + UNIT + unit_name + OF + quantity_name + conversions;

            conversions.Rule = MakeStarRule(conversions, conversion);
            conversion.Rule = simple_conversion | complex_conversion;

            simple_conversion.Rule = simple_conversion_op + unit_expression |
                                        simple_conversion_op + expression + unit_expression;
            complex_conversion.Rule = complex_conversion_op + complex_conversion_expression;

            unit_expression.Rule = unit_name | binary_unit_expression | unary_unit_expression;
            binary_unit_expression.Rule =   unit_expression + MUL_OP + unit_expression |
                                            unit_expression + DIV_OP + unit_expression |
                                            unit_expression + POW_OP + number;
            unary_unit_expression.Rule = LEFT_PAREN + unit_expression + RIGHT_PAREN;

            complex_conversion_expression.Rule = expression_with_units | expression_with_units + EQUAL_STATEMENT + unit_variable;

            simple_conversion_op.Rule = SIMPLE_MUTUAL_CONVERSION_OP | SIMPLE_TO_THAT_CONVERSION_OP | SIMPLE_TO_THIS_CONVERSION_OP;
            complex_conversion_op.Rule = COMPLEX_MUTUAL_CONVERSION_OP | COMPLEX_TO_THAT_CONVERSION_OP | COMPLEX_TO_THIS_CONVERSION_OP;

            binary_operator.Rule = ADD_OP | SUB_OP | MUL_OP | DIV_OP | POW_OP;
            unary_operator.Rule = NEG_OP | POS_OP;

            expression.Rule = number | constant | external_variable | binary_expression | unary_expression;
            binary_expression.Rule = expression + binary_operator + expression;
            unary_expression.Rule = LEFT_PAREN + expression + RIGHT_PAREN | unary_operator + expression;

            expression_with_units.Rule = unit_variable | binary_expression_with_units | unary_expression_with_units;
            binary_expression_with_units.Rule = expression + binary_operator + expression_with_units | expression_with_units + binary_operator + expression;
            unary_expression_with_units.Rule = LEFT_PAREN + expression_with_units + RIGHT_PAREN | unary_operator + expression_with_units;

            unit_variable.Rule = LEFT_BRACKET + unit_name + RIGHT_BRACKET;

            qualified_identifier.Rule = MakePlusRule(qualified_identifier, dot, identifier);

            unit_name.Rule = identifier;
            quantity_name.Rule = identifier;
            namespace_name.Rule = qualified_identifier;
            external_variable.Rule = EXTERNAL_VARIABLE_PREFIX + qualified_identifier;

            #endregion
        }

        new private KeyTerm ToTerm(string text)
        {
            return base.ToTerm(text, string.Format("\"{0}\"", text));
        }
    }
}

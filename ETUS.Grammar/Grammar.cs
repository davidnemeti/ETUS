using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Irony.AstBinders;

using DomainCore;

using ETUS.DomainModel;
using ETUS.DomainModel.Expressions;

namespace ETUS.Grammar
{
    public class UDLGrammar : Irony.Parsing.Grammar
    {
        public UDLGrammar()
        {
            NonTerminalWithType group = NonTerminalWithType.Of<Group>();
            NonTerminalWithType namespace_usage = NonTerminalWithType.Of<NamespaceUsing>();
            NonTerminalWithType namespace_usages = NonTerminalWithType.Of<List<NamespaceUsing>>();
            NonTerminalWithType @namespace = NonTerminalWithType.Of<Namespace>();
            NonTerminalWithType namespaces = NonTerminalWithType.Of<List<Namespace>>();
            NonTerminalWithType definitions = NonTerminalWithType.Of<List<Definition>>();
            NonTerminalWithType definition = NonTerminalWithType.OfAbstract<Definition>();
            NonTerminalWithType quantity_definition = NonTerminalWithType.Of<QuantityDefinition>();
            NonTerminalWithType prefix_definition = NonTerminalWithType.Of<PrefixDefinition>();
            NonTerminalWithType unit_definition = NonTerminalWithType.Of<UnitDefinition>();

            IdentifierTerminal identifier = new IdentifierTerminal("identifier");
            NonTerminal qualified_identifier = new NonTerminal("qualified_identifier");

            NonTerminalWithType unit_name = NonTerminalWithType.Of<Name>();
            NonTerminalWithType prefix_name = NonTerminalWithType.Of<Name>();
            NonTerminalWithType quantity_name = NonTerminalWithType.Of<Name>();
            NonTerminalWithType namespace_name = NonTerminalWithType.Of<Name>();

            NonTerminalWithType conversions = NonTerminalWithType.Of<List<Conversion>>();
            NonTerminalWithType conversion = NonTerminalWithType.OfAbstract<Conversion>();
            NonTerminalWithType simple_conversion = NonTerminalWithType.Of<SimpleConversion>();
            NonTerminalWithType complex_conversion = NonTerminalWithType.Of<ComplexConversion>();
            NonTerminalWithType simple_conversion_op = NonTerminalWithType.Of<Direction>();
//            NonTerminal simple_conversion_op = new NonTerminal("simple_conversion_op");
            NonTerminalWithType complex_conversion_op = NonTerminalWithType.Of<Direction>();
            NonTerminalWithType unit_expression = NonTerminalWithType.OfAbstract<UnitExpression>();
            NonTerminalWithType binary_unit_expression = NonTerminalWithType.Of<UnitExpression.Binary>();
            NonTerminal unary_unit_expression = new NonTerminal("unary_unit_expression");
            NonTerminalWithType complex_conversion_expression = NonTerminalWithType.OfAbstract<ExpressionWithUnit>();
            NonTerminalWithType expression = NonTerminalWithType.OfAbstract<Expression>();
            NonTerminalWithType binary_expression = NonTerminalWithType.Of<Expression.Binary>();
            NonTerminalWithType unary_expression = NonTerminalWithType.Of<Expression.Unary>();
            NonTerminalWithType expression_with_unit = NonTerminalWithType.OfAbstract<ExpressionWithUnit>();
            NonTerminalWithType binary_expression_with_unit = NonTerminalWithType.Of<ExpressionWithUnit.Binary>();
            NonTerminalWithType binary_expression_with_unit2 = NonTerminalWithType.Of<ExpressionWithUnit.Binary2>();
            NonTerminalWithType unary_expression_with_unit = NonTerminalWithType.Of<ExpressionWithUnit.Unary>();
            NonTerminalWithType unit_variable = NonTerminalWithType.Of<ExpressionWithUnit.Unit>();
            NonTerminalWithType binary_operator = NonTerminalWithType.Of<BinaryOperator>();
            NonTerminalWithType unary_operator = NonTerminalWithType.Of<UnaryOperator>();
            NonTerminalWithType external_variable = NonTerminalWithType.Of<Expression.ExternalVariable>();

            ObjectBoundToBnfExpression NUMBER = new NumberLiteral("number").Bind((context, parseNode) => new Expression.Number<double> { Value = Convert.ToDouble(parseNode.Token.Value) });

            ConstantTerminal CONSTANT = new ConstantTerminal("constant");

            KeyTerm DOT = ToTerm(".");

            KeyTerm USE = ToTerm("use");
            KeyTerm DECLARE = ToTerm("declare");
            KeyTerm DEFINE = ToTerm("define");
            KeyTerm PREFIX = ToTerm("prefix");
            KeyTerm NAMESPACE = ToTerm("namespace");
            KeyTerm QUANTITY = ToTerm("quantity");
            KeyTerm UNIT = ToTerm("unit");
            KeyTerm OF = ToTerm("of");
            ObjectBoundToBnfExpression SIMPLE_MUTUAL_CONVERSION_OP = ToTerm("<=>").Bind((context, parseNode) => Direction.BiDir);
            ObjectBoundToBnfExpression SIMPLE_TO_THIS_CONVERSION_OP = ToTerm("<=").Bind((context, parseNode) => Direction.From);
            ObjectBoundToBnfExpression SIMPLE_TO_THAT_CONVERSION_OP = ToTerm("=>").Bind((context, parseNode) => Direction.To);
            ObjectBoundToBnfExpression COMPLEX_MUTUAL_CONVERSION_OP = ToTerm("<:>").Bind((context, parseNode) => Direction.BiDir);
            ObjectBoundToBnfExpression COMPLEX_TO_THIS_CONVERSION_OP = ToTerm("<:").Bind((context, parseNode) => Direction.From);
            ObjectBoundToBnfExpression COMPLEX_TO_THAT_CONVERSION_OP = ToTerm(":>").Bind((context, parseNode) => Direction.To);

            KeyTerm EXTERNAL_VARIABLE_PREFIX = ToTerm("::");

            ObjectBoundToBnfExpression POS_OP = ToTerm("+").Bind((context, parseNode) => UnaryOperator.Pos);
            ObjectBoundToBnfExpression NEG_OP = ToTerm("-").Bind((context, parseNode) => UnaryOperator.Neg);

            ObjectBoundToBnfExpression ADD_OP = ToTerm("+").Bind((context, parseNode) => BinaryOperator.Add);
            ObjectBoundToBnfExpression SUB_OP = ToTerm("-").Bind((context, parseNode) => BinaryOperator.Sub);
            ObjectBoundToBnfExpression MUL_OP = ToTerm("*").Bind((context, parseNode) => BinaryOperator.Mul);
            ObjectBoundToBnfExpression DIV_OP = ToTerm("/").Bind((context, parseNode) => BinaryOperator.Div);
            ObjectBoundToBnfExpression POW_OP = ToTerm("^").Bind((context, parseNode) => BinaryOperator.Pow);

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

            CONSTANT.Add("PI", Constant.PI);
            CONSTANT.Add("π", Constant.PI);

            #endregion

            #region Rules

            this.Root = group;

            group.Rule = namespace_usages.Bind(() => new Group().NamespaceUsings) + namespaces.Bind(() => new Group().Namespaces);

            namespace_usages.Rule = MakeStarRule(namespace_usages, namespace_usage)
                .Bind((context, parseNode) => new List<NamespaceUsing>(parseNode.ChildNodes.Select(childNode => childNode.AstNode).Cast<NamespaceUsing>()));

            namespaces.Rule = MakePlusRule(namespaces, @namespace)
                .Bind((context, parseNode) => new List<Namespace>(parseNode.ChildNodes.Select(childNode => childNode.AstNode).Cast<Namespace>()));

            definitions.Rule = MakePlusRule(definitions, definition)
                .Bind((context, parseNode) => new List<Definition>(parseNode.ChildNodes.Select(childNode => childNode.AstNode).Cast<Definition>()));

            definition.Rule = quantity_definition | unit_definition | prefix_definition;

            namespace_usage.Rule = USE + NAMESPACE + namespace_name;
            namespace_declaration.Rule = DECLARE + NAMESPACE + namespace_name;

            //BnfTermInRule id;
            //namespace_declaration.Rule = DECLARE + NAMESPACE + (id = identifier);
            //namespace_declaration.Rule = DECLARE + NAMESPACE + Bind(identifier, () => new NamespaceDeclaration().Name);
            //Bind<NamespaceDeclaration>(namespace_declaration);
            //Bind(id, () => new NamespaceDeclaration().Name);
            //Bind2(identifier => new NamespaceDeclaration().Name);

//            namespace_declaration.AstConfig.NodeCreator = (astContext, parseTreeNode) => parseTreeNode.AstNode = new NamespaceDeclaration() { Name = new Name((string)parseTreeNode.ChildNodes[2].AstNode) };
            identifier.AstConfig.NodeCreator = (astContext, parseTreeNode) => parseTreeNode.AstNode = parseTreeNode.FindTokenAndGetText();

            prefix_definition.Rule = DEFINE + PREFIX + prefix_name + expression;
            quantity_definition.Rule = DEFINE + QUANTITY + quantity_name;
            unit_definition.Rule = DEFINE + UNIT + unit_name + OF + quantity_name + conversions;

            conversions.Rule = MakeStarRule(conversions, conversion);
            conversion.Rule = simple_conversion | complex_conversion;

            simple_conversion.Rule = simple_conversion_op + unit_expression |
                                        simple_conversion_op + expression + unit_expression;
            complex_conversion.Rule = complex_conversion_op + complex_conversion_expression;

            unit_expression.Rule = unit_name | binary_unit_expression | unary_unit_expression;
            binary_unit_expression.Rule =   unit_expression + MUL_OP + unit_expression |
                                            "1" + DIV_OP + unit_expression |
                                            unit_expression + DIV_OP + unit_expression |
                                            unit_expression + POW_OP + NUMBER;
            unary_unit_expression.Rule = LEFT_PAREN + unit_expression + RIGHT_PAREN;

            complex_conversion_expression.Rule = expression_with_unit | expression_with_unit + EQUAL_STATEMENT + unit_variable;

            simple_conversion_op.Rule = SIMPLE_MUTUAL_CONVERSION_OP | SIMPLE_TO_THAT_CONVERSION_OP | SIMPLE_TO_THIS_CONVERSION_OP;
            complex_conversion_op.Rule = COMPLEX_MUTUAL_CONVERSION_OP | COMPLEX_TO_THAT_CONVERSION_OP | COMPLEX_TO_THIS_CONVERSION_OP;

//            binary_operator.AstConfig.NodeCreator = (astContext, parseTreeNode) => parseTreeNode.AstNode = parseTreeNode.ChildNodes[0].Token.ValueString == ADD_OP.Text ? new BinaryOperator.Add() : null;
            binary_operator.Rule = ADD_OP | SUB_OP | MUL_OP | DIV_OP | POW_OP;
            unary_operator.Rule = NEG_OP | POS_OP;

            var boo = ToTerm("boo");
            var soo = ToTerm("soo");
            MarkPunctuation(soo);
            expression.Rule = NUMBER | CONSTANT | external_variable | binary_expression | unary_expression | Empty;
            binary_expression.Rule = boo + binary_Expression__term1 + binary_Expression__op + binary_Expression__term2 |
                soo + binary_Expression__term2 + binary_Expression__op + binary_Expression__term1;
            unary_expression.Rule = LEFT_PAREN + expression + RIGHT_PAREN | unary_operator + expression;

            expression_with_unit.Rule = unit_variable | binary_expression_with_unit | binary_expression_with_unit2 | unary_expression_with_unit;
            binary_expression_with_unit.Rule = expression_with_unit + binary_operator + expression;
            binary_expression_with_unit2.Rule = expression + binary_operator + expression_with_unit;
            unary_expression_with_unit.Rule = LEFT_PAREN + expression_with_unit + RIGHT_PAREN | unary_operator + expression_with_unit;

            unit_variable.Rule = LEFT_BRACKET + unit_name + RIGHT_BRACKET;

            qualified_identifier.Rule = MakePlusRule(qualified_identifier, DOT, identifier);

            unit_name.Rule = identifier;
            prefix_name.Rule = identifier;
            quantity_name.Rule = identifier;
            namespace_name.Rule = qualified_identifier;
            external_variable.Rule = EXTERNAL_VARIABLE_PREFIX + qualified_identifier;

            #endregion

            LanguageFlags = LanguageFlags.CreateAst;
        }


        new private KeyTerm ToTerm(string text)
        {
            return base.ToTerm(text, string.Format("\"{0}\"", text));
        }
    }
}

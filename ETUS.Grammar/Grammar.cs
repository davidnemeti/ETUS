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
            var group = TypeForBoundMembers.Of<Group>();
            var namespace_usage = TypeForBoundMembers.Of<NamespaceUsing>();
            var namespace_usages = TypeForCollection.Of<List<NamespaceUsing>>();
            var @namespace = TypeForBoundMembers.Of<Namespace>();
            var namespaces = TypeForCollection.Of<List<Namespace>>();
            var definitions = TypeForCollection.Of<List<Definition>>();
            var definition = TypeForTransient.Of<Definition>();
            var quantity_definition = TypeForBoundMembers.Of<QuantityDefinition>();
            var prefix_definition = TypeForBoundMembers.Of<PrefixDefinition>();
            var unit_definition = TypeForBoundMembers.Of<UnitDefinition>();

            IdentifierTerminal IDENTIFIER = new IdentifierTerminal("identifier");
            NonTerminal qualified_identifier = new NonTerminal("qualified_identifier");

            DataForBnfTerm<Name> name = IDENTIFIER.SetValue((context, parseNode) => new Name { Value = parseNode.Token.ValueString });
            DataForBnfTerm<Name> namespace_name = qualified_identifier.SetValue((context, parseNode) => new Name { Value = parseNode.Token.ValueString });
            DataForBnfTerm<NameRef> nameref = qualified_identifier.SetValue((context, parseNode) => new NameRef(parseNode.Token.ValueString));

            DataForBnfTerm<Reference<QuantityDefinition>> quantity_reference = nameref.SetValue(nameRef => Reference.Get<QuantityDefinition>(nameRef));
            DataForBnfTerm<Reference<UnitDefinition>> unit_reference = nameref.SetValue(nameRef => Reference.Get<UnitDefinition>(nameRef));

            var conversions = TypeForCollection.Of<List<Conversion>>();
            var conversion = TypeForTransient.Of<Conversion>();
            var simple_conversion = TypeForBoundMembers.Of<SimpleConversion>();
            var complex_conversion = TypeForBoundMembers.Of<ComplexConversion>();
            var simple_conversion_op = TypeForBoundMembers.Of<Direction>();
//            NonTerminal simple_conversion_op = new NonTerminal("simple_conversion_op");
            var complex_conversion_op = TypeForBoundMembers.Of<Direction>();
            var unit_expression = TypeForTransient.Of<UnitExpression>();
            var binary_unit_expression = TypeForBoundMembers.Of<UnitExpression.Binary>();
            NonTerminal unary_unit_expression = new NonTerminal("unary_unit_expression");
            var complex_conversion_expression = TypeForTransient.Of<ExpressionWithUnit>();
            var expression = TypeForTransient.Of<Expression>();
            var binary_expression = TypeForBoundMembers.Of<Expression.Binary>();
            var unary_expression = TypeForBoundMembers.Of<Expression.Unary>();
            var expression_with_unit = TypeForTransient.Of<ExpressionWithUnit>();
            var binary_expression_with_unit = TypeForBoundMembers.Of<ExpressionWithUnit.Binary>();
            var binary_expression_with_unit2 = TypeForBoundMembers.Of<ExpressionWithUnit.Binary2>();
            var unary_expression_with_unit = TypeForBoundMembers.Of<ExpressionWithUnit.Unary>();
            var unit_variable = TypeForBoundMembers.Of<ExpressionWithUnit.Unit>();
            var binary_operator = TypeForTransient.Of<BinaryOperator>();
            var unary_operator = TypeForTransient.Of<UnaryOperator>();
            var external_variable = TypeForBoundMembers.Of<Expression.ExternalVariable>();

            DataForBnfTerm<Expression.Number<double>> NUMBER = new NumberLiteral("number")
                .SetValue((context, parseNode) => new Expression.Number<double> { Value = Convert.ToDouble(parseNode.Token.Value) });

            var SIMPLE_MUTUAL_CONVERSION_OP = ToTerm("<=>").SetValue(Direction.BiDir);
            var SIMPLE_TO_THIS_CONVERSION_OP = ToTerm("<=").SetValue(Direction.From);
            var SIMPLE_TO_THAT_CONVERSION_OP = ToTerm("=>").SetValue(Direction.To);
            var COMPLEX_MUTUAL_CONVERSION_OP = ToTerm("<:>").SetValue(Direction.BiDir);
            var COMPLEX_TO_THIS_CONVERSION_OP = ToTerm("<:").SetValue(Direction.From);
            var COMPLEX_TO_THAT_CONVERSION_OP = ToTerm(":>").SetValue(Direction.To);

            var POS_OP = ToTerm("+").SetValue(UnaryOperator.Pos);
            var NEG_OP = ToTerm("-").SetValue(UnaryOperator.Neg);

            var ADD_OP = ToTerm("+").SetValue(BinaryOperator.Add);
            var SUB_OP = ToTerm("-").SetValue(BinaryOperator.Sub);
            var MUL_OP = ToTerm("*").SetValue(BinaryOperator.Mul);
            var DIV_OP = ToTerm("/").SetValue(BinaryOperator.Div);
            var POW_OP = ToTerm("^").SetValue(BinaryOperator.Pow);

            ConstantTerminal CONSTANT = new ConstantTerminal("constant");

            KeyTerm USE = ToTerm("use");
            KeyTerm DECLARE = ToTerm("declare");
            KeyTerm DEFINE = ToTerm("define");
            KeyTerm PREFIX = ToTerm("prefix");
            KeyTerm NAMESPACE = ToTerm("namespace");
            KeyTerm QUANTITY = ToTerm("quantity");
            KeyTerm UNIT = ToTerm("unit");
            KeyTerm OF = ToTerm("of");
            KeyTerm EXTERNAL_VARIABLE_PREFIX = ToTerm("::");
            KeyTerm DOT = ToTerm(".");
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

            group.Rule = namespace_usages.Bind(() => group._.NamespaceUsings) + namespaces.Bind(() => group._.Namespaces);

            group.Rule = namespace_usage.StarList<List<NamespaceUsing>, NamespaceUsing>().Bind(() => group._.NamespaceUsings) + namespaces.Bind(() => group._.Namespaces);
            group.Rule = namespace_usage.BindStarList(typeof(List<>), () => group._.NamespaceUsings) + namespaces.Bind(() => group._.Namespaces);

            namespace_usages.Rule = MakeStarRule(namespace_usages, namespace_usage);

            namespaces.Rule = MakePlusRule(namespaces, @namespace);

            definitions.Rule = MakePlusRule(definitions, definition);

            definition.Rule = quantity_definition | unit_definition | prefix_definition;

            namespace_usage.Rule = USE + NAMESPACE + nameref.Bind(() => namespace_usage._.NameRef);
            @namespace.Rule = DECLARE + NAMESPACE + namespace_name.Bind(() => @namespace._.Name) + definitions.Bind(() => @namespace._.Definitions);

            prefix_definition.Rule = DEFINE + PREFIX + name.Bind(() => prefix_definition._.Name) + expression.Bind(() => prefix_definition._.Factor);
            quantity_definition.Rule = DEFINE + QUANTITY + name.Bind(() => quantity_definition._.Name);

            unit_definition.Rule = DEFINE + UNIT + name.Bind(() => unit_definition._.Name) + OF + quantity_reference.Bind(() => unit_definition._.Quantity) +
                conversions.Bind(() => unit_definition._.Conversions);

            conversions.Rule = MakeStarRule(conversions, conversion);
            conversion.Rule = simple_conversion | complex_conversion;

            simple_conversion.Rule = simple_conversion_op + unit_expression |
                                        simple_conversion_op + expression + unit_expression;
            complex_conversion.Rule = complex_conversion_op + complex_conversion_expression;

            unit_expression.Rule = name | binary_unit_expression | unary_unit_expression;
            binary_unit_expression.Rule =   unit_expression + MUL_OP + unit_expression |
                                            "1" + DIV_OP + unit_expression |
                                            unit_expression + DIV_OP + unit_expression |
                                            unit_expression + POW_OP + NUMBER;
            unary_unit_expression.Rule = LEFT_PAREN + unit_expression + RIGHT_PAREN;

            complex_conversion_expression.Rule = expression_with_unit | expression_with_unit + EQUAL_STATEMENT + unit_variable;

            simple_conversion_op.Rule = SIMPLE_MUTUAL_CONVERSION_OP | SIMPLE_TO_THAT_CONVERSION_OP | SIMPLE_TO_THIS_CONVERSION_OP;
            complex_conversion_op.Rule = COMPLEX_MUTUAL_CONVERSION_OP | COMPLEX_TO_THAT_CONVERSION_OP | COMPLEX_TO_THIS_CONVERSION_OP;

            binary_operator.Rule = ADD_OP | SUB_OP | MUL_OP | DIV_OP | POW_OP;
            unary_operator.Rule = NEG_OP | POS_OP;

            expression.Rule = NUMBER | CONSTANT | external_variable | binary_expression | unary_expression | Empty;
            binary_expression.Rule = expression.Bind(() => binary_expression._.Term1) + binary_operator.Bind(() => binary_expression._.Op) + expression.Bind(() => binary_expression._.Term2);
            unary_expression.Rule = LEFT_PAREN + expression + RIGHT_PAREN | unary_operator + expression;

            expression_with_unit.Rule = unit_variable | binary_expression_with_unit | binary_expression_with_unit2 | unary_expression_with_unit;
            binary_expression_with_unit.Rule = expression_with_unit + binary_operator + expression;
            binary_expression_with_unit2.Rule = expression + binary_operator + expression_with_unit;
            unary_expression_with_unit.Rule = LEFT_PAREN + expression_with_unit + RIGHT_PAREN | unary_operator + expression_with_unit;

            unit_variable.Rule = LEFT_BRACKET + unit_reference + RIGHT_BRACKET;

            qualified_identifier.Rule = MakePlusRule(qualified_identifier, DOT, IDENTIFIER);

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

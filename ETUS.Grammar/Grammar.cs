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
            TypeForBoundMembers group = TypeForBoundMembers.Of<Group>();
            TypeForBoundMembers namespace_usage = TypeForBoundMembers.Of<NamespaceUsing>();
            TypeForCollection namespace_usages = TypeForCollection.Of<List<NamespaceUsing>>();
            TypeForBoundMembers @namespace = TypeForBoundMembers.Of<Namespace>();
            TypeForCollection namespaces = TypeForCollection.Of<List<Namespace>>();
            TypeForCollection definitions = TypeForCollection.Of<List<Definition>>();
            TypeForTransient definition = TypeForTransient.Of<Definition>();
            TypeForBoundMembers quantity_definition = TypeForBoundMembers.Of<QuantityDefinition>();
            TypeForBoundMembers prefix_definition = TypeForBoundMembers.Of<PrefixDefinition>();
            TypeForBoundMembers unit_definition = TypeForBoundMembers.Of<UnitDefinition>();

            IdentifierTerminal IDENTIFIER = new IdentifierTerminal("identifier");
            NonTerminal qualified_identifier = new NonTerminal("qualified_identifier");

            ObjectBoundToBnfTerm<Name> name = IDENTIFIER.Bind((context, parseNode) => new Name { Value = parseNode.Token.ValueString });
            ObjectBoundToBnfTerm<NameRef> namespace_name = qualified_identifier.Bind((context, parseNode) => new NameRef(parseNode.Token.ValueString));
            ObjectBoundToBnfTerm<NameRef> nameref = qualified_identifier.Bind((context, parseNode) => new NameRef(parseNode.Token.ValueString));

            ObjectBoundToBnfTerm<Reference<QuantityDefinition>> quantity_reference = nameref.Bind((context, parseNode, nameRef) => Reference.Get<QuantityDefinition>(nameRef));
            ObjectBoundToBnfTerm<Reference<UnitDefinition>> unit_reference = nameref.Bind((context, parseNode, nameRef) => Reference.Get<UnitDefinition>(nameRef));

            TypeForCollection conversions = TypeForCollection.Of<List<Conversion>>();
            TypeForTransient conversion = TypeForTransient.Of<Conversion>();
            TypeForBoundMembers simple_conversion = TypeForBoundMembers.Of<SimpleConversion>();
            TypeForBoundMembers complex_conversion = TypeForBoundMembers.Of<ComplexConversion>();
            TypeForBoundMembers simple_conversion_op = TypeForBoundMembers.Of<Direction>();
//            NonTerminal simple_conversion_op = new NonTerminal("simple_conversion_op");
            TypeForBoundMembers complex_conversion_op = TypeForBoundMembers.Of<Direction>();
            TypeForTransient unit_expression = TypeForTransient.Of<UnitExpression>();
            TypeForBoundMembers binary_unit_expression = TypeForBoundMembers.Of<UnitExpression.Binary>();
            NonTerminal unary_unit_expression = new NonTerminal("unary_unit_expression");
            TypeForTransient complex_conversion_expression = TypeForTransient.Of<ExpressionWithUnit>();
            TypeForTransient expression = TypeForTransient.Of<Expression>();
            TypeForBoundMembers binary_expression = TypeForBoundMembers.Of<Expression.Binary>();
            TypeForBoundMembers unary_expression = TypeForBoundMembers.Of<Expression.Unary>();
            TypeForTransient expression_with_unit = TypeForTransient.Of<ExpressionWithUnit>();
            TypeForBoundMembers binary_expression_with_unit = TypeForBoundMembers.Of<ExpressionWithUnit.Binary>();
            TypeForBoundMembers binary_expression_with_unit2 = TypeForBoundMembers.Of<ExpressionWithUnit.Binary2>();
            TypeForBoundMembers unary_expression_with_unit = TypeForBoundMembers.Of<ExpressionWithUnit.Unary>();
            TypeForBoundMembers unit_variable = TypeForBoundMembers.Of<ExpressionWithUnit.Unit>();
            TypeForTransient binary_operator = TypeForTransient.Of<BinaryOperator>();
            TypeForTransient unary_operator = TypeForTransient.Of<UnaryOperator>();
            TypeForBoundMembers external_variable = TypeForBoundMembers.Of<Expression.ExternalVariable>();

            ObjectBoundToBnfTerm NUMBER = new NumberLiteral("number").Bind((context, parseNode) => new Expression.Number<double> { Value = Convert.ToDouble(parseNode.Token.Value) });

            ObjectBoundToBnfTerm SIMPLE_MUTUAL_CONVERSION_OP = ToTerm("<=>").Bind((context, parseNode) => Direction.BiDir);
            ObjectBoundToBnfTerm SIMPLE_TO_THIS_CONVERSION_OP = ToTerm("<=").Bind((context, parseNode) => Direction.From);
            ObjectBoundToBnfTerm SIMPLE_TO_THAT_CONVERSION_OP = ToTerm("=>").Bind((context, parseNode) => Direction.To);
            ObjectBoundToBnfTerm COMPLEX_MUTUAL_CONVERSION_OP = ToTerm("<:>").Bind((context, parseNode) => Direction.BiDir);
            ObjectBoundToBnfTerm COMPLEX_TO_THIS_CONVERSION_OP = ToTerm("<:").Bind((context, parseNode) => Direction.From);
            ObjectBoundToBnfTerm COMPLEX_TO_THAT_CONVERSION_OP = ToTerm(":>").Bind((context, parseNode) => Direction.To);

            ObjectBoundToBnfTerm POS_OP = ToTerm("+").Bind((context, parseNode) => UnaryOperator.Pos);
            ObjectBoundToBnfTerm NEG_OP = ToTerm("-").Bind((context, parseNode) => UnaryOperator.Neg);

            ObjectBoundToBnfTerm ADD_OP = ToTerm("+").Bind((context, parseNode) => BinaryOperator.Add);
            ObjectBoundToBnfTerm SUB_OP = ToTerm("-").Bind((context, parseNode) => BinaryOperator.Sub);
            ObjectBoundToBnfTerm MUL_OP = ToTerm("*").Bind((context, parseNode) => BinaryOperator.Mul);
            ObjectBoundToBnfTerm DIV_OP = ToTerm("/").Bind((context, parseNode) => BinaryOperator.Div);
            ObjectBoundToBnfTerm POW_OP = ToTerm("^").Bind((context, parseNode) => BinaryOperator.Pow);


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

            group.Rule = namespace_usages.Bind(() => new Group().NamespaceUsings) + namespaces.Bind(() => new Group().Namespaces);

            namespace_usages.Rule = MakeStarRule(namespace_usages, namespace_usage);

            namespaces.Rule = MakePlusRule(namespaces, @namespace);

            definitions.Rule = MakePlusRule(definitions, definition);

            definition.Rule = quantity_definition | unit_definition | prefix_definition;

            namespace_usage.Rule = USE + NAMESPACE + nameref.Bind(() => new NamespaceUsing().NameRef);
            @namespace.Rule = DECLARE + NAMESPACE + namespace_name.Bind(() => new Namespace().Name) + definitions.Bind(() => new Namespace().Definitions);

            prefix_definition.Rule = DEFINE + PREFIX + name.Bind(() => new PrefixDefinition().Name) + expression.Bind(() => new PrefixDefinition().Factor);
            quantity_definition.Rule = DEFINE + QUANTITY + name.Bind(() => new QuantityDefinition().Name);

            unit_definition.Rule = DEFINE + UNIT + name.Bind(() => new UnitDefinition().Name) + OF + quantity_reference.Bind(() => new UnitDefinition().Quantity) +
                conversions.Bind(() => new UnitDefinition().Conversions);

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
            binary_expression.Rule = expression + binary_operator + expression;
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

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
using Irony.Extension;
using Irony.Extension.AstBinders;

using DomainCore;

using ETUS.DomainModel;
using ETUS.DomainModel.Expressions;

namespace ETUS.Grammar
{
    public class UDLGrammar : GrammarExtension
    {
        public UDLGrammar()
        {
            var group = TypeForBoundMembers.Of<Group>();
            var namespace_usage = TypeForBoundMembers.Of<NamespaceUsing>();
            var @namespace = TypeForBoundMembers.Of<Namespace>();
            var definition = TypeForTransient.Of<Definition>();
            var quantity_definition = TypeForBoundMembers.Of<QuantityDefinition>();
            var prefix_definition = TypeForBoundMembers.Of<PrefixDefinition>();
            var unit_definition = TypeForBoundMembers.Of<UnitDefinition>();

            KeyTerm DOT = ToTerm(".");
            IdentifierTerminal IDENTIFIER = new IdentifierTerminal("identifier");
            IBnfTerm<string> qualified_identifier = IDENTIFIER.PlusList<string>(DOT).ConvertValue(identifiers => string.Join(DOT.Text, identifiers));

            ValueForBnfTerm<Name> name = IDENTIFIER.CreateValue((context, parseNode) => new Name { Value = parseNode.Token.ValueString });
            ValueForBnfTerm<Name> namespace_name = qualified_identifier.ConvertValue(qual_id => new Name {Value = qual_id});
            ValueForBnfTerm<NameRef> nameref = qualified_identifier.ConvertValue(qual_id => new NameRef(qual_id));

            ValueForBnfTerm<Reference<QuantityDefinition>> quantity_reference = nameref.ConvertValue(nameRef => Reference.Get<QuantityDefinition>(nameRef));
            ValueForBnfTerm<Reference<UnitDefinition>> unit_reference = nameref.ConvertValue(nameRef => Reference.Get<UnitDefinition>(nameRef));

            var conversion = TypeForTransient.Of<Conversion>();
            var simple_conversion = TypeForBoundMembers.Of<SimpleConversion>();
            var complex_conversion = TypeForBoundMembers.Of<ComplexConversion>();
            var simple_conversion_op = TypeForBoundMembers.Of<Direction>();
            var complex_conversion_op = TypeForBoundMembers.Of<Direction>();
            var unit_expression = TypeForTransient.Of<UnitExpression>();
            var binary_unit_expression = TypeForBoundMembers.Of<UnitExpression.Binary>();
            var square_unit_expression = TypeForBoundMembers.Of<UnitExpression.Square>();
            var cube_unit_expression = TypeForBoundMembers.Of<UnitExpression.Cube>();
            var recip_unit_expression = TypeForBoundMembers.Of<UnitExpression.Recip>();
            var unit_unit_expression = TypeForBoundMembers.Of<UnitExpression.Unit>();
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
            var unit_expression_binary_operator = TypeForTransient.Of<UnitExpression.Binary.Operator>();
            var unary_operator = TypeForTransient.Of<UnaryOperator>();
            var external_variable = TypeForBoundMembers.Of<Expression.ExternalVariable>();

            ValueForBnfTerm<Expression.Number<double>> NUMBER = new NumberLiteral("number")
                .CreateValue((context, parseNode) => new Expression.Number<double> { Value = Convert.ToDouble(parseNode.Token.Value) });

            var SIMPLE_MUTUAL_CONVERSION_OP = ToTerm("<=>").CreateValue(Direction.BiDir);
            var SIMPLE_TO_THIS_CONVERSION_OP = ToTerm("<=").CreateValue(Direction.From);
            var SIMPLE_TO_THAT_CONVERSION_OP = ToTerm("=>").CreateValue(Direction.To);
            var COMPLEX_MUTUAL_CONVERSION_OP = ToTerm("<:>").CreateValue(Direction.BiDir);
            var COMPLEX_TO_THIS_CONVERSION_OP = ToTerm("<:").CreateValue(Direction.From);
            var COMPLEX_TO_THAT_CONVERSION_OP = ToTerm(":>").CreateValue(Direction.To);

            var POS_OP = ToTerm("+").CreateValue(UnaryOperator.Pos);
            var NEG_OP = ToTerm("-").CreateValue(UnaryOperator.Neg);

            var ADD_OP = ToTerm("+").CreateValue(BinaryOperator.Add);
            var SUB_OP = ToTerm("-").CreateValue(BinaryOperator.Sub);
            var MUL_OP = ToTerm("*").CreateValue(BinaryOperator.Mul);
            var DIV_OP = ToTerm("/").CreateValue(BinaryOperator.Div);
            var POW_OP = ToTerm("^").CreateValue(BinaryOperator.Pow);

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

            group.Rule = namespace_usage.StarList().BindMember(group, () => group._.NamespaceUsings) + @namespace.PlusList().BindMember(group, () => group._.Namespaces);

            definition.SetRuleOr(quantity_definition, unit_definition, prefix_definition);

            namespace_usage.Rule = USE + NAMESPACE + nameref.BindMember(namespace_usage, () => namespace_usage._.NameRef);

            @namespace.Rule = DECLARE + NAMESPACE + namespace_name.BindMember(@namespace, () => @namespace._.Name) + definition.PlusList().BindMember(@namespace, () => @namespace._.Definitions);

            prefix_definition.Rule =
                DEFINE + PREFIX
                + name.BindMember(prefix_definition, () => prefix_definition._.Name)
                + expression.BindMember(prefix_definition, () => prefix_definition._.Factor);

            quantity_definition.Rule = DEFINE + QUANTITY + name.BindMember(quantity_definition, () => quantity_definition._.Name);

            unit_definition.Rule =
                DEFINE + UNIT
                + name.BindMember(unit_definition, () => unit_definition._.Name)
                + OF
                + quantity_reference.BindMember(unit_definition, () => unit_definition._.Quantity)
                + conversion.StarList().BindMember(unit_definition, () => unit_definition._.Conversions);

            conversion.SetRuleOr(simple_conversion, complex_conversion);

            simple_conversion.Rule =
                simple_conversion_op.BindMember(simple_conversion, () => simple_conversion._.Direction)
                + unit_expression.BindMember(simple_conversion, () => simple_conversion._.OtherUnit)
                |
                simple_conversion_op.BindMember(simple_conversion, () => simple_conversion._.Direction)
                + expression.BindMember(simple_conversion, () => simple_conversion._.Factor)
                + unit_expression.BindMember(simple_conversion, () => simple_conversion._.OtherUnit);

            complex_conversion.Rule =
                complex_conversion_op.BindMember(complex_conversion, () => complex_conversion._.Direction)
                + complex_conversion_expression.BindMember(complex_conversion, () => complex_conversion._.Expr);

            unit_expression.SetRuleOr(
                binary_unit_expression,
                square_unit_expression,
                cube_unit_expression,
                recip_unit_expression,
                unit_unit_expression,
                LEFT_PAREN + unit_expression + RIGHT_PAREN
                );

            binary_unit_expression.Rule =
                unit_expression.BindMember(binary_unit_expression, () => binary_unit_expression._.Term1)
                + unit_expression_binary_operator.BindMember(binary_unit_expression, () => binary_unit_expression._.Op)
                + unit_expression.BindMember(binary_unit_expression, () => binary_unit_expression._.Term2);

            square_unit_expression.Rule =
                unit_expression.BindMember(square_unit_expression, () => square_unit_expression._.Base)
                + POW_OP.Cast(square_unit_expression)
                + ToTerm("2").ToType(square_unit_expression);

            cube_unit_expression.Rule =
                unit_expression.BindMember(cube_unit_expression, () => cube_unit_expression._.Base)
                + POW_OP
                + ToTerm("3");

            recip_unit_expression.Rule =
                ToTerm("1")
                + DIV_OP
                + unit_expression.BindMember(recip_unit_expression, () => recip_unit_expression._.Denominator);

            unit_unit_expression.Rule = unit_definition.ConvertValue(unitDefinition => unitDefinition.GetReference()).BindMember(unit_unit_expression, () => unit_unit_expression._.Value);

            complex_conversion_expression.Rule = expression_with_unit | expression_with_unit + EQUAL_STATEMENT + unit_variable;

            simple_conversion_op.Rule = SIMPLE_MUTUAL_CONVERSION_OP | SIMPLE_TO_THAT_CONVERSION_OP | SIMPLE_TO_THIS_CONVERSION_OP;
            complex_conversion_op.Rule = COMPLEX_MUTUAL_CONVERSION_OP | COMPLEX_TO_THAT_CONVERSION_OP | COMPLEX_TO_THIS_CONVERSION_OP;

            binary_operator.Rule = ADD_OP | SUB_OP | MUL_OP | DIV_OP | POW_OP;

            unit_expression_binary_operator.SetRuleOr(
                MUL_OP.Cast<UnitExpression.Binary.Operator>(),
                DIV_OP.Cast<UnitExpression.Binary.Operator>()
                );

            unary_operator.Rule = NEG_OP | POS_OP;

            expression.Rule = NUMBER | CONSTANT | external_variable | binary_expression | unary_expression;
            binary_expression.Rule = expression.BindMember(() => binary_expression._.Term1) + binary_operator.BindMember(() => binary_expression._.Op) + expression.BindMember(() => binary_expression._.Term2);
            unary_expression.Rule = LEFT_PAREN + expression + RIGHT_PAREN | unary_operator + expression;

            expression_with_unit.Rule = unit_variable | binary_expression_with_unit | binary_expression_with_unit2 | unary_expression_with_unit;
            binary_expression_with_unit.Rule = expression_with_unit + binary_operator + expression;
            binary_expression_with_unit2.Rule = expression + binary_operator + expression_with_unit;
            unary_expression_with_unit.Rule = LEFT_PAREN + expression_with_unit + RIGHT_PAREN | unary_operator + expression_with_unit;

            unit_variable.Rule = LEFT_BRACKET + unit_reference + RIGHT_BRACKET;

            external_variable.Rule = EXTERNAL_VARIABLE_PREFIX + qualified_identifier;

            #endregion

            LanguageFlags = LanguageFlags.CreateAst;
            BrowsableAstNodes = true;

#if true
            // these all should fail with compile error...

            @namespace.SetRuleOr(DECLARE + NAMESPACE +
                namespace_name.BindMember(() => @namespace._.Name) + definition.PlusList().BindMember(@namespace, () => @namespace._.Definitions));

            @namespace.SetRuleOr(DECLARE + NAMESPACE +
                namespace_name.BindMember(@namespace, () => @namespace._.Name) + definition.PlusList().BindMember(() => @namespace._.Definitions));

            @namespace.SetRuleOr(DECLARE + NAMESPACE +
                namespace_name.BindMember(@namespace, () => @namespace._.Name) + definition.PlusList().BindMember(namespace_usage, () => @namespace._.Definitions));

            @namespace.SetRuleOr(DECLARE + NAMESPACE +
                namespace_name.BindMember(namespace_usage, () => @namespace._.Name) + definition.PlusList().BindMember(namespace_usage, () => @namespace._.Definitions));

            @namespace.SetRuleOr(namespace_name.BindMember(namespace_usage, () => @namespace._.Name) + definition.PlusList().BindMember(namespace_usage, () => @namespace._.Definitions));

            conversion.SetRuleOr(simple_conversion, complex_conversion, unit_expression);

            definition.Rule = quantity_definition | unit_definition | prefix_definition | complex_conversion;
#endif
        }
    }
}

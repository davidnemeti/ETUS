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

            TypeForValue<string> IDENTIFIER = ToIdentifier("identifier")
                .CreateValue<string>((context, parseNode) => parseNode.FindTokenAndGetText());
            TypeForValue<object> NUMBER = ToNumber("number")
                .CreateValue<object>((context, parseNode) => parseNode.FindToken().Value);

            KeyTerm DOT = ToTerm(".");

            TypeForValue<string> qualified_identifier = IDENTIFIER.PlusList(DOT).ConvertValue(identifiers => string.Join(DOT.Text, identifiers));

            TypeForValue<Name> name = IDENTIFIER.ConvertValue(identifier => new Name { Value = identifier });
            TypeForValue<Name> namespace_name = qualified_identifier.ConvertValue(qual_id => new Name {Value = qual_id});
            TypeForValue<NameRef> nameref = qualified_identifier.ConvertValue(qual_id => new NameRef(qual_id));

            TypeForValue<Reference<QuantityDefinition>> quantity_reference = nameref.ConvertValue(nameRef => Reference.Get<QuantityDefinition>(nameRef));
            TypeForValue<Reference<UnitDefinition>> unit_reference = nameref.ConvertValue(nameRef => Reference.Get<UnitDefinition>(nameRef));

            var conversion = TypeForTransient.Of<Conversion>();
            var simple_conversion = TypeForBoundMembers.Of<SimpleConversion>();
            var complex_conversion = TypeForTransient.Of<ComplexConversion>();
            var complex_conversion_without_equal = TypeForBoundMembers.Of<ComplexConversion>();
            var complex_conversion_with_equal = TypeForBoundMembers.Of<ComplexConversion>();
            var simple_conversion_op = TypeForBoundMembers.Of<Direction>();
            var complex_conversion_op = TypeForBoundMembers.Of<Direction>();
            var unit_expression = TypeForTransient.Of<UnitExpression>();
            var binary_unit_expression = TypeForBoundMembers.Of<UnitExpression.Binary>();
            var square_unit_expression = TypeForBoundMembers.Of<UnitExpression.Square>();
            var cube_unit_expression = TypeForBoundMembers.Of<UnitExpression.Cube>();
            var recip_unit_expression = TypeForBoundMembers.Of<UnitExpression.Recip>();
            var unit_unit_expression = TypeForBoundMembers.Of<UnitExpression.Unit>();
            var expression = TypeForTransient.Of<Expression>();
            var binary_expression = TypeForBoundMembers.Of<Expression.Binary>();
            var number_expression = TypeForBoundMembers.Of<Expression.Number>();
            var unary_expression = TypeForBoundMembers.Of<Expression.Unary>();
            var expression_with_unit = TypeForTransient.Of<ExpressionWithUnit>();
            var binary_expression_with_unit = TypeForBoundMembers.Of<ExpressionWithUnit.Binary>();
            var binary_expression_with_unit2 = TypeForBoundMembers.Of<ExpressionWithUnit.Binary2>();
            var unary_expression_with_unit = TypeForBoundMembers.Of<ExpressionWithUnit.Unary>();
            var unit_variable_expression_with_unit = TypeForTransient.Of<ExpressionWithUnit.Unit>();
            var binary_operator = TypeForTransient.Of<BinaryOperator>();
            var unit_expression_binary_operator = TypeForTransient.Of<UnitExpression.Binary.Operator>();
            var unary_operator = TypeForTransient.Of<UnaryOperator>();
            var external_variable = TypeForBoundMembers.Of<Expression.ExternalVariable>();

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

            var CONSTANT = TypeForConstant.Of<Expression.Constant>();

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

            RegisterBracePair(LEFT_PAREN, RIGHT_PAREN);
            RegisterBracePair(LEFT_BRACKET, RIGHT_BRACKET);

            MarkPunctuation(LEFT_PAREN, RIGHT_PAREN, LEFT_BRACKET, RIGHT_BRACKET);

            #region Constants

            CONSTANT.Add("PI", Constants.PI);
            CONSTANT.Add("π", Constants.PI);

            #endregion

            #region Rules

            this.Root = group;

            group.Rule = namespace_usage.StarList().BindMember(group, t => t.NamespaceUsings) + @namespace.PlusList().BindMember(group, t => t.Namespaces);

            definition.SetRuleOr(quantity_definition, unit_definition, prefix_definition);

            namespace_usage.Rule = USE + NAMESPACE + nameref.BindMember(namespace_usage, t => t.NameRef);

            @namespace.Rule = DECLARE + NAMESPACE + namespace_name.BindMember(@namespace, t => t.Name) + definition.PlusList().BindMember(@namespace, t => t.Definitions);

            prefix_definition.Rule =
                DEFINE + PREFIX
                + name.BindMember(prefix_definition, t => t.Name)
                + expression.BindMember(prefix_definition, t => t.Factor);

            quantity_definition.Rule = DEFINE + QUANTITY + name.BindMember(quantity_definition, t => t.Name);

            unit_definition.Rule =
                DEFINE + UNIT
                + name.BindMember(unit_definition, t => t.Name)
                + OF
                + quantity_reference.BindMember(unit_definition, t => t.Quantity)
                + conversion.StarList().BindMember(unit_definition, t => t.Conversions)
                ;

            conversion.SetRuleOr(simple_conversion, complex_conversion);

            simple_conversion.Rule =
                simple_conversion_op.BindMember(simple_conversion, t => t.Direction)
                + expression.BindMember(simple_conversion, t => t.Factor)
                + unit_expression.BindMember(simple_conversion, t => t.OtherUnit)
                |
                simple_conversion_op.BindMember(simple_conversion, t => t.Direction)
                + unit_expression.BindMember(simple_conversion, t => t.OtherUnit)
                ;

            complex_conversion.Rule = complex_conversion_with_equal | complex_conversion_without_equal;

            complex_conversion_without_equal.Rule =
                complex_conversion_op.BindMember(complex_conversion, t => t.Direction)
                + expression_with_unit.BindMember(complex_conversion, t => t.Expr)
                ;

            complex_conversion_with_equal.Rule =
                complex_conversion_without_equal
                + PreferShiftHere()
                + EQUAL_STATEMENT + unit_variable_expression_with_unit.ConvertValue(unit_variable => unit_variable.Value).BindMember(complex_conversion, t => t.OtherUnit)
                ;

            unit_expression.SetRuleOr(
                binary_unit_expression,
                square_unit_expression,
                cube_unit_expression,
                recip_unit_expression,
                unit_unit_expression,
                LEFT_PAREN + unit_expression + RIGHT_PAREN
                );

            binary_unit_expression.Rule =
                unit_expression.BindMember(binary_unit_expression, t => t.Term1)
                + unit_expression_binary_operator.BindMember(binary_unit_expression, t => t.Op)
                + unit_expression.BindMember(binary_unit_expression, t => t.Term2);

            square_unit_expression.Rule =
                unit_expression.BindMember(square_unit_expression, t => t.Base)
                + POW_OP
                + ToTerm("2");

            cube_unit_expression.Rule =
                unit_expression.BindMember(cube_unit_expression, t => t.Base)
                + POW_OP
                + ToTerm("3");

            recip_unit_expression.Rule =
                ToTerm("1")
                + DIV_OP
                + unit_expression.BindMember(recip_unit_expression, t => t.Denominator);

            unit_unit_expression.Rule = nameref.ConvertValue(namerefForUnitDef => Reference.Get<UnitDefinition>(namerefForUnitDef)).BindMember(unit_unit_expression, t => t.Value);

            simple_conversion_op.Rule = SIMPLE_MUTUAL_CONVERSION_OP | SIMPLE_TO_THAT_CONVERSION_OP | SIMPLE_TO_THIS_CONVERSION_OP;
            complex_conversion_op.Rule = COMPLEX_MUTUAL_CONVERSION_OP | COMPLEX_TO_THAT_CONVERSION_OP | COMPLEX_TO_THIS_CONVERSION_OP;

            binary_operator.Rule = ADD_OP | SUB_OP | MUL_OP | DIV_OP | POW_OP;

            unit_expression_binary_operator.SetRuleOr(
                MUL_OP.Cast(unit_expression_binary_operator),
                DIV_OP.Cast(unit_expression_binary_operator)
                );

            unary_operator.Rule = NEG_OP | POS_OP;

            expression.SetRuleOr(number_expression, CONSTANT, external_variable, binary_expression, unary_expression, LEFT_PAREN + expression + RIGHT_PAREN);

            number_expression.Rule = NUMBER.ConvertValue(number => new Expression.Number(number));

            binary_expression.Rule =
                expression.BindMember(binary_expression, t => t.Term1)
                + binary_operator.BindMember(binary_expression, t => t.Op)
                + expression.BindMember(binary_expression, t => t.Term2);

            unary_expression.Rule = unary_operator.BindMember(unary_expression, t => t.Op) + expression.BindMember(unary_expression, t => t.Term);

            external_variable.Rule =
                EXTERNAL_VARIABLE_PREFIX
                + nameref.BindMember(external_variable, t => t.NameRef);

            expression_with_unit.SetRuleOr(
                unit_variable_expression_with_unit,
                binary_expression_with_unit,
                binary_expression_with_unit2,
                unary_expression_with_unit,
                LEFT_PAREN + expression_with_unit + RIGHT_PAREN
                );

            binary_expression_with_unit.Rule =
                expression_with_unit.BindMember(binary_expression_with_unit, t => t.Term1)
                + binary_operator.BindMember(binary_expression_with_unit, t => t.Op)
                + expression.BindMember(binary_expression_with_unit, t => t.Term2);

            binary_expression_with_unit2.Rule =
                expression.BindMember(binary_expression_with_unit2, t => t.Term1)
                + binary_operator.BindMember(binary_expression_with_unit2, t => t.Op)
                + expression_with_unit.BindMember(binary_expression_with_unit2, t => t.Term2);

            unary_expression_with_unit.Rule =
                unary_operator.BindMember(unary_expression_with_unit, t => t.Op)
                + expression_with_unit.BindMember(unary_expression_with_unit, t => t.Term);

            unit_variable_expression_with_unit.Rule =
                LEFT_BRACKET
                + unit_expression.BindMember(unit_variable_expression_with_unit, t => t.Value)
                + RIGHT_BRACKET;

            #endregion

            LanguageFlags = LanguageFlags.CreateAst;
//            BrowsableAstNodes = true;

#if false
            // these all should fail with compile error...

            binary_expression_with_unit2.Rule =
                expression.BindMember(binary_expression_with_unit2, t => t.Term2)
                + binary_operator.BindMember(binary_expression_with_unit2, t => t.Op)
                + expression_with_unit.BindMember(binary_expression_with_unit2, t => t.Term1);

            @namespace.SetRuleOr(DECLARE + NAMESPACE +
                namespace_name.BindMember(t => t.Name) + definition.PlusList().BindMember(@namespace, t => t.Definitions));

            @namespace.SetRuleOr(DECLARE + NAMESPACE +
                namespace_name.BindMember(@namespace, t => t.Name) + definition.PlusList().BindMember(t => t.Definitions));

            @namespace.SetRuleOr(DECLARE + NAMESPACE +
                namespace_name.BindMember(@namespace, t => t.Name) + definition.PlusList().BindMember(namespace_usage, t => t.Definitions));

            @namespace.SetRuleOr(DECLARE + NAMESPACE +
                namespace_name.BindMember(namespace_usage, t => t.Name) + definition.PlusList().BindMember(namespace_usage, t => t.Definitions));

            @namespace.SetRuleOr(namespace_name.BindMember(namespace_usage, t => t.Name) + definition.PlusList().BindMember(namespace_usage, t => t.Definitions));

            conversion.SetRuleOr(simple_conversion, complex_conversion, unit_expression);

            definition.Rule = quantity_definition | unit_definition | prefix_definition | complex_conversion;
#endif
        }
    }
}

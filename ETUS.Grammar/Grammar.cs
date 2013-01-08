﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

using Irony;
using Irony.Ast;
using Irony.Parsing;
using Irony.ITG;
using Irony.ITG.Ast;
using Irony.ITG.Unparsing;

using DomainCore;

using ETUS.DomainModel;
using ETUS.DomainModel.Expressions;

namespace ETUS.Grammar
{
    public class UDLGrammar : Irony.ITG.Ast.Grammar
    {
        public UDLGrammar()
            : base(AstCreation.CreateAst, EmptyCollectionHandling.ReturnNull, ErrorHandling.ThrowException)
        {
            var group = new BnfiTermType<Group>();
            var namespace_usage = new BnfiTermType<NamespaceUsing>();
            var @namespace = new BnfiTermType<Namespace>();
            var definitions = new BnfiTermCollection<List<Definition>, Definition>();
            var definition = new BnfiTermTransient<Definition>();
            var quantity_definition = new BnfiTermType<QuantityDefinition>();
            var prefix_definition = new BnfiTermType<PrefixDefinition>();
            var unit_definition = new BnfiTermType<UnitDefinition>();

            var conversions = new BnfiTermCollection<List<Conversion>, Conversion>();
            var conversion = new BnfiTermTransient<Conversion>();
            var simple_conversion = new BnfiTermType<SimpleConversion>();
            var complex_conversion = new BnfiTermTransient<ComplexConversion>();
            var complex_conversion_without_equal = new BnfiTermType<ComplexConversion>();
            var complex_conversion_with_equal = new BnfiTermType<ComplexConversion>();
            var simple_conversion_op = new BnfiTermTransient<Direction>();
            var complex_conversion_op = new BnfiTermTransient<Direction>();

            var unit_expression = new BnfiTermTransient<UnitExpression>();
            var binary_unit_expression = new BnfiTermType<UnitExpression.Binary>();
            var square_unit_expression = new BnfiTermType<UnitExpression.Square>();
            var cube_unit_expression = new BnfiTermType<UnitExpression.Cube>();
            var recip_unit_expression = new BnfiTermType<UnitExpression.Recip>();
            var unit_unit_expression = new BnfiTermType<UnitExpression.Unit>();

            var expression = new BnfiTermTransient<Expression>();
            var binary_expression = new BnfiTermType<Expression.Binary>();
            var number_expression = new BnfiTermValue<Expression.Number>();
            var unary_expression = new BnfiTermType<Expression.Unary>();

            var expression_with_unit = new BnfiTermTransient<ExpressionWithUnit>();
            var binary_expression_with_unit = new BnfiTermType<ExpressionWithUnit.Binary>();
            var binary_expression_with_unit2 = new BnfiTermType<ExpressionWithUnit.Binary2>();
            var unary_expression_with_unit = new BnfiTermType<ExpressionWithUnit.Unary>();
            var unit_variable_expression_with_unit = new BnfiTermType<ExpressionWithUnit.Unit>();

            var binary_operator = new BnfiTermTransient<BinaryOperator>();
            var unit_expression_binary_operator = new BnfiTermTransient<UnitExpression.Binary.Operator>();
            var unary_operator = new BnfiTermTransient<UnaryOperator>();
            var external_variable = new BnfiTermType<Expression.ExternalVariable>();

            var SIMPLE_MUTUAL_CONVERSION_OP = ToTerm("<=>", Direction.BiDir);
            var SIMPLE_TO_THIS_CONVERSION_OP = ToTerm("<=", Direction.From);
            var SIMPLE_TO_THAT_CONVERSION_OP = ToTerm("=>", Direction.To);
            var COMPLEX_MUTUAL_CONVERSION_OP = ToTerm("<:>", Direction.BiDir);
            var COMPLEX_TO_THIS_CONVERSION_OP = ToTerm("<:", Direction.From);
            var COMPLEX_TO_THAT_CONVERSION_OP = ToTerm(":>", Direction.To);

            var POS_OP = ToTerm("+", UnaryOperator.Pos);
            var NEG_OP = ToTerm("-", UnaryOperator.Neg);

            var ADD_OP = ToTerm("+", BinaryOperator.Add);
            var SUB_OP = ToTerm("-", BinaryOperator.Sub);
            var MUL_OP = ToTerm("*", BinaryOperator.Mul);
            var DIV_OP = ToTerm("/", BinaryOperator.Div);
            var POW_OP = ToTerm("^", BinaryOperator.Pow);

            var USE = ToTerm("use");
            var DECLARE = ToTerm("declare");
            var DEFINE = ToTerm("define");
            var PREFIX = ToTerm("prefix");
            var NAMESPACE = ToTerm("namespace");
            var QUANTITY = ToTerm("quantity");
            var UNIT = ToTerm("unit");
            var OF = ToTerm("of");
            var EXTERNAL_VARIABLE_PREFIX = ToTerm("::");
            var EQUAL_STATEMENT = ToTerm("=");
            var DOT = ToTerm(".");
            var LEFT_PAREN = ToPunctuation("(");
            var RIGHT_PAREN = ToPunctuation(")");
            var LEFT_BRACKET = ToPunctuation("[");
            var RIGHT_BRACKET = ToPunctuation("]");

            var NUMBER = CreateNumber();
            var IDENTIFIER = CreateIdentifier();

            var qualified_identifier = new BnfiTermValue<string>();

            var name = new BnfiTermValue<Name>();
            var namespace_name = new BnfiTermValue<Name>();
            var nameref = new BnfiTermValue<NameRef>();

            var quantity_reference = new BnfiTermValue<Reference<QuantityDefinition>>();
            var unit_reference = new BnfiTermValue<Reference<UnitDefinition>>();

            var CONSTANT = new BnfiTermConstant<Expression.Constant>()
            {
                { "PI", Constants.PI },
                { "π", Constants.PI }
            };

            RegisterOperators(20, ADD_OP, SUB_OP);
            RegisterOperators(30, MUL_OP, DIV_OP);
            RegisterOperators(40, NEG_OP, POS_OP);
            RegisterOperators(50, Associativity.Right, POW_OP);

            RegisterBracePair(LEFT_PAREN, RIGHT_PAREN);
            RegisterBracePair(LEFT_BRACKET, RIGHT_BRACKET);

            #region Rules

            this.Root = group;

            group.Rule =
                namespace_usage.StarList().BindMember(group, t => t.NamespaceUsings)
                + @namespace.PlusList().BindMember(group, t => t.Namespaces)
                ;

            namespace_usage.Rule =
                USE
                + NAMESPACE
                + nameref.BindMember(namespace_usage, t => t.NameRef)
                ;

            @namespace.Rule =
                DECLARE
                + NAMESPACE
                + namespace_name.BindMember(@namespace, t => t.Name)
                + definitions.BindMember(@namespace, t => t.Definitions)
                ;

            definitions.Rule = definition.PlusList();

            definition.SetRuleOr(
                prefix_definition,
                quantity_definition,
                unit_definition
                );

            prefix_definition.Rule =
                DEFINE
                + PREFIX
                + name.BindMember(prefix_definition, t => t.Name)
                + expression.BindMember(prefix_definition, t => t.Factor)
                ;

            quantity_definition.Rule =
                DEFINE
                + QUANTITY
                + name.BindMember(quantity_definition, t => t.Name)
                ;

            unit_definition.Rule =
                DEFINE + UNIT
                + name.BindMember(unit_definition, t => t.Name)
                + OF
                + quantity_reference.BindMember(unit_definition, t => t.Quantity)
                + conversions.BindMember(unit_definition, t => t.Conversions)
                ;

            conversions.Rule = conversion.StarList();

            conversion.SetRuleOr(
                simple_conversion,
                complex_conversion
                );

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

            BnfiTermValue<UnitExpression> xxx;

            complex_conversion_with_equal.Rule =
                complex_conversion_without_equal.Copy()
                + PreferShiftHere()
                + EQUAL_STATEMENT
                + (xxx = unit_variable_expression_with_unit.ConvertValue(unit_variable => unit_variable.Value)).BindMember(complex_conversion, t => t.OtherUnit)
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
                + POW_OP.NoAst()
                + ToTerm("2");

            cube_unit_expression.Rule =
                unit_expression.BindMember(cube_unit_expression, t => t.Base)
                + POW_OP.NoAst()
                + ToTerm("3");

            recip_unit_expression.Rule =
                ToTerm("1")
                + DIV_OP.NoAst()
                + unit_expression.BindMember(recip_unit_expression, t => t.Denominator);

            unit_unit_expression.Rule = unit_reference.BindMember(unit_unit_expression, t => t.Value);

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

            unary_expression.Rule = unary_operator.BindMember(unary_expression, t => t.Op)
                + expression.BindMember(unary_expression, t => t.Term);

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

            qualified_identifier.Rule = IDENTIFIER.PlusList(DOT).ConvertValue(identifiers => string.Join(DOT.Text, identifiers));

            name.Rule = IDENTIFIER.ConvertValue(identifier => new Name { Value = identifier });
            namespace_name.Rule = qualified_identifier.ConvertValue(qual_id => new Name { Value = qual_id });
            nameref.Rule = qualified_identifier.ConvertValue(qual_id => new NameRef(qual_id));

            quantity_reference.Rule = nameref.ConvertValue(nameRef => Reference.Get<QuantityDefinition>(nameRef));
            unit_reference.Rule = nameref.ConvertValue(nameRef => Reference.Get<UnitDefinition>(nameRef));

            #endregion

            #region Unparse

            name.UtokenizerForUnparse = obj => new Utoken[] { ((Name)obj).Value };
            namespace_name.UtokenizerForUnparse = obj => new Utoken[] { ((Name)obj).Value };
            nameref.UtokenizerForUnparse = obj => new Utoken[] { ((NameRef)obj).Value };
            number_expression.UtokenizerForUnparse = obj => new Utoken[] { ((Expression.Number)obj).Value.ToString() };
            quantity_reference.UtokenizerForUnparse = obj => new Utoken[] { ((Reference<QuantityDefinition>)obj).NameRef.Value };
            unit_reference.UtokenizerForUnparse = obj => new Utoken[] { ((Reference<UnitDefinition>)obj).NameRef.Value };
            xxx.InverseValueConverterForUnparse = obj => new ExpressionWithUnit.Unit { Value = ((UnitExpression)obj) };

            Formatting.InsertUtokensBefore(definitions, Utoken.EmptyLine);
            Formatting.InsertUtokensAfter(definition, Utoken.NewLine);
            Formatting.InsertUtokensBefore(conversions, Utoken.NewLine, Utoken.IncreaseIndentLevel);
            Formatting.InsertUtokensAfter(conversion, Utoken.NewLine);
            Formatting.InsertUtokensAfter(conversions, Utoken.NewLine, Utoken.DecreaseIndentLevel);

            #endregion

#if false
            // these all should fail with compile error...

            var xxx_transient = new BnfiTermTransient<ExpressionWithUnit.Unit>();
            xxx_transient.Rule =
                LEFT_BRACKET
                + unit_expression.BindMember(unit_variable_expression_with_unit, t => t.Value)
                + RIGHT_BRACKET;

            unit_expression.SetRuleOr(
                binary_unit_expression,
                square_unit_expression,
                cube_unit_expression,
                recip_unit_expression,
                unit_unit_expression,
                USE + unit_expression + DEFINE
                );

            unit_expression.SetRuleOr(USE + unit_expression + DEFINE);
            unit_expression.Rule = USE + unit_expression + DEFINE;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony;
using Irony.Ast;
using Irony.Parsing;
using ETUS.DomainModel2;
using ETUS.DomainModel2.Expressions;
using SchemaLanguage;
using System.Reflection;
using System.IO;

namespace ETUS.Grammar
{
    public class UDLGrammar : Irony.Parsing.Grammar
    {
        public UDLGrammar()
        {
            //Type _binaryExpression = typeof(Expression.Binary);
            //Type _expression = typeof(Expression);
            //Type _binaryOperator = typeof(BinaryOperator);

            NonTerminal package = new NonTerminal("package", typeof(Package));
            NonTerminal namespace_usage = new NonTerminal("namespace_usage");
            NonTerminal namespace_usages = new NonTerminal("namespace_usages");
            NonTerminal namespace_declaration = new NonTerminal("namespace");
            NonTerminal definitions = new NonTerminal("definitions");
            NonTerminal definition = new NonTerminal("definition");
            NonTerminal quantity_definition = new NonTerminal("quantity_definition");
            NonTerminal prefix_definition = new NonTerminal("prefix_definition");
            NonTerminal unit_definition = new NonTerminal("unit_definition");

            IdentifierTerminal identifier = new IdentifierTerminal("identifier");
            NonTerminal qualified_identifier = new NonTerminal("qualified_identifier");

            NonTerminal unit_name = new NonTerminal("unit_name");
            NonTerminal prefix_name = new NonTerminal("prefix_name");
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
            NonTerminalType binary_expression = new NonTerminalType(typeof(Expression.Binary));
            NonTerminal unary_expression = new NonTerminal("unary_expression");
            NonTerminal expression_with_unit = new NonTerminal("expression_with_units");
            NonTerminal binary_expression_with_unit = new NonTerminal("binary_expression_with_units");
            NonTerminal unary_expression_with_unit = new NonTerminal("unary_expression_with_units");
            NonTerminal unit_variable = new NonTerminal("unit_variable");
            NonTerminal binary_operator = new NonTerminal("binary_operator");
//            NonTerminalType binary_operator = new NonTerminalType(typeof(BinaryOperator));
            NonTerminal unary_operator = new NonTerminal("unary_operator");

            BnfTermProperty binary_Expression__expr1 = new BnfTermProperty(GetProperty(() => new Expression.Binary(null, null, null).Expr1), expression);
            BnfTermProperty binary_Expression__op = new BnfTermProperty(GetProperty(() => new Expression.Binary(null, null, null).Op), binary_operator);
            BnfTermProperty binary_Expression__expr2 = new BnfTermProperty(GetProperty(() => new Expression.Binary(null, null, null).Expr2), expression);

            NumberLiteral number = new NumberLiteral("number", NumberOptions.Default, (context, parseNode) => parseNode.AstNode = new Expression.DoubleNumber(Convert.ToDouble(parseNode.Token.Value)));
            ConstantTerminal constant = new ConstantTerminal("constant");
            NonTerminal external_variable = new NonTerminal("external_variable");

            MarkTransient(expression);

            KeyTerm dot = ToTerm(".");

            KeyTerm USE = ToTerm("use");
            KeyTerm DECLARE = ToTerm("declare");
            KeyTerm DEFINE = ToTerm("define");
            KeyTerm PREFIX = ToTerm("prefix");
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

//            TerminalType ADD_OP = new TerminalType(ToTerm("+"), typeof(BinaryOperator.Add));
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

            constant.Add("PI", Constant.PI);
            constant.Add("π", Constant.PI);

            #endregion

            #region Rules

//            this.Root = package;
            this.Root = binary_expression;

            package.Rule = namespace_usages + namespace_declaration + definitions;
//            package.Rule = namespace_declaration;

            package.AstConfig.NodeCreator = (astContext, parseTreeNode) => parseTreeNode.AstNode = new Package() { NamespaceDeclaration = (NamespaceDeclaration) parseTreeNode.ChildNodes[0].AstNode};

            namespace_usages.Rule = MakeStarRule(namespace_usages, namespace_usage);
            definitions.Rule = MakePlusRule(definitions, definition);
            definition.Rule = quantity_definition | unit_definition | prefix_definition;

            namespace_usage.Rule = USE + NAMESPACE + namespace_name;
            namespace_declaration.Rule = DECLARE + NAMESPACE + namespace_name;

            //BnfTermInRule id;
            //namespace_declaration.Rule = DECLARE + NAMESPACE + (id = identifier);
            //namespace_declaration.Rule = DECLARE + NAMESPACE + Bind(identifier, () => new NamespaceDeclaration().Name);
            //Bind<NamespaceDeclaration>(namespace_declaration);
            //Bind(id, () => new NamespaceDeclaration().Name);
            //Bind2(identifier => new NamespaceDeclaration().Name);

            namespace_declaration.AstConfig.NodeCreator = (astContext, parseTreeNode) => parseTreeNode.AstNode = new NamespaceDeclaration() { Name = new Name((string)parseTreeNode.ChildNodes[2].AstNode) };
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
                                            unit_expression + POW_OP + number;
            unary_unit_expression.Rule = LEFT_PAREN + unit_expression + RIGHT_PAREN;

            complex_conversion_expression.Rule = expression_with_unit | expression_with_unit + EQUAL_STATEMENT + unit_variable;

            simple_conversion_op.Rule = SIMPLE_MUTUAL_CONVERSION_OP | SIMPLE_TO_THAT_CONVERSION_OP | SIMPLE_TO_THIS_CONVERSION_OP;
            complex_conversion_op.Rule = COMPLEX_MUTUAL_CONVERSION_OP | COMPLEX_TO_THAT_CONVERSION_OP | COMPLEX_TO_THIS_CONVERSION_OP;

            binary_operator.AstConfig.NodeCreator = (astContext, parseTreeNode) => parseTreeNode.AstNode = parseTreeNode.ChildNodes[0].Token.ValueString == ADD_OP.Text ? new BinaryOperator.Add() : null;
            binary_operator.Rule = ADD_OP | SUB_OP | MUL_OP | DIV_OP | POW_OP;
            unary_operator.Rule = NEG_OP | POS_OP;

            var boo = ToTerm("boo");
            var soo = ToTerm("soo");
            MarkPunctuation(soo);
            expression.Rule = number | constant | external_variable | binary_expression | unary_expression | Empty;
            binary_expression.Rule = boo + binary_Expression__expr1 + binary_Expression__op + binary_Expression__expr2 |
                soo + binary_Expression__expr2 + binary_Expression__op + binary_Expression__expr1;
            unary_expression.Rule = LEFT_PAREN + expression + RIGHT_PAREN | unary_operator + expression;

            expression_with_unit.Rule = unit_variable | binary_expression_with_unit | unary_expression_with_unit;
            binary_expression_with_unit.Rule = expression + binary_operator + expression_with_unit | expression_with_unit + binary_operator + expression;
            unary_expression_with_unit.Rule = LEFT_PAREN + expression_with_unit + RIGHT_PAREN | unary_operator + expression_with_unit;

            unit_variable.Rule = LEFT_BRACKET + unit_name + RIGHT_BRACKET;

            qualified_identifier.Rule = MakePlusRule(qualified_identifier, dot, identifier);

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

        //BnfTermProperty Bind<TProperty>(BnfTerm element, System.Linq.Expressions.Expression<Func<TProperty>> expr)
        //{
        //    return new BnfTermProperty(element, GetProperty(expr));
        //}

        //Tuple<> Bind2<TProperty>(Expression<Func<BnfTermInRule, TProperty>> expr)
        //{
        //    return new BnfTermInRule(element, GetProperty(expr));
        //}

        void Bind<TType>(BnfTerm element)
            where TType : new()
        {
            element.AstConfig.NodeCreator = (astContext, parseTreeNode) => parseTreeNode.AstNode = new TType();
//            element.AstConfig.NodeCreator = (astContext, parseTreeNode) => parseTreeNode.AstNode = new TType() { Name = (string)parseTreeNode.ChildNodes[2].AstNode };
        }

        public static PropertyInfo GetProperty<T>(System.Linq.Expressions.Expression<Func<T>> expr)
        {
            var member = expr.Body as System.Linq.Expressions.MemberExpression;
            if (member == null)
                throw new InvalidOperationException("Expression is not a member access expression.");
            var property = member.Member as PropertyInfo;
            if (property == null)
                throw new InvalidOperationException("Member in expression is not a property.");
            return property;
        }
    }

    //class BnfTermProperty : BnfExpression
    //{
    //    readonly PropertyInfo propertyInfo;

    //    public BnfTermProperty(PropertyInfo propertyInfo, BnfTerm element)
    //        : base(element)
    //    {
    //        this.propertyInfo = propertyInfo;
    //    }
    //}

    //class BnfTermProperty : BnfTerm
    //{
    //    readonly PropertyInfo propertyInfo;
    //    public readonly BnfTerm bnfTerm;

    //    public BnfTermProperty(PropertyInfo propertyInfo, BnfTerm bnfTerm)
    //        : base(propertyInfo.Name)
    //    {
    //        this.propertyInfo = propertyInfo;
    //        this.bnfTerm = bnfTerm;
    //    }
    //}

    class BnfTermProperty : NonTerminal
    {
        public PropertyInfo PropertyInfo { get; private set; }
        public BnfTerm BnfTerm { get; private set; }

        public BnfTermProperty(PropertyInfo propertyInfo, BnfTerm bnfTerm)
            : base(string.Format("{0}.{1}", Helper.TypeNameWithDeclaringTypes(propertyInfo.DeclaringType), propertyInfo.Name.ToLower()), new BnfExpression(bnfTerm))
        {
            this.PropertyInfo = propertyInfo;
            this.BnfTerm = bnfTerm;
            this.Flags |= TermFlags.IsTransient | TermFlags.NoAstNode;
        }
    }

    public static class Helper
    {
        public static void ThrowGrammarError(GrammarErrorLevel grammarErrorLevel, string message, params object[] args)
        {
            if (args.Length > 0)
                message = string.Format(message, args);

            throw new GrammarErrorException(message, new GrammarError(grammarErrorLevel, null, message));
        }

        public static string TypeNameWithDeclaringTypes(Type type)
        {
            return type.IsNested
                ? string.Format("{0}_{1}", TypeNameWithDeclaringTypes(type.DeclaringType), type.Name.ToLower())
                : type.Name.ToLower();
        }

        public static string GetNonTerminalsAsText(LanguageData language, bool omitProperties = false)
        {
            var sw = new StringWriter();
            foreach (var nonTerminal in language.GrammarData.NonTerminals.OrderBy(nonTerminal => nonTerminal.Name))
            {
                if (omitProperties && nonTerminal is BnfTermProperty)
                    continue;

                sw.WriteLine("{0}{1}", nonTerminal.Name, nonTerminal.Flags.IsSet(TermFlags.IsNullable) ? "  (Nullable) " : string.Empty);
                foreach (Production pr in nonTerminal.Productions)
                {
                    sw.WriteLine("   {0}", ProductionToString(pr, omitProperties));
                }
            }
            return sw.ToString();
        }

        private static string ProductionToString(Production production, bool omitProperties)
        {
            var sw = new StringWriter();
            sw.Write("{0} -> ", production.LValue.Name);
            foreach (BnfTerm bnfTerm in production.RValues)
            {
                BnfTerm bnfTermToWrite = omitProperties && bnfTerm is BnfTermProperty
                    ? ((BnfTermProperty)bnfTerm).BnfTerm
                    : bnfTerm;

                sw.Write("{0} ", bnfTermToWrite.Name);
            }
            return sw.ToString();
        }
    }

    class NonTerminalType : NonTerminal
    {
        readonly Type type;

        //IDictionary<int, PropertyInfo> parseTreeChildIndexToProperty = new Dictionary<int, PropertyInfo>();
        //ISet<BnfTerm> bnfTermsPunctuationOrEmptyTransient = new HashSet<BnfTerm>();

        //void NonTerminalType_Reduced(object sender, ReducedEventArgs e)
        //{
        //    int parseTreeChildIndex = 0;
        //    foreach (BnfTerm bnfTerm in e.ReducedProduction.RValues)
        //    {
        //        // NOTE: we can recognize empty transient terms only by using bnfTermsPunctuationOrEmptyTransient (since they were eliminated earlier)
        //        if (bnfTerm.Flags.IsSet(TermFlags.IsPunctuation) || bnfTermsPunctuationOrEmptyTransient.Contains(bnfTerm))
        //            continue;

        //        if (bnfTerm is BnfTermProperty)
        //        {
        //            BnfTermProperty bnfTermProperty = (BnfTermProperty)bnfTerm;
        //            parseTreeChildIndexToProperty[parseTreeChildIndex] = bnfTermProperty.PropertyInfo;
        //        }

        //        parseTreeChildIndex++;
        //    }
        //    bnfTermsPunctuationOrEmptyTransient.Clear();
        //}

        public NonTerminalType(Type type)
            : base(Helper.TypeNameWithDeclaringTypes(type))
        {
            this.type = type;
//            this.Reduced += NonTerminalType_Reduced;
        }

        public new BnfExpression Rule
        {
            get { return base.Rule; }
            set
            {
                AstConfig.NodeCreator = (context, parseTreeNode) =>
                    {
                        parseTreeNode.AstNode = Activator.CreateInstance(type);

                        foreach (var parseTreeChild in parseTreeNode.ChildNodes)
                        {
                            PropertyInfo propertyInfo = (PropertyInfo)parseTreeChild.Tag;
                            if (propertyInfo != null)
                            {
                                propertyInfo.SetValue(parseTreeNode.AstNode, parseTreeChild.AstNode);
                            }
                            else if (!parseTreeChild.Term.Flags.IsSet(TermFlags.NoAstNode))
                            {
                                // NOTE: we shouldn't get here since the Rule setter should have handle this kind of error
                                context.AddMessage(ErrorLevel.Warning, parseTreeChild.Token.Location, "No property assigned for term: {0}", parseTreeChild.Term);
                            }
                        }

                        //foreach (var parseTreeChild in parseTreeNode.ChildNodes.Select((parseTreeChild, parseTreeChildIndex) =>
                        //    new { Value = parseTreeChild, Index = parseTreeChildIndex }))
                        //{
                        //    PropertyInfo propertyInfo;
                        //    if (parseTreeChildIndexToProperty.TryGetValue(parseTreeChild.Index, out propertyInfo))
                        //    {
                        //        propertyInfo.SetValue(parseTreeNode.AstNode, parseTreeChild.Value.AstNode);
                        //    }
                        //    else if (!parseTreeChild.Value.Term.Flags.IsSet(TermFlags.NoAstNode))
                        //    {
                        //        context.AddMessage(ErrorLevel.Warning, parseTreeChild.Value.Token.Location, "No property assigned for term: {0}", parseTreeChild.Value.Term);
                        //    }
                        //}

                        //parseTreeChildIndexToProperty.Clear();
                    };

                foreach (var bnfTermList in value.Data)
                {
                    foreach (var bnfTerm in bnfTermList)
                    {
                        if (bnfTerm is BnfTermProperty)
                            ((BnfTermProperty)bnfTerm).Reduced += nonTerminal_Reduced;
                        else if (!bnfTerm.Flags.IsSet(TermFlags.NoAstNode))
                            Helper.ThrowGrammarError(GrammarErrorLevel.Error, "No property assigned for term: {0}", bnfTerm);
                    }
                }

                base.Rule = value;      // NOTE: this must come after setting AstConfig.NodeCreator
            }
        }

        void nonTerminal_Reduced(object sender, ReducedEventArgs e)
        {
            e.ResultNode.Tag = ((BnfTermProperty)sender).PropertyInfo;
            //if (e.ResultNode.IsPunctuationOrEmptyTransient())
            //    bnfTermsPunctuationOrEmptyTransient.Add(e.ResultNode.Term);
        }
    }

    class TerminalType : BnfTerm
    {
        readonly Terminal terminal;

        public TerminalType(Terminal terminal, Type type)
            : base(type.Name, null, type)
        {
        }
    }
}

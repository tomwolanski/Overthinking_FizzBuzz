using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Linq;
using System.Collections.Immutable;
using System.CodeDom.Compiler;
using System.Globalization;
using System.IO;
using System.Text;

namespace FizzBuzz.SourceGenerator
{
    [Generator(LanguageNames.CSharp)]
    public class FizzBuzzGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput((ctx) =>
            {
                ctx.AddSource("ModuloOptionAttribute.g.cs", Consts.OptionAttrCode);
            });

            var methodCandidates = context.SyntaxProvider
                .ForAttributeWithMetadataName("FizzBuzzGenerator.ModuloOptionAttribute",
                (node, _) => node is MethodDeclarationSyntax sds && sds.Modifiers.Any(SyntaxKind.PartialKeyword),
                (ctx, _) =>
                {
                    var methodDeclaration = (MethodDeclarationSyntax)ctx.TargetNode;
                    var methodSymbol = (IMethodSymbol)ctx.TargetSymbol;
                    var typeSymbol = methodSymbol.ContainingType;

                    return GetUnionDefinition(methodSymbol, typeSymbol, methodDeclaration.GetLocation());
                });

            context.RegisterSourceOutput(methodCandidates, (ctx, def) =>
            {
                var code = Generator.CreateTypeSourceCode(def);
                ctx.AddSource($"{def.TypeName}.g.cs", code);
            });
        }


        private static TypeDefinition GetUnionDefinition(IMethodSymbol methodSymbol, INamedTypeSymbol typeSymbol, Location? location)
        {
            var options = methodSymbol.GetAttributes()
                .Where(a => a.AttributeClass?.Name == "ModuloOptionAttribute")
                .Select(a =>
                {
                    var loc = a.ApplicationSyntaxReference.GetSyntax().GetLocation();
                    var divider = (int)a.ConstructorArguments.First().Value;
                    var text = (string)a.ConstructorArguments.Last().Value;

                    return new OptionDefinition(divider, text, loc);
                })
                .ToImmutableArray();

            return new TypeDefinition(
                typeSymbol.ContainingNamespace.IsGlobalNamespace
                    ? null
                    : typeSymbol.ContainingNamespace.ToDisplayString(),
                typeSymbol.Name,
                methodSymbol.Name,
                options,
                location);
        }
    }

    record TypeDefinition(string? Namespace, string TypeName, string MethodName, ImmutableArray<OptionDefinition> Options, Location? Location);

    public record OptionDefinition(int Divider, string Text, Location Location);
}



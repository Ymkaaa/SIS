using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.IO;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Text;
using SIS.WebServer.ViewEngine;
using System.Text.RegularExpressions;

namespace SIS.WebServer.ViewEngine
{
    public class ViewEngine : IViewEngine
    {
        public string Execute<T>(string viewContent, T model)
        {
            string csharpHtmlCode = this.GetCSharpCode(viewContent);
            string code = $@"
            using System;
            using System.Linq;
            using System.Text;
            using System.Collections.Generic;
            using SIS.WebServer.ViewEngine;
            
            namespace AppViewCodeNamespace
            {{
                public class AppViewCode : IView
                {{
                    public string GetHtml(object model)
                    {{
                        var Model = model as {model.GetType().FullName};
                        StringBuilder html = new StringBuilder();
                        
                        {csharpHtmlCode}

                        return html.ToString();
                    }}
                }}
            }}
            ";
            
            IView view = CompileAndInstance(code, model.GetType().Assembly);
            string htmlResult = view?.GetHtml(model);

            return htmlResult;
        }

        private string GetCSharpCode(string viewContent)
        {
            StringBuilder cSharpCode = new StringBuilder();
            string[] lines = viewContent.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            string[] supportedOperators = new[] { "for", "if", "else" };

            foreach (string line in lines)
            {
                string cSharpLine = string.Empty;

                if (line.TrimStart().StartsWith("{") || line.TrimStart().StartsWith("}"))
                {
                    cSharpLine = line;
                }
                else if (supportedOperators.Any(x => line.TrimStart().StartsWith($"@{x}")))
                {
                    int atSignIndex = line.IndexOf("@");
                    cSharpLine = line.Remove(atSignIndex, 1);
                }
                else
                {
                    if (!line.Contains("@"))
                    {
                        cSharpLine = $"html.AppendLine(@\"{line.Replace("\"", "\"\"")}\");";
                    }
                    else
                    {
                        string cSharpStringToAppend = "html.AppendLine(@\"";
                        string restOfLine = line;

                        while (restOfLine.Contains("@"))
                        {
                            int atSignIndex = restOfLine.IndexOf("@");
                            string plainText = restOfLine.Substring(0, atSignIndex);

                            Regex cSharpCodeRegex = new Regex(@"[^\s<\""]+", RegexOptions.Compiled);
                            string cSharpExpression = cSharpCodeRegex.Match(restOfLine.Substring(atSignIndex + 1))?.Value;

                            cSharpStringToAppend += plainText + "\" + " + cSharpExpression + " + @\"";

                            if (restOfLine.Length <= atSignIndex + cSharpExpression.Length + 1)
                            {
                                restOfLine = string.Empty;
                            }
                            else
                            {
                                restOfLine = restOfLine.Substring(atSignIndex + cSharpExpression.Length + 1);
                            }
                        }

                        cSharpStringToAppend += $"{restOfLine}\");";
                        cSharpLine = cSharpStringToAppend;
                    }
                }

                cSharpCode.AppendLine(cSharpLine);
            }


            return cSharpCode.ToString();
        }

        private IView CompileAndInstance(string code, Assembly modelAssembly)
        {
            CSharpCompilation compilation = CSharpCompilation.Create("AppViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(modelAssembly.Location));

            AssemblyName[] netStandardAssemblies = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();
            foreach (AssemblyName name in netStandardAssemblies)
            {
                compilation = compilation.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(name).Location));
            }

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                EmitResult compilationResult = compilation.Emit(memoryStream);

                if (!compilationResult.Success)
                {
                    foreach (Diagnostic error in compilationResult.Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error || x.Severity == DiagnosticSeverity.Warning))
                    {
                        Console.WriteLine(error.GetMessage());
                    }

                    return null;
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                byte[] assemblyBytes = memoryStream.ToArray();

                Assembly assembly = Assembly.Load(assemblyBytes);

                Type type = assembly.GetType("AppViewCodeNamespace.AppViewCode");
                if (type == null)
                {
                    Console.WriteLine("AppViewCode not found.");
                    return null;
                }

                Object instance = Activator.CreateInstance(type);
                if (instance == null)
                {
                    Console.WriteLine("AppViewCode cannot be instanciated.");
                    return null;
                }

                return instance as IView;
            }
        }
    }
}

using System.IO;
using System.Linq;
using System.Reflection;
using System;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Collections;
using SIS.MvcFramework.Identity;

namespace SIS.MvcFramework.ViewEngineX
{
    public class ViewEngine : IViewEngine
    {
        private string GetModelType<T>(T model)
        {
            if (model is IEnumerable)
            {
                return $"IEnumerable<{model.GetType().GetGenericArguments()[0].FullName}>";
            }

            return model.GetType().FullName;
        }

        public string Execute<T>(string viewContent, T model, Principal user = null)
        {
            string csharpHtmlCode = this.GetCSharpCode(viewContent);
            string code = $@"
            using System;
            using System.Linq;
            using System.Text;
            using System.Collections.Generic;
            using SIS.MvcFramework.ViewEngineX;
            using System.Net;
            using SIS.MvcFramework.Identity;

            namespace AppViewCodeNamespace
            {{
                public class AppViewCode : IView
                {{
                    public string GetHtml(object model, Principal user)
                    {{
                        var Model = {(model == null ? "new {}" : $"model as {GetModelType(model)}")};
                        var User = user;

                        StringBuilder html = new StringBuilder();

                        {csharpHtmlCode}

                        return html.ToString();
                    }}
                }}
            }}
            ";

            IView view = CompileAndInstance(code, model?.GetType().Assembly);
            string htmlResult = view?.GetHtml(model, user);

            return htmlResult;
        }

        private string GetCSharpCode(string viewContent)
        {
            var lines = viewContent.Split(new string[] { "\r\n", "\n\r", "\n" }, StringSplitOptions.None);
            var csharpCode = new StringBuilder();
            var supportedOperators = new[] { "for", "if", "else" };
            var csharpCodeRegex = new Regex(@"[^\s<""\&]+", RegexOptions.Compiled);
            var csharpCodeDepth = 0; // If > 0, Inside CSharp Syntax
        
            foreach (var line in lines)
            {
                string currentLine = line;
        
                if (currentLine.TrimStart().StartsWith("@{"))
                {
                    csharpCodeDepth++;
                }
                else if (currentLine.TrimStart().StartsWith("{") || currentLine.TrimStart().StartsWith("}"))
                {
                    // { / }
                    if (csharpCodeDepth > 0)
                    {
                        if (currentLine.TrimStart().StartsWith("{"))
                        {
                            csharpCodeDepth++;
                        }
                        else if (currentLine.TrimStart().StartsWith("}"))
                        {
                            if ((--csharpCodeDepth) == 0)
                            {
                                continue;
                            }
                        }
                    }
        
                    csharpCode.AppendLine(currentLine);
                }
                else if (csharpCodeDepth > 0)
                {
                    csharpCode.AppendLine(currentLine);
                    continue;
                }
                else if (supportedOperators.Any(x => currentLine.TrimStart().StartsWith("@" + x)))
                {
                    // @C#
                    var atSignLocation = currentLine.IndexOf("@");
                    var csharpLine = currentLine.Remove(atSignLocation, 1);
                    csharpCode.AppendLine(csharpLine);
                }
                else
                {
                    // HTML
                    if (currentLine.Contains("@RenderBody()"))
                    {
                        var csharpLine = $"html.AppendLine(@\"{currentLine}\");";
                        csharpCode.AppendLine(csharpLine);
                    }
                    else
                    {
                        var csharpStringToAppend = "html.AppendLine(@\"";
                        var restOfLine = currentLine;
                        while (restOfLine.Contains("@"))
                        {
                            var atSignLocation = restOfLine.IndexOf("@");
                            var plainText = restOfLine.Substring(0, atSignLocation).Replace("\"", "\"\"");
                            var csharpExpression = csharpCodeRegex.Match(restOfLine.Substring(atSignLocation + 1))?.Value;
        
                            if (csharpExpression.Contains("{") && csharpExpression.Contains("}"))
                            {
                                var csharpInlineExpression =
                                    csharpExpression.Substring(1, csharpExpression.IndexOf("}") - 1);
        
                                csharpStringToAppend += plainText + "\" + " + csharpInlineExpression + " + @\"";
        
                                csharpExpression = csharpExpression.Substring(0, csharpExpression.IndexOf("}") + 1);
                            }
                            else
                            {
                                csharpStringToAppend += plainText + "\" + " + csharpExpression + " + @\"";
                            }
        
                            if (restOfLine.Length <= atSignLocation + csharpExpression.Length + 1)
                            {
                                restOfLine = string.Empty;
                            }
                            else
                            {
                                restOfLine = restOfLine.Substring(atSignLocation + csharpExpression.Length + 1);
                            }
                        }
        
                        csharpStringToAppend += $"{restOfLine.Replace("\"", "\"\"")}\");";
                        csharpCode.AppendLine(csharpStringToAppend);
                    }
                }
            }
        
            return csharpCode.ToString();
        }

        //private string GetCSharpCode(string viewContent)
        //{
        //    StringBuilder cSharpCode = new StringBuilder();
        //    string[] lines = viewContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        //    string[] supportedOperators = new[] { "for", "if", "else" };

        //    foreach (string line in lines)
        //    {
        //        string cSharpLine = string.Empty;

        //        if (line.TrimStart().StartsWith("{") || line.TrimStart().StartsWith("}"))
        //        {
        //            cSharpLine = line;
        //        }
        //        else if (supportedOperators.Any(x => line.TrimStart().StartsWith($"@{x}")))
        //        {
        //            int atSignIndex = line.IndexOf("@");
        //            cSharpLine = line.Remove(atSignIndex, 1);
        //        }
        //        else
        //        {
        //            if (!line.Contains("@"))
        //            {
        //                cSharpLine = $"html.AppendLine(@\"{line.Replace("\"", "\"\"")}\");";
        //            }
        //            else if (line.Contains("@RenderBody()"))
        //            {
        //                cSharpLine = $"html.AppendLine(@\"{line}\");";
        //            }
        //            else
        //            {
        //                string cSharpStringToAppend = "html.AppendLine(@\"";
        //                string restOfLine = line;

        //                while (restOfLine.Contains("@"))
        //                {
        //                    int atSignIndex = restOfLine.IndexOf("@");
        //                    string plainText = restOfLine.Substring(0, atSignIndex);

        //                    Regex cSharpCodeRegex = new Regex(@"[^\s<\""]+", RegexOptions.Compiled);
        //                    string cSharpExpression = cSharpCodeRegex.Match(restOfLine.Substring(atSignIndex + 1))?.Value;

        //                    cSharpStringToAppend += plainText.Replace("\"", "\"\"") + "\" + " + cSharpExpression + " + @\"";

        //                    if (restOfLine.Length <= atSignIndex + cSharpExpression.Length + 1)
        //                    {
        //                        restOfLine = string.Empty;
        //                    }
        //                    else
        //                    {
        //                        restOfLine = restOfLine.Substring(atSignIndex + cSharpExpression.Length + 1);
        //                    }
        //                }

        //                cSharpStringToAppend += $"{restOfLine}\");";
        //                cSharpLine = cSharpStringToAppend;
        //            }
        //        }

        //        cSharpCode.AppendLine(cSharpLine);
        //    }

        //    return cSharpCode.ToString();
        //}

        private IView CompileAndInstance(string code, Assembly modelAssembly)
        {
            modelAssembly = modelAssembly ?? Assembly.GetEntryAssembly();

            CSharpCompilation compilation = CSharpCompilation.Create("AppViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(Assembly.GetEntryAssembly().Location))
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

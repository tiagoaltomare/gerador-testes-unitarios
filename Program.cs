using System;
using System.Reflection;
using System.IO;
using System.Net;
using System.Text;

namespace RoboTestesAutomatizados
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!Directory.Exists(Configuracoes.pastaEntrada))
            {
                Console.WriteLine($"[ERRO] Crie a pasta {Configuracoes.pastaEntrada} e coloque as dlls para leitura nela depois execute novamente a aplicação.");
                Environment.Exit(0);
            }
            string[] fileNames = Directory.GetFiles(Configuracoes.pastaEntrada);
            
            foreach(string fileName in fileNames)
            {
                if(fileName.Contains("dll"))
                {
                    Console.WriteLine("Loading DLL '{0}'", fileName);
                    Assembly assembly = Assembly.LoadFrom(fileName);
                    Type[] types = assembly.GetTypes();

                    foreach(Type type in types)
                    {
                        if(!Directory.Exists(Configuracoes.pastaSaida))
                        {
                            Console.WriteLine($"[ERRO] Crie a pasta {Configuracoes.pastaSaida} e execute novamente a aplicação.");
                            Environment.Exit(0);
                        }

                        using(StreamWriter sw = new StreamWriter($"{Configuracoes.pastaSaida}{type.Name}UnitTest.cs"))
                        {
                            sw.WriteLine(Configuracoes.baseClass);
                            sw.WriteLine($"namespace {type.Namespace}.TesteUnitario{Environment.NewLine}{{");
                            sw.WriteLine($"[TestClass]{Environment.NewLine}public class {type.Name}UnitTest{Environment.NewLine}{{");

                            PropertyInfo[] propertyInfos;
                            propertyInfos = type.GetProperties();
                            string testConstructorOut = Configuracoes.testConstructor;
                            testConstructorOut = testConstructorOut.Replace("CLASS_NAME",type.Name);
                            string auxiliar = "";

                            foreach (PropertyInfo propertyInfo in propertyInfos)
                            {
                                string constructorPropertiesOut = Configuracoes.constructorProperties;
                                constructorPropertiesOut = constructorPropertiesOut.Replace("PROPERTIE",propertyInfo.Name);
                                constructorPropertiesOut = constructorPropertiesOut.Replace("VALUE_TYPE", ReturnValueType(propertyInfo.PropertyType));
                                auxiliar = $"{auxiliar}{constructorPropertiesOut};";
                            }
                            testConstructorOut = testConstructorOut.Replace("PROPERTIES", auxiliar);
                            sw.WriteLine(testConstructorOut);
                            Console.WriteLine("Construtor Gerado");

                            foreach (PropertyInfo propertyInfo in propertyInfos)
                            {
                                string testGetSetOut = Configuracoes.testGetSet;
                                testGetSetOut = testGetSetOut.Replace("NAME_PARAMETER",propertyInfo.Name);
                                testGetSetOut = testGetSetOut.Replace("VALUE_TYPE", ReturnValueType(propertyInfo.PropertyType));
                                testGetSetOut = testGetSetOut.Replace("NAME_CLASS", type.Name);
                                sw.WriteLine(testGetSetOut);
                            }

                            Console.WriteLine("Gets e Sets Gerados");

                            MethodInfo[] methodInfos;
                            methodInfos = type.GetMethods();

                            foreach (MethodInfo methodInfo in methodInfos)
                            {
                                if(!methodInfo.Name.Contains("get")
                                    && !methodInfo.Name.Contains("set")
                                    && !methodInfo.Name.Contains("ToString") 
                                    && !methodInfo.Name.Contains("Equals")
                                    && !methodInfo.Name.Contains("GetHashCode")
                                    && !methodInfo.Name.Contains("GetType")
                                )
                                {
                                    string testMethodOut = Configuracoes.testMethod;
                                    string inputParameters = "";

                                    testMethodOut = testMethodOut.Replace("RETURN_PARAMETER", ValueTypeToSTring(methodInfo.ReturnParameter.ParameterType));
                                    testMethodOut = testMethodOut.Replace("NAME_METHOD", methodInfo.Name);
                                    testMethodOut = testMethodOut.Replace("NAME_CLASS", type.Name);;

                                    ParameterInfo[] parameters;
                                    parameters = methodInfo.GetParameters();

                                    foreach (ParameterInfo parameter in parameters)
                                    {
                                        inputParameters = $"{inputParameters}{ReturnValueType(parameter.ParameterType)},";
                                    }

                                    if(parameters.Length > 0)
                                        inputParameters = inputParameters.Remove(inputParameters.Length -1);
                                        
                                    testMethodOut = testMethodOut.Replace("PARAMETERS", inputParameters);
                                    testMethodOut = testMethodOut.Replace("RETURN_TYPE_VALUE_ASSERTION", ReturnValueType(methodInfo.ReturnParameter.ParameterType));

                                    sw.WriteLine(testMethodOut);
                                }
                            }
                            Console.WriteLine("Métodos Gerados");
                            sw.WriteLine("}}");
                        }
                    }
                }
            }

            Console.WriteLine("Processo Finalizado");
        }

        static string ReturnValueType(Type propertytype)
        {
            switch (Type.GetTypeCode(propertytype))
            {
                case TypeCode.Int32:
                    return "1";
                case TypeCode.Int64:
                    return "1000";
                case TypeCode.String:
                    return "\"abc\"";      
                case TypeCode.Boolean:
                    return "true";
                case TypeCode.Decimal:
                    return "10.0";
                case TypeCode.DateTime:
                    return "DateTime.Now";
                default:
                    return "Object";
            }
        }

        static string ValueTypeToSTring(Type returnParameter)
        {
            switch (Type.GetTypeCode(returnParameter))
            {
                case TypeCode.Int32:
                    return "int";
                case TypeCode.Int64:
                    return "long";
                case TypeCode.String:
                    return "string";      
                case TypeCode.Boolean:
                    return "bool";
                case TypeCode.Decimal:
                    return "decimal";
                case TypeCode.DateTime:
                    return "DateTime";
                default:
                    return "Object";
            }
        }
    }
}

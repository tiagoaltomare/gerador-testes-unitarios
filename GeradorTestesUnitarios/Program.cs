using System;
using System.Reflection;
using System.IO;
using System.Net;
using System.Text;

namespace GeradorTestesUnitarios
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!Directory.Exists(Configuracoes.pastaEntrada))
            {
                Console.WriteLine("[ERRO] Crie a pasta "+Configuracoes.pastaEntrada+" e coloque as dlls para leitura nela depois execute novamente a aplicação.");
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
                        var typeName = type.Name.Replace("\\", "").Replace("/", "").Replace("|", "").Replace("<", "").Replace(">", "").Replace("*", "").Replace(":", "");

                        if (!Directory.Exists(Configuracoes.pastaSaida))
                        {
                            Console.WriteLine("[ERRO] Crie a pasta " + Configuracoes.pastaSaida + " e execute novamente a aplicação.");
                            Environment.Exit(0);
                        }
                        else
                        {
                            if (!Directory.Exists(Configuracoes.pastaSaida + "\\" + type.Namespace))
                            {
                                Directory.CreateDirectory(Configuracoes.pastaSaida + "\\" + type.Namespace);
                            }
                        }

                        using (StreamWriter sw = new StreamWriter(Configuracoes.pastaSaida + "\\" + type.Namespace + "\\" + typeName + "UnitTest.cs"))
                        {
                            sw.WriteLine(Configuracoes.baseClass);
                            sw.WriteLine("namespace "+ type.Namespace +".TesteUnitario"+Environment.NewLine+"{");
                            sw.WriteLine("[ExcludeFromCodeCoverage]\r\n[TestClass]\r\npublic class "+type.Name+"Tests"+"\r\n"+"{" +"\r\n"+ typeName + " objTest;\r\n DateTime data = new DateTime(2018,10,31);\r\n");

                            PropertyInfo[] propertyInfos;
                            propertyInfos = type.GetProperties();
                            string testConstructorOut = Configuracoes.testConstructor;
                            testConstructorOut = testConstructorOut.Replace("NEW_OBJECT", type.Name).Replace("PROPERTIES", "");
                            sw.WriteLine(testConstructorOut);
                            Console.WriteLine("Construtor Gerado");

                            foreach (PropertyInfo propertyInfo in propertyInfos)
                            {
                                string valueType = ReturnValueType(propertyInfo.PropertyType);
                                string testGetSetOut = Configuracoes.testGetSet;
                                testGetSetOut = testGetSetOut.Replace("NAME_PARAMETER",propertyInfo.Name);
                                testGetSetOut = testGetSetOut.Replace("VALUE_TYPE", valueType);
                                testGetSetOut = testGetSetOut.Replace("NAME_CLASS", type.Name);

                                string assert = string.Empty;
                                switch (Type.GetTypeCode(propertyInfo.PropertyType))
                                {
                                    case TypeCode.Object:
                                        assert = Configuracoes.assertInstance;
                                        if (!propertyInfo.PropertyType.Name.Contains("Nullable"))
                                        {
                                            if (propertyInfo.PropertyType.FullName != null)
                                            {
                                                if (propertyInfo.PropertyType.FullName?.Contains("ICollection") == true)
                                                {
                                                    testGetSetOut = testGetSetOut.Replace("ASSERT", assert.Replace("NAME_PARAMETER", propertyInfo.Name).Replace("VALUE_TYPE", valueType.Replace("new ", "").Replace("()", "")));
                                                }
                                                else
                                                {
                                                    testGetSetOut = testGetSetOut.Replace("ASSERT", assert.Replace("NAME_PARAMETER", propertyInfo.Name).Replace("VALUE_TYPE", valueType.Replace("new ", "").Replace("()", "")));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            assert = Configuracoes.assertTrue;
                                            if(valueType.Equals("new DateTime(2018,10,31)"))
                                            {
                                                testGetSetOut = testGetSetOut.Replace("ASSERT", assert.Replace("NAME_PARAMETER", propertyInfo.Name).Replace("VALUE_TYPE", "data"));
                                            }
                                            else
                                            {
                                                testGetSetOut = testGetSetOut.Replace("ASSERT", assert.Replace("NAME_PARAMETER", propertyInfo.Name).Replace("VALUE_TYPE", valueType));
                                            }
                                        }
                                        break;
                                    default:
                                        assert = Configuracoes.assertTrue;
                                        if (valueType.Equals("new DateTime(2018,10,31)"))
                                        {
                                            testGetSetOut = testGetSetOut.Replace("ASSERT", assert.Replace("NAME_PARAMETER", propertyInfo.Name).Replace("VALUE_TYPE", "data"));
                                        }
                                        else
                                        {
                                            testGetSetOut = testGetSetOut.Replace("ASSERT", assert.Replace("NAME_PARAMETER", propertyInfo.Name).Replace("VALUE_TYPE", valueType));
                                        }
                                        break;
                                }

                                sw.WriteLine(testGetSetOut);
                            }

                            Console.WriteLine("Gets e Sets Gerados");

                            MethodInfo[] methodInfos;
                            methodInfos = type.GetMethods();

                            foreach (MethodInfo methodInfo in methodInfos)
                            {
                                if (!methodInfo.Name.Contains("get")
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
                                    testMethodOut = testMethodOut.Replace("NAME_CLASS", type.Name); ;

                                    ParameterInfo[] parameters;
                                    parameters = methodInfo.GetParameters();

                                    foreach (ParameterInfo parameter in parameters)
                                    {
                                        inputParameters = inputParameters + ReturnValueType(parameter.ParameterType) + ",";
                                    }

                                    if (parameters.Length > 0)
                                        inputParameters = inputParameters.Remove(inputParameters.Length - 1);

                                    testMethodOut = testMethodOut.Replace("PARAMETERS", inputParameters);
                                    testMethodOut = testMethodOut.Replace("RETURN_TYPE_VALUE_ASSERTION", ReturnValueType(methodInfo.ReturnParameter.ParameterType));

                                    sw.WriteLine(testMethodOut);
                                }
                            }
                            Console.WriteLine("Métodos Gerados");

                            sw.WriteLine("}"+"\r\n"+"}");
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
                case TypeCode.Empty:
                    return "";
                case TypeCode.Object:
                    if(propertytype.Name.Contains("Nullable"))
                    {
                        if (propertytype.FullName.Contains("Int"))
                        {
                            return "1";
                        }
                        else if (propertytype.FullName.Contains("DateTime"))
                        {
                            return "new DateTime(2018,10,31)";
                        }
                        else if (propertytype.FullName.Contains("Byte"))
                        {
                            return "1";
                        }
                        else if (propertytype.FullName.Contains("Boolean"))
                        {
                            return "true";
                        }
                        else if (propertytype.FullName.Contains("Decimal"))
                        {
                            return "10";
                        }
                        else if (propertytype.FullName.Contains("Double"))
                        {
                            return "10";
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                    {
                        if(propertytype.FullName != null)
                        {
                            if (propertytype.FullName?.Contains("ICollection") == true)
                            {
                                return "new List<" + propertytype.GenericTypeArguments[0].Name + ">()";
                            }
                            else
                            {
                                return "new " + propertytype.Name + "()";
                            }
                        }
                        else
                        {
                            return "";
                        }
                    }
                case TypeCode.DBNull:
                    return "null";
                case TypeCode.Boolean:
                    return "true";
                case TypeCode.Char:
                    return "1";
                case TypeCode.SByte:
                    return "1";
                case TypeCode.Byte:
                    return "1";
                case TypeCode.Int16:
                    return "1";
                case TypeCode.UInt16:
                    return "1";
                case TypeCode.Int32:
                    return "1";
                case TypeCode.UInt32:
                    return "1";
                case TypeCode.Int64:
                    return "1000";
                case TypeCode.UInt64:
                    return "1";
                case TypeCode.Single:
                    return "1";
                case TypeCode.Double:
                    return "1";
                case TypeCode.Decimal:
                    return "10";
                case TypeCode.DateTime:
                    return "new DateTime(2018,10,31)";
                case TypeCode.String:
                    return "\"abc\"";
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

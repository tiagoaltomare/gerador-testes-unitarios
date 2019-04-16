using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorTestesUnitarios
{
    public static class Configuracoes
    {
        public const string pastaEntrada = @"C:\Users\SEU_USUARIO\Documents\Files\In\";
        public const string pastaSaida = @"C:\Users\SEU_USUARIO\Documents\Files\Out\";
        public const string baseClass = "using System;\r\n using System.Text;\r\n using System.IO;\r\n using Microsoft.VisualStudio.TestTools.UnitTesting;\r\n using System.Diagnostics.CodeAnalysis;\r\n using System.Collections.Generic;\r\n";
        public const string testConstructor = @"[TestInitialize]"+"\r\n"+ "public void TestInitialize()" + "\r\n" + "{" + "\r\n" + "objTest = new NEW_OBJECT(); PROPERTIES " + "\r\n" + " } " + "\r\n";
        public const string constructorProperties = "PROPERTIE = VALUE_TYPE";
        public const string assertTrue = "Assert.IsTrue(objTest.NAME_PARAMETER == VALUE_TYPE);";
        public const string assertInstance = "Assert.IsInstanceOfType(objTest.NAME_PARAMETER, typeof(VALUE_TYPE));";
        public const string testGetSet = "[TestMethod]" + "\r\n" + "public void NAME_PARAMETER_Test()" +"\r\n"+ "{" + "\r\n" + "objTest.NAME_PARAMETER = VALUE_TYPE;" + "\r\n ASSERT \r\n"+"}" + "\r\n";
        public const string testMethod = @"[TestMethod]" + "\r\n" + "public RETURN_PARAMETER NAME_METHOD_Test(){var returnMethod = objTest.NAME_METHOD(PARAMETERS); Assert.IsTrue(returnMethod == RETURN_TYPE_VALUE_ASSERTION); " + "\r\n" + "}" + "\r\n";
    }
}

//MethodInfo[] methodInfos;
//methodInfos = type.GetMethods();

//                            foreach (MethodInfo methodInfo in methodInfos)
//                            {
//                                if(!methodInfo.Name.Contains("get")
//                                    && !methodInfo.Name.Contains("set")
//                                    && !methodInfo.Name.Contains("ToString") 
//                                    && !methodInfo.Name.Contains("Equals")
//                                    && !methodInfo.Name.Contains("GetHashCode")
//                                    && !methodInfo.Name.Contains("GetType")
//                                )
//                                {
//                                    string testMethodOut = Configuracoes.testMethod;
//string inputParameters = "";

//testMethodOut = testMethodOut.Replace("RETURN_PARAMETER", ValueTypeToSTring(methodInfo.ReturnParameter.ParameterType));
//                                    testMethodOut = testMethodOut.Replace("NAME_METHOD", methodInfo.Name);
//                                    testMethodOut = testMethodOut.Replace("NAME_CLASS", type.Name);;

//                                    ParameterInfo[] parameters;
//parameters = methodInfo.GetParameters();

//                                    foreach (ParameterInfo parameter in parameters)
//                                    {
//                                        inputParameters = inputParameters + ReturnValueType(parameter.ParameterType)+",";
//                                    }

//                                    if(parameters.Length > 0)
//                                        inputParameters = inputParameters.Remove(inputParameters.Length -1);

//                                    testMethodOut = testMethodOut.Replace("PARAMETERS", inputParameters);
//                                    testMethodOut = testMethodOut.Replace("RETURN_TYPE_VALUE_ASSERTION", ReturnValueType(methodInfo.ReturnParameter.ParameterType));

//                                    sw.WriteLine(testMethodOut);
//                                }
//                            }
//                            Console.WriteLine("Métodos Gerados");
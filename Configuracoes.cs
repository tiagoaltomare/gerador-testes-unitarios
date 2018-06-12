namespace RoboTestesAutomatizados
{
    public static class Configuracoes
    {
        public const string pastaEntrada = @"C:\Files\In\";
        public const string pastaSaida = @"C:\Files\Out\";
        public const string baseClass = "using System; using System.Text; using System.IO; using Micosoft.VisualStudio.TestTools.UnitTesting; ";
        public const string testConstructor = @"[TestInitialize] public CLASS_NAMEUnitTest(){ PROPERTIES }";
        public const string constructorProperties = "this.PROPERTIE = VALUE_TYPE";
        public const string testGetSet = @"[TestMethod] public void NAME_PARAMETER_Test(){objTest.NAME_PARAMETER = VALUE_TYPE; assert.istrue(objTest.NAME_PARAMETER == VALUE_TYPE, ""validação da propriedade NAME_PARAMETER da classe NAME_CLASS falhou""); }"; 
        public const string testMethod = @"[TestMethod] public RETURN_PARAMETER NAME_METHOD_Test(){var returnMethod = objTest.NAME_METHOD(PARAMETERS); assert.istrue(returnMethod == RETURN_TYPE_VALUE_ASSERTION, ""validação do método NAME_METHOD da classe NAME_CLASS falhou""); }";
    }
}
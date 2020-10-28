using System;
using System.Linq;
using Assembly_Lib;
using Xunit;

namespace TestProject
{
    public class AssemblyLibTests
    {
        [Fact]
        public void ShouldReturnNullForInvalidPath()
        {
            Assert.Null(AssemblyLib.GetAssemblyInfo("bakabaka"));
        }

        [Fact]
        public void NumberOfNamespacesShouldBeCorrect()
        {
            Assert.Equal(1, AssemblyLib.GetAssemblyInfo("C:\\Users\\maksf\\Desktop\\СПП\\lab-3\\Assembly Lib.dll")?.NamespaceInfos?.Count);
        }
        
        [Fact]
        public void NumberOfMethodsShouldBeCorrect()
        {
            Assert.Equal(8, AssemblyLib.GetAssemblyInfo("C:\\Users\\maksf\\Desktop\\СПП\\lab-3\\Assembly Lib.dll")?.NamespaceInfos.Values.First().DataTypeInfos.Last().MethodInfos.Length);
        }
        
        [Fact]
        public void NumberOfFieldsShouldBeCorrect()
        {
            Assert.Equal(0, AssemblyLib.GetAssemblyInfo("C:\\Users\\maksf\\Desktop\\СПП\\lab-3\\Assembly Lib.dll")?.NamespaceInfos.Values.First().DataTypeInfos.Last().FieldInfos.Length);
        }
        
        [Fact]
        public void NumberOfPropertiesShouldBeCorrect()
        {
            Assert.Equal(2, AssemblyLib.GetAssemblyInfo("C:\\Users\\maksf\\Desktop\\СПП\\lab-3\\Assembly Lib.dll")?.NamespaceInfos?.Values.First().DataTypeInfos.Last().PropertyInfos.Length);
        }
        
    }
}
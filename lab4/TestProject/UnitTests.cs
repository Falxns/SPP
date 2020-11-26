using Xunit;
using System.Collections.Generic;
using System.IO;
using TestsClassGenerator;
using TestClass = TestsClassGenerator.TestClass;

namespace TestProject
{
    public class UnitTests
    {
        private TestClassGenerator _generator = new TestClassGenerator();
        private static string _emptyClassInput = "C:\\Users\\maksf\\Desktop\\SPP\\lab4\\TestProject\\Blank1.cs";
        private static string[] _emptyClassesInput = { 
            "C:\\Users\\maksf\\Desktop\\SPP\\lab4\\TestProject\\Blank1.cs",
            "C:\\Users\\maksf\\Desktop\\SPP\\lab4\\TestProject\\Blank2.cs"
        };
        private static string _correctPath = "C:\\Users\\maksf\\Desktop\\SPP\\lab1\\ConsoleApp\\Program.cs";
        private static string[] _correctClassesInput = { 
            "C:\\Users\\maksf\\Desktop\\SPP\\lab1\\ConsoleApp\\Program.cs", 
            "C:\\Users\\maksf\\Desktop\\SPP\\lab1\\MainLibrary\\Tracist.cs"
        };
        
        [Fact]
        public void ClassWithNoMethods()
        {
            using (StreamReader SourceReader = File.OpenText(_emptyClassInput))
            {
                List<TestClass> result = _generator.GenerateTestClasses(SourceReader.ReadToEnd());
                Assert.Empty(result);
            }
        }
        
        [Fact]
        public void SeveralClassesWithNoMethods()
        {
            foreach (var path in _emptyClassesInput)
            {
                using (StreamReader SourceReader = File.OpenText(path))
                {
                    List<TestClass> result = _generator.GenerateTestClasses(SourceReader.ReadToEnd());
                    Assert.Empty(result);
                }
            }
        }
        
        [Fact]
        public void CorrectInputForSeveralClasses()
        {
            int result = 0;
            foreach (var path in _correctClassesInput)
            {
                using (StreamReader SourceReader = File.OpenText(path))
                {
                    result += _generator.GenerateTestClasses(SourceReader.ReadToEnd()).Count;
                }
            }
            Assert.Equal(4, result);
        }
        
        [Fact]
        public void CorrectInput()
        {
            using (StreamReader SourceReader = File.OpenText(_correctPath))
            {
                Assert.NotEmpty(_generator.GenerateTestClasses(SourceReader.ReadToEnd()));
            }
        }
    }
}
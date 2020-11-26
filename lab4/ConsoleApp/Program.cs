using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks.Dataflow;
using TestsClassGenerator;

class Dataflow
{
   private const string DestPath = "C:\\Users\\maksf\\Desktop\\SPP\\lab4\\Output\\";
   private const int MaxDegreeLoad = 1;
   private const int MaxDegreeGenerate = 1;
   private const int MaxDegreeSave = 1;
   private static string[] _input = new[] { 
      "C:\\Users\\maksf\\Desktop\\SPP\\lab1\\MainLibrary\\Tracist.cs", 
      "C:\\Users\\maksf\\Desktop\\SPP\\lab1\\ConsoleApp\\Program.cs",
      "C:\\Users\\maksf\\Desktop\\SPP\\lab2\\App\\Program.cs",
      "C:\\Users\\maksf\\Desktop\\SPP\\lab2\\FakerLib\\Faker.cs",
      "C:\\Users\\maksf\\Desktop\\SPP\\lab3\\Assembly Lib\\Main.cs"
   };

   static void Main()
   {
      var sw = new Stopwatch();
      sw.Start();
      
      var loadSourceFileToMemory = new TransformBlock<string, string>(async path =>
      {
         Console.WriteLine("Loading to memory '{0}'...", path);

         using StreamReader SourceReader = File.OpenText(path);
         return await SourceReader.ReadToEndAsync();
      }, new ExecutionDataflowBlockOptions
      {
         MaxDegreeOfParallelism = MaxDegreeLoad,
         EnsureOrdered = false
      });

      var generateTestClass = new TransformManyBlock<string, TestClass>(text =>
      {
         Console.WriteLine("Generating test classes...");
         
         TestClassGenerator classGenerator = new TestClassGenerator();

         return classGenerator.GenerateTestClasses(text);
      }, new ExecutionDataflowBlockOptions
      {
         MaxDegreeOfParallelism = MaxDegreeGenerate,
         EnsureOrdered = false
      });

      var saveTestClassToFile = new ActionBlock<TestClass>(async testClass =>
      {
         using StreamWriter DestinationWriter = File.CreateText(DestPath + testClass.FileName);
         Console.WriteLine("Saving '{0}' on disk...", testClass.FileName);
         await DestinationWriter.WriteAsync(testClass.Source);
         Console.WriteLine("Saved '{0}'!", testClass.FileName);
      }, new ExecutionDataflowBlockOptions
      {
         MaxDegreeOfParallelism = MaxDegreeSave,
         EnsureOrdered = false
      });

      var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

      loadSourceFileToMemory.LinkTo(generateTestClass, linkOptions);
      generateTestClass.LinkTo(saveTestClassToFile, linkOptions);

      foreach (var path in _input)
      {
         loadSourceFileToMemory.Post(path);
      }

      loadSourceFileToMemory.Complete();

      saveTestClassToFile.Completion.Wait();
      sw.Stop();
      Console.WriteLine(sw.ElapsedMilliseconds);
   }
}

using System;
using System.Collections.Generic;

namespace PurityEngine.UnitTests {

    [Serializable]
    public class UnitTestFailedException : System.Exception
    {
        public UnitTestFailedException() { }
        public UnitTestFailedException(string message) : base(message) { }
        public UnitTestFailedException(string message, System.Exception inner) : base(message, inner) { }
        protected UnitTestFailedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class UnitTest {
        public static List<UnitTest> unitTests = new List<UnitTest>();

        public delegate dynamic UnitTestBody();

        public UnitTestBody task;
        public string expectedOutput;
        public string id;

        public UnitTest(UnitTestBody body, dynamic output) {
            task = body;
            expectedOutput = output;
        }

        public static void InitializeUnitTests() {
            Register(new UnitTest(CompilerTest1,"if (noop) {noop}"));
        }

        public static dynamic CompilerTest1() {
            AL.Compiler cmp = new AL.Compiler();
            return cmp.Compile("if (noop) { noop }").ToString();
        }

        public static void Register(UnitTest t) {
            if (!unitTests.Contains(t)) {
                unitTests.Add(t);
            }
        }

        public void _Run() {
            dynamic output = task();
            if (output != expectedOutput) {
                throw new UnitTestFailedException("The task \"" + task.ToString() + "\" outputted " + output.ToString() + " but expected " + expectedOutput.ToString());
            }
        }

        public static void Run() {
            foreach (UnitTest test in unitTests) {
                test._Run();
            }
            Console.WriteLine("Unit tests ran successfully.");
        }
    }   
}
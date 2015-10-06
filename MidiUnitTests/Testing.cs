// Copyright (c) 2009, Tom Lokovic
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//     * Redistributions of source code must retain the above copyright notice,
//       this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

using System;

namespace MidiUnitTests
{
    /// <summary>
    /// Simple NUnit-like framework for unit tests.
    /// </summary>
    public static class TestRunner
    {        
        /// <summary>
        /// Runs all tests in the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly to process.</param>
        /// <remarks>
        /// <para>Finds all classes in assembly that have the <see cref="TestFixture"/> attribute.
        /// For each test fixture, finds all methods with the <see cref="Test"/> attribute.  For
        /// each test method, instantiates the fixture object, calls all methods with the
        /// <see cref="SetUp"/> attribute, then calls the test method.</para>
        /// <para>Assumes that tests throw uncaught exceptions to indicate failure.</para>
        /// </remarks>
        public static void RunTestsInAssembly(System.Reflection.Assembly assembly)
        {
            int numTestsPassed = 0;
            foreach (Type fixture in assembly.GetTypes())
            {
                if (TestFixture.IsOn(fixture))
                {
                    foreach (System.Reflection.MethodInfo testMethod in fixture.GetMethods())
                    {
                        if (Test.IsOn(testMethod))
                        {
                            Console.WriteLine("Running Test {0}.{1}...",
                                fixture.FullName, testMethod.Name);
                            Type[] noTypes = { };
                            object instance = fixture.GetConstructor(noTypes).Invoke(null);
                            foreach (System.Reflection.MethodInfo setupMethod in
                                fixture.GetMethods())
                            {
                                if (SetUp.IsOn(setupMethod))
                                {
                                    try
                                    {
                                        setupMethod.Invoke(instance, null);
                                    }
                                    catch (System.Reflection.TargetInvocationException te)
                                    {
                                        PrintFailure(String.Format(
                                            "setup method {0} for test {1} on fixture {2}",
                                            setupMethod.Name, testMethod.Name, fixture.FullName),
                                            te.InnerException);
                                        return;
                                    }
                                }
                            }
                            try
                            {
                                testMethod.Invoke(instance, null);
                            }
                            catch (System.Reflection.TargetInvocationException te)
                            {
                                PrintFailure(String.Format("test {0} on fixture {1}",
                                    testMethod.Name, fixture.FullName), te.InnerException);
                                return;
                            }
                            Console.WriteLine("Passed.");
                            numTestsPassed++;
                        }
                    }
                }
            }
            Console.WriteLine("All {0} tests passed.", numTestsPassed);
        }

        private static void PrintFailure(string failureContext, Exception e)
        {
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("FAILED in {0}", failureContext);
            Console.WriteLine("Exception Type: {0}", e.GetType());
            Console.WriteLine("Exception Message: {0}", e.Message);
            Console.WriteLine("Stack trace:");
            Console.WriteLine(e.StackTrace);
            Console.WriteLine("FAILURE.");
        }
    }

    /// <summary>
    /// Simple NUnit-like assertion class.
    /// </summary>
    public static class Assert
    {
        public static void True(bool condition)
        {
            if (!condition)
            {
                throw new Exception("Assert.True failed.");
            }
        }


        public static void False(bool condition)
        {
            if (condition)
            {
                throw new Exception("Assert.True failed.");
            }
        }

        public static void AreEqual(object a, object b)
        {
            if (!a.Equals(b))
            {
                throw new Exception(String.Format("Assert.AreEqual failed: {0} != {1}.", a, b));
            }
        }

        public static void AreNotEqual(object a, object b)
        {
            if (a.Equals(b))
            {
                throw new Exception(String.Format("Assert.AreNotEqual failed: {0} == {1}.", a, b));
            }
        }

        public static void Throws(Type exceptionType, Action snippet)
        {
            bool failedToThrow = false;
            try
            {
                snippet();
                failedToThrow = true;
            }
            catch (Exception ex)
            {
                if (!exceptionType.IsInstanceOfType(ex))
                {
                    throw new Exception(String.Format(
                        "Assert.Throws failed: wrong exception type (got {0}, expected {1})",
                        ex.GetType(), exceptionType));
                }
            }
            if (failedToThrow)
            {
                throw new Exception(String.Format(
                    "Assert.Throws failed: did not throw (expected {0})", exceptionType));
            }
        }
    }

    /// <summary>
    /// Attribute used to indicate that a class is a test fixture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TestFixture : System.Attribute {
        public static bool IsOn(Type t)
        {
            foreach (System.Attribute attr in t.GetCustomAttributes(false))
            {
                if (attr is TestFixture)
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Attribute used to indicate that a method is a test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class Test : System.Attribute {
        public static bool IsOn(System.Reflection.MethodInfo m)
        {
            foreach (System.Attribute attr in m.GetCustomAttributes(false))
            {
                if (attr is Test)
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Attribute used to indicate that a method is a setup method for a test fixture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SetUp : System.Attribute
    {
        public static bool IsOn(System.Reflection.MethodInfo m)
        {
            foreach (System.Attribute attr in m.GetCustomAttributes(false))
            {
                if (attr is SetUp)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

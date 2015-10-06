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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Midi;

namespace MidiExamples
{
    class Program
    {
        /// <summary>
        /// A dictionary mapping a console key to example instances.
        /// </summary>
        static Dictionary<ConsoleKey, ExampleBase> examples =
            new Dictionary<ConsoleKey, ExampleBase>
        {
            { ConsoleKey.A, new Example01()},
            { ConsoleKey.B, new Example02()},
            { ConsoleKey.C, new Example03()},
//            { ConsoleKey.D, new Example04()},
            { ConsoleKey.E, new Example05()},
            { ConsoleKey.F, new Example06()},
        };

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("MIDI Examples:");
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------------------");
                foreach (KeyValuePair<ConsoleKey, ExampleBase> example in examples)
                {
                    Console.WriteLine("{0} : {1} ({2})",
                        example.Key.ToString().ToLower(),
                        example.Value.FileName, example.Value.Description);
                }
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine();
                Console.Write("Enter the letter for an example to run, or Escape to quit: ");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return;
                }
                if (examples.ContainsKey(keyInfo.Key))
                {
                    ExampleBase example = examples[keyInfo.Key];
                        Console.Clear();
                        example.Run();
                }
            }
        }
    }
}

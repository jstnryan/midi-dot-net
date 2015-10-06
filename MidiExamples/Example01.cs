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
using Midi;

namespace MidiExamples
{
    /// <summary>
    /// Prints information about MIDI input and output devices.
    /// </summary>
    class Example01 : ExampleBase
    {
        public Example01()
            : base("Example01.cs", "Prints info about devices.")
        { }

        public override void Run()
        {
            // Print a table of the input device names, or "No input devices" if there are none.
            if (InputDevice.InstalledDevices.Count == 0)
            {
                Console.WriteLine("No input devices.");
            }
            else
            {
                Console.WriteLine("Input Devices:");
                foreach (InputDevice device in InputDevice.InstalledDevices)
                {
                    Console.WriteLine("  {0}", device.Name);
                }
            }
            Console.WriteLine();

            // Print a table of the output device names, or "No output devices" if there are none.
            if (OutputDevice.InstalledDevices.Count == 0)
            {
                Console.WriteLine("No output devices.");
            }
            else
            {
                Console.WriteLine("Output Devices:");
                foreach (OutputDevice device in OutputDevice.InstalledDevices)
                {
                    Console.WriteLine("  {0}", device.Name);
                }
            }

            // All done.
            Console.WriteLine();
            ExampleUtil.PressAnyKeyToContinue();
        }
    }
}

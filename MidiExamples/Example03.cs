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
using System.Threading;
using System.Collections.Generic;

namespace MidiExamples
{
    /// <summary>
    /// Simple drum machine.
    /// </summary>
    /// <remarks>
    /// This example creates a simple drum machine controlled by the QWERTY keyboard and using
    /// OutputDevice.SendPercussion().
    /// </remarks>
    class Example03 : ExampleBase
    {
        public Example03()
            : base("Example03.cs", "Alphabetic keys play MIDI percussion sounds.")
        { }

        // Defines QUERTY order so that the percussion sounds can map to the keyboard in
        // that order.
        private static ConsoleKey[] QwertyOrder  = new ConsoleKey[] {
            ConsoleKey.Q, ConsoleKey.W, ConsoleKey.E, ConsoleKey.R, ConsoleKey.T, ConsoleKey.Y, 
            ConsoleKey.U, ConsoleKey.I, ConsoleKey.O, ConsoleKey.P, ConsoleKey.A, ConsoleKey.S, 
            ConsoleKey.D, ConsoleKey.F, ConsoleKey.G, ConsoleKey.H, ConsoleKey.J, ConsoleKey.K, 
            ConsoleKey.L, ConsoleKey.Z, ConsoleKey.X, ConsoleKey.C, ConsoleKey.V, ConsoleKey.B, 
            ConsoleKey.N, ConsoleKey.M 
        };

        public override void Run()
        {
            // Prompt the user to choose an output device (or if there is only one, use that one).
            OutputDevice outputDevice = ExampleUtil.ChooseOutputDeviceFromConsole();
            if (outputDevice == null)
            {
                Console.WriteLine("No output devices, so can't run this example.");
                ExampleUtil.PressAnyKeyToContinue();
                return;
            }
            outputDevice.Open();

            // Generate two maps from console keys to percussion sounds: one for when alphabetic
            // keys are pressed and one for when they're pressed with shift.
            Dictionary<ConsoleKey, Percussion> unshiftedKeys =
                new Dictionary<ConsoleKey, Percussion>();
            Dictionary<ConsoleKey, Percussion> shiftedKeys =
                new Dictionary<ConsoleKey, Percussion>();
            for (int i = 0; i < 26; ++i)
            {
                unshiftedKeys[QwertyOrder[i]] = Percussion.BassDrum1 + i;
                if (i < 20)
                {
                    shiftedKeys[QwertyOrder[i]] = Percussion.BassDrum1 + 26 + i;
                }
            }

            Console.WriteLine("Press alphabetic keys (with and without SHIFT) to play MIDI "+
                "percussion sounds.");
            Console.WriteLine("Press Escape when finished.");
            Console.WriteLine();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else if ((keyInfo.Modifiers & ConsoleModifiers.Shift) != 0)
                {
                    if (shiftedKeys.ContainsKey(keyInfo.Key))
                    {
                        Percussion note = shiftedKeys[keyInfo.Key];
                        Console.Write("\rNote {0} ({1})         ", (int)note, note.Name());
                        outputDevice.SendPercussion(note, 90);
                    }
                }
                else
                {
                    if (unshiftedKeys.ContainsKey(keyInfo.Key))
                    {
                        Percussion note = unshiftedKeys[keyInfo.Key];
                        Console.Write("\rNote {0} ({1})         ", (int)note, note.Name());
                        outputDevice.SendPercussion(note, 90);
                    }
                }
            }

            // Close the output device.
            outputDevice.Close();

            // All done.
        }
    }
}

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
using Midi;

namespace MidiExamples
{
    /// <summary>
    /// Utility functions for MIDI examples.
    /// </summary>
    public class ExampleUtil
    {
        /// <summary>
        /// Chooses an output device, possibly prompting the user at the console.
        /// </summary>
        /// <returns>The chosen output device, or null if none could be chosen.</returns>
        /// If there is exactly one output device, that one is chosen without prompting the user.
        public static OutputDevice ChooseOutputDeviceFromConsole()
        {
            if (OutputDevice.InstalledDevices.Count == 0)
            {
                return null;
            }
            if (OutputDevice.InstalledDevices.Count == 1) {
                return OutputDevice.InstalledDevices[0];
            }
            Console.WriteLine("Output Devices:");
            for (int i = 0; i < OutputDevice.InstalledDevices.Count; ++i)
            {
                Console.WriteLine("   {0}: {1}", i, OutputDevice.InstalledDevices[i].Name);
            }
            Console.Write("Choose the id of an output device...");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int deviceId = (int)keyInfo.Key - (int)ConsoleKey.D0;
                if (deviceId >= 0 && deviceId < OutputDevice.InstalledDevices.Count)
                {
                    return OutputDevice.InstalledDevices[deviceId];
                }
            }
        }

        /// <summary>
        /// Chooses an input device, possibly prompting the user at the console.
        /// </summary>
        /// <returns>The chosen input device, or null if none could be chosen.</returns>
        /// If there is exactly one input device, that one is chosen without prompting the user.
        public static InputDevice ChooseInputDeviceFromConsole()
        {
            if (InputDevice.InstalledDevices.Count == 0)
            {
                return null;
            }
            if (InputDevice.InstalledDevices.Count == 1)
            {
                return InputDevice.InstalledDevices[0];
            }
            Console.WriteLine("Input Devices:");
            for (int i = 0; i < InputDevice.InstalledDevices.Count; ++i)
            {
                Console.WriteLine("   {0}: {1}", i, InputDevice.InstalledDevices[i]);
            }
            Console.Write("Choose the id of an input device...");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int deviceId = (int)keyInfo.Key - (int)ConsoleKey.D0;
                if (deviceId >= 0 && deviceId < InputDevice.InstalledDevices.Count)
                {
                    return InputDevice.InstalledDevices[deviceId];
                }
            }
        }

        /// <summary>
        /// Prints "Press any key to continue." with a newline, then waits for a key to be pressed.
        /// </summary>
        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Key mappings for mock MIDI keys on the QWERTY keyboard.
        /// </summary>
        private static Dictionary<ConsoleKey, int> mockKeys = new Dictionary<ConsoleKey,int>
        {
            {ConsoleKey.Q,        53},
            {ConsoleKey.D2,       54},
            {ConsoleKey.W,        55},
            {ConsoleKey.D3,       56},
            {ConsoleKey.E,        57},
            {ConsoleKey.D4,       58},
            {ConsoleKey.R,        59},
            {ConsoleKey.T,        60},
            {ConsoleKey.D6,       61},
            {ConsoleKey.Y,        62},
            {ConsoleKey.D7,       63},
            {ConsoleKey.U,        64},
            {ConsoleKey.I,        65},
            {ConsoleKey.D9,       66},
            {ConsoleKey.O,        67},
            {ConsoleKey.D0,       68},
            {ConsoleKey.P,        69},
            {ConsoleKey.OemMinus, 70},
            {ConsoleKey.Oem4,     71},
            {ConsoleKey.Oem6,     72}
        };

        /// <summary>
        /// If the specified key is one of the computer keys used for mock MIDI input, returns true
        /// and sets pitch to the value.
        /// </summary>
        /// <param name="key">The computer key pressed.</param>
        /// <param name="pitch">The pitch it mocks.</param>
        /// <returns></returns>
        public static bool IsMockPitch(ConsoleKey key, out Pitch pitch)
        {
            if (mockKeys.ContainsKey(key))
            {
                pitch = (Pitch)mockKeys[key];
                return true;
            }
            pitch = 0;
            return false;
        }
    }
}

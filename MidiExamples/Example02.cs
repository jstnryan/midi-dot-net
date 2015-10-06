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

namespace MidiExamples
{
    /// <summary>
    /// Demonstrates simple single-threaded output.
    /// </summary>
    /// <remarks>
    /// This example uses the OutputDevice.Send* methods directly to generate output.  It uses
    /// Thread.Sleep for timing, which isn't practical in real applications because it blocks
    /// the main thread, forcing the user to sit and wait.  See Example03.cs for a more realistic
    /// example using Clock for scheduling.
    /// </remarks>
    class Example02 : ExampleBase
    {
        public Example02()
            : base("Example02.cs", "Simple MIDI output example.")
        { }

        void PlayChordRun(OutputDevice outputDevice, Chord chord, int millisecondsBetween)
        {
            Pitch previousNote = (Pitch)(-1);
            for (Pitch pitch = Pitch.A0; pitch < Pitch.C8; ++pitch)
            {
                if (chord.Contains(pitch))
                {
                    if (previousNote != (Pitch)(-1))
                    {
                        outputDevice.SendNoteOff(Channel.Channel1, previousNote, 80);
                    }
                    outputDevice.SendNoteOn(Channel.Channel1, pitch, 80);
                    Thread.Sleep(millisecondsBetween);
                    previousNote = pitch;
                }
            }
            if (previousNote != (Pitch)(-1))
            {
                outputDevice.SendNoteOff(Channel.Channel1, previousNote, 80);
            }
        }

        public override void Run()
        {
            // Prompt user to choose an output device (or if there is only one, use that one).
            OutputDevice outputDevice = ExampleUtil.ChooseOutputDeviceFromConsole();
            if (outputDevice == null)
            {
                Console.WriteLine("No output devices, so can't run this example.");
                ExampleUtil.PressAnyKeyToContinue();
                return;
            }
            outputDevice.Open();

            Console.WriteLine("Playing an arpeggiated C chord and then bending it down.");
            outputDevice.SendControlChange(Channel.Channel1, Control.SustainPedal, 0);
            outputDevice.SendPitchBend(Channel.Channel1, 8192);
            // Play C, E, G in half second intervals.
            outputDevice.SendNoteOn(Channel.Channel1, Pitch.C4, 80);
            Thread.Sleep(500);
            outputDevice.SendNoteOn(Channel.Channel1, Pitch.E4, 80);
            Thread.Sleep(500);
            outputDevice.SendNoteOn(Channel.Channel1, Pitch.G4, 80);
            Thread.Sleep(500);

            // Now apply the sustain pedal.
            outputDevice.SendControlChange(Channel.Channel1, Control.SustainPedal, 127);

            // Now release the C chord notes, but they should keep ringing because of the sustain
            // pedal.
            outputDevice.SendNoteOff(Channel.Channel1, Pitch.C4, 80);
            outputDevice.SendNoteOff(Channel.Channel1, Pitch.E4, 80);
            outputDevice.SendNoteOff(Channel.Channel1, Pitch.G4, 80);

            // Now bend the pitches down.
            for (int i = 0; i < 17; ++i)
            {
                outputDevice.SendPitchBend(Channel.Channel1, 8192 - i * 450);
                Thread.Sleep(200);
            }
            
            // Now release the sustain pedal, which should silence the notes, then center
            // the pitch bend again.
            outputDevice.SendControlChange(Channel.Channel1, Control.SustainPedal, 0);
            outputDevice.SendPitchBend(Channel.Channel1, 8192);

            Console.WriteLine("Playing the first two bars of Mary Had a Little Lamb...");
            Clock clock = new Clock(120);
            clock.Schedule(new NoteOnMessage(outputDevice, Channel.Channel1, Pitch.E4, 80, 0));
            clock.Schedule(new NoteOffMessage(outputDevice, Channel.Channel1, Pitch.E4, 80, 1));
            clock.Schedule(new NoteOnMessage(outputDevice, Channel.Channel1, Pitch.D4, 80, 1));
            clock.Schedule(new NoteOffMessage(outputDevice, Channel.Channel1, Pitch.D4, 80, 2));
            clock.Schedule(new NoteOnMessage(outputDevice, Channel.Channel1, Pitch.C4, 80, 2));
            clock.Schedule(new NoteOffMessage(outputDevice, Channel.Channel1, Pitch.C4, 80, 3));
            clock.Schedule(new NoteOnMessage(outputDevice, Channel.Channel1, Pitch.D4, 80, 3));
            clock.Schedule(new NoteOffMessage(outputDevice, Channel.Channel1, Pitch.D4, 80, 4));
            clock.Schedule(new NoteOnMessage(outputDevice, Channel.Channel1, Pitch.E4, 80, 4));
            clock.Schedule(new NoteOffMessage(outputDevice, Channel.Channel1, Pitch.E4, 80, 5));
            clock.Schedule(new NoteOnMessage(outputDevice, Channel.Channel1, Pitch.E4, 80, 5));
            clock.Schedule(new NoteOffMessage(outputDevice, Channel.Channel1, Pitch.E4, 80, 6));
            clock.Schedule(new NoteOnMessage(outputDevice, Channel.Channel1, Pitch.E4, 80, 6));
            clock.Schedule(new NoteOffMessage(outputDevice, Channel.Channel1, Pitch.E4, 80, 7));
            clock.Start();
            Thread.Sleep(5000);
            clock.Stop();

            Console.WriteLine("Playing sustained chord runs up the keyboard...");
            outputDevice.SendControlChange(Channel.Channel1, Control.SustainPedal, 127);
            PlayChordRun(outputDevice, new Chord("C"), 100);
            outputDevice.SendControlChange(Channel.Channel1, Control.SustainPedal, 0);
            outputDevice.SendControlChange(Channel.Channel1, Control.SustainPedal, 127);
            PlayChordRun(outputDevice, new Chord("F"), 100);
            outputDevice.SendControlChange(Channel.Channel1, Control.SustainPedal, 0);
            outputDevice.SendControlChange(Channel.Channel1, Control.SustainPedal, 127);
            PlayChordRun(outputDevice, new Chord("G"), 100);
            outputDevice.SendControlChange(Channel.Channel1, Control.SustainPedal, 0);
            outputDevice.SendControlChange(Channel.Channel1, Control.SustainPedal, 127);
            PlayChordRun(outputDevice, new Chord("C"), 100);
            Thread.Sleep(2000);
            outputDevice.SendControlChange(Channel.Channel1, Control.SustainPedal, 0);

            // Close the output device.
            outputDevice.Close();

            // All done.
            Console.WriteLine();
            ExampleUtil.PressAnyKeyToContinue();
        }
    }
}

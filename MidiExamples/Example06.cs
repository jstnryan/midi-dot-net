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
    /// Simple arpeggiator.
    /// </summary>
    /// <remarks>
    /// This example demonstrates input, output and Clock-based scheduling.  As Note On and
    /// Note Off events are received from the input device, the Arpeggiator class schedules
    /// arpeggiated chords or scales based on the note played. 
    /// </remarks>
    public class Example06 : ExampleBase
    {
        public Example06()
            : base("Example06.cs", "Arpeggiator.")
        { }

        class Arpeggiator
        {
            public Arpeggiator(InputDevice inputDevice, OutputDevice outputDevice, Clock clock)
            {
                this.inputDevice = inputDevice;
                this.outputDevice = outputDevice;
                this.clock = clock;
                this.currentChordPattern = 0;
                this.currentScalePattern = 0;
                this.playingChords = false;
                this.lastSequenceForPitch = new Dictionary<Pitch, List<Pitch>>();

                if (inputDevice != null)
                {
                    inputDevice.NoteOn += new InputDevice.NoteOnHandler(this.NoteOn);
                    inputDevice.NoteOff += new InputDevice.NoteOffHandler(this.NoteOff);
                }
            }

            /// <summary>
            /// String describing the arpeggiator's current configuration.
            /// </summary>
            public string Status
            {
                get
                {
                    lock (this)
                    {
                        if (playingChords)
                        {
                            return "Chord: " + Chord.Patterns[currentChordPattern].Name;
                        }
                        else
                        {
                            return "Scale: " + Scale.Patterns[currentScalePattern].Name;
                        }
                    }
                }
            }

            /// <summary>
            /// Toggle between playing chords and playing scales.
            /// </summary>
            public void ToggleMode()
            {
                lock(this)
                {
                    playingChords = !playingChords;
                }
            }

            /// <summary>
            /// Changes the current chord or scale, whichever is the current mode.
            /// </summary>
            public void Change(int delta)
            {
                lock (this)
                {
                    if (playingChords)
                    {
                        currentChordPattern = currentChordPattern + delta;
                        while (currentChordPattern < 0)
                        {
                            currentChordPattern += Chord.Patterns.Length;
                        }
                        while (currentChordPattern >= Chord.Patterns.Length)
                        {
                            currentChordPattern -= Chord.Patterns.Length;
                        }
                    }
                    else
                    {
                        currentScalePattern = currentScalePattern + delta;
                        while (currentScalePattern < 0)
                        {
                            currentScalePattern += Scale.Patterns.Length;
                        }
                        while (currentScalePattern >= Scale.Patterns.Length)
                        {
                            currentScalePattern -= Scale.Patterns.Length;
                        }
                    }
                }
            }

            public void NoteOn(NoteOnMessage msg)
            {
                lock (this)
                {
                    List<Pitch> pitches = new List<Pitch>();
                    if (playingChords)
                    {
                        Chord chord = new Chord(msg.Pitch.NotePreferringSharps(),
                            Chord.Patterns[currentChordPattern], 0);
                        Pitch p = msg.Pitch;
                        for (int i = 0; i < chord.NoteSequence.Length; ++i)
                        {
                            p = chord.NoteSequence[i].PitchAtOrAbove(p);
                            pitches.Add(p);
                        }
                    }
                    else
                    {
                        Scale scale = new Scale(msg.Pitch.NotePreferringSharps(),
                            Scale.Patterns[currentScalePattern]);
                        Pitch p = msg.Pitch;
                        for (int i = 0; i < scale.NoteSequence.Length; ++i)
                        {
                            p = scale.NoteSequence[i].PitchAtOrAbove(p);
                            pitches.Add(p);
                        }
                        pitches.Add(msg.Pitch + 12);
                    }
                    lastSequenceForPitch[msg.Pitch] = pitches;
                    for (int i = 1; i < pitches.Count; ++i)
                    {
                        clock.Schedule(new NoteOnMessage(outputDevice, msg.Channel,
                            pitches[i], msg.Velocity, msg.Time + i));
                    }
                }
            }

            public void NoteOff(NoteOffMessage msg)
            {
                if (!lastSequenceForPitch.ContainsKey(msg.Pitch))
                {
                    return;
                }
                List<Pitch> pitches = lastSequenceForPitch[msg.Pitch];
                lastSequenceForPitch.Remove(msg.Pitch);
                for (int i = 1; i < pitches.Count; ++i)
                {
                    clock.Schedule(new NoteOffMessage(outputDevice, msg.Channel,
                        pitches[i], msg.Velocity, msg.Time + i));
                }
            }

            private InputDevice inputDevice;
            private OutputDevice outputDevice;
            private Clock clock;
            private int currentChordPattern;
            private int currentScalePattern;
            private bool playingChords;
            private Dictionary<Pitch, List<Pitch>> lastSequenceForPitch;
        }

        class Drummer
        {
            public Drummer(Clock clock, OutputDevice outputDevice, int beatsPerMeasure)
            {
                this.clock = clock;
                this.outputDevice = outputDevice;
                this.beatsPerMeasure = beatsPerMeasure;
                this.messagesForOneMeasure = new List<Message>();
                for (int i = 0; i < beatsPerMeasure; ++i)
                {
                    Percussion percussion = i == 0 ? Percussion.PedalHiHat : Percussion.MidTom1;
                    int velocity = i == 0 ? 100 : 40;
                    messagesForOneMeasure.Add(new PercussionMessage(outputDevice, percussion,
                        velocity, i));
                }
                messagesForOneMeasure.Add(new CallbackMessage(
                    new CallbackMessage.CallbackType(CallbackHandler), 0));
                clock.Schedule(messagesForOneMeasure, 0);
            }
            
            private void CallbackHandler(float time)
            {
                // Round up to the next measure boundary.
                float timeOfNextMeasure = time + beatsPerMeasure;
                clock.Schedule(messagesForOneMeasure, timeOfNextMeasure);
            }

            private Clock clock;
            private OutputDevice outputDevice;
            private int beatsPerMeasure;
            private List<Message> messagesForOneMeasure;
        }

        public override void Run()
        {
            // Create a clock running at the specified beats per minute.
            int beatsPerMinute = 180;
            Clock clock = new Clock(beatsPerMinute);
            
            // Prompt user to choose an output device (or if there is only one, use that one.
            OutputDevice outputDevice = ExampleUtil.ChooseOutputDeviceFromConsole();
            if (outputDevice == null)
            {
                Console.WriteLine("No output devices, so can't run this example.");
                ExampleUtil.PressAnyKeyToContinue();
                return;
            }
            outputDevice.Open();

            // Prompt user to choose an input device (or if there is only one, use that one).
            InputDevice inputDevice = ExampleUtil.ChooseInputDeviceFromConsole();
            if (inputDevice != null)
            {
                inputDevice.Open();
            }

            Arpeggiator arpeggiator = new Arpeggiator(inputDevice, outputDevice, clock);
            Drummer drummer = new Drummer(clock, outputDevice, 4);

            clock.Start();
            if (inputDevice != null)
            {
                inputDevice.StartReceiving(clock);
            }

            bool done = false;
            while (!done)
            {
                Console.Clear();
                Console.WriteLine("BPM = {0}, Playing = {1}, Arpeggiator Mode = {2}",
                    clock.BeatsPerMinute, clock.IsRunning, arpeggiator.Status);
                Console.WriteLine("Escape : Quit");
                Console.WriteLine("Down : Slower");
                Console.WriteLine("Up: Faster");
                Console.WriteLine("Left: Previous Chord or Scale");
                Console.WriteLine("Right: Next Chord or Scale");
                Console.WriteLine("Space = Toggle Play");
                Console.WriteLine("Enter = Toggle Scales/Chords");
                ConsoleKey key = Console.ReadKey(true).Key;
                Pitch pitch;
                if (key == ConsoleKey.Escape)
                {
                    done = true;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    clock.BeatsPerMinute -= 2;
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    clock.BeatsPerMinute += 2;
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    arpeggiator.Change(1);
                }
                else if (key == ConsoleKey.LeftArrow)
                {
                    arpeggiator.Change(-1);
                }
                else if (key == ConsoleKey.Spacebar)
                {
                    if (clock.IsRunning)
                    {
                        clock.Stop();
                        if (inputDevice != null)
                        {
                            inputDevice.StopReceiving();
                        }
                        outputDevice.SilenceAllNotes();
                    }
                    else
                    {
                        clock.Start();
                        if (inputDevice != null)
                        {
                            inputDevice.StartReceiving(clock);
                        }
                    }
                }
                else if (key == ConsoleKey.Enter)
                {
                    arpeggiator.ToggleMode();
                }
                else if (ExampleUtil.IsMockPitch(key, out pitch))
                {
                    // We've hit a QUERTY key which is meant to simulate a MIDI note, so
                    // send the Note On to the output device and tell the arpeggiator.
                    NoteOnMessage noteOn = new NoteOnMessage(outputDevice, 0, pitch, 100,
                        clock.Time);
                    clock.Schedule(noteOn);
                    arpeggiator.NoteOn(noteOn);
                    // We don't get key release events for the console, so schedule a
                    // simulated Note Off one beat from now.
                    NoteOffMessage noteOff = new NoteOffMessage(outputDevice, 0, pitch, 100,
                        clock.Time + 1);
                    CallbackMessage.CallbackType noteOffCallback = beatTime =>
                    {
                        arpeggiator.NoteOff(noteOff);
                    };
                    clock.Schedule(new CallbackMessage(beatTime => arpeggiator.NoteOff(noteOff),
                        noteOff.Time));
                }
            }

            if (clock.IsRunning)
            {
                clock.Stop();
                if (inputDevice != null)
                {
                    inputDevice.StopReceiving();
                }
                outputDevice.SilenceAllNotes();
            }

            outputDevice.Close();
            if (inputDevice != null)
            {
                inputDevice.Close();
                inputDevice.RemoveAllEventHandlers();
            }

            // All done.
        }
    }
}

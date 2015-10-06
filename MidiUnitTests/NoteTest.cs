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

namespace MidiUnitTests
{
    /// <summary>Unit tests for the Note class.</summary>
    [TestFixture]
    class NoteTest
    {
        [Test]
        public void Construction()
        {
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => new Note('H'));
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => new Note('c'));
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => new Note('c', Note.Natural));
            Assert.AreEqual(new Note('B'), new Note('B', Note.Natural));
            Assert.AreEqual(new Note('C'), new Note("C"));
            Assert.AreEqual(new Note('D', Note.Sharp), new Note("D#"));
            Assert.AreEqual(new Note('E', Note.DoubleSharp), new Note("E##"));
            Assert.AreEqual(new Note('F', Note.Flat), new Note("Fb"));
            Assert.AreEqual(new Note('G', Note.DoubleFlat), new Note("Gbb"));
            Assert.Throws(typeof(ArgumentNullException), () => new Note((string)null));
            Assert.Throws(typeof(ArgumentException), () => new Note(""));
            Assert.Throws(typeof(ArgumentException), () => new Note("X"));
            Assert.Throws(typeof(ArgumentException), () => new Note("Cf"));
            Assert.Throws(typeof(ArgumentException), () => new Note("C##x"));
            Assert.Throws(typeof(ArgumentException), () => new Note("Db#"));
        }
        
        [Test]
        public void Equality()
        {
            Assert.AreEqual(new Note('C'), new Note('C', Note.Natural));
            Assert.AreEqual(new Note('C', Note.Natural), new Note('C', Note.Natural));
            Assert.AreEqual(new Note('C', Note.Sharp), new Note('C', Note.Sharp));
            Assert.AreNotEqual(new Note('C', Note.Natural), new Note('C', Note.Sharp));
            Assert.AreNotEqual(new Note('B', Note.Natural), new Note('C', Note.Natural));
            Assert.AreNotEqual(new Note('B', Note.Sharp), new Note('C', Note.Natural));
            Assert.AreNotEqual(new Note('C', Note.Sharp), new Note('D', Note.Flat));
        }

        [Test]
        public void IsEnharmonicWith()
        {
            Assert.True(new Note('C').IsEharmonicWith(new Note('C')));
            Assert.True(new Note('B', Note.Sharp).IsEharmonicWith(new Note('C', Note.Natural)));
            Assert.True(new Note('C', Note.Natural).IsEharmonicWith(new Note('B', Note.Sharp)));
            Assert.True(new Note('B', Note.DoubleSharp).IsEharmonicWith(new Note('C', Note.Sharp)));
            Assert.False(new Note('B', Note.DoubleSharp).IsEharmonicWith(
                new Note('D', Note.Natural)));
            Assert.True(new Note('B', 3).IsEharmonicWith(new Note('D', Note.Natural)));
            Assert.True(new Note('D', -3).IsEharmonicWith(new Note('B', Note.Natural)));
            Assert.True(new Note('C', 11).IsEharmonicWith(new Note('B', Note.Natural)));
        }

        [Test]
        public void Properties()
        {
            Assert.AreEqual(new Note("C").Letter, 'C');
            Assert.AreEqual(new Note("C#").Letter, 'C');
            Assert.AreEqual(new Note("D#").Letter, 'D');
            Assert.AreEqual(new Note("Db").Letter, 'D');
            Assert.AreEqual(new Note("B").Letter, 'B');

            Assert.AreEqual(new Note("C").Accidental, Note.Natural);
            Assert.AreEqual(new Note("C#").Accidental, Note.Sharp);
            Assert.AreEqual(new Note("D#").Accidental, Note.Sharp);
            Assert.AreEqual(new Note("Db").Accidental, Note.Flat);
            Assert.AreEqual(new Note("B").Accidental, Note.Natural);

            Assert.AreEqual(new Note("C").PositionInOctave, 0);
            Assert.AreEqual(new Note("C#").PositionInOctave, 1);
            Assert.AreEqual(new Note("D#").PositionInOctave, 3);
            Assert.AreEqual(new Note("Db").PositionInOctave, 1);
            Assert.AreEqual(new Note("B").PositionInOctave, 11);
            Assert.AreEqual(new Note("B#").PositionInOctave, 0);
        }

        [Test]
        public void ToStringTest()
        {
            Assert.AreEqual(new Note("C").ToString(), "C");
            Assert.AreEqual(new Note('F', 5).ToString(), "F#####");
            Assert.AreEqual(new Note('G', -3).ToString(), "Gbbb");
        }

        [Test]
        public void NoteToPitch()
        {
            Assert.AreEqual(new Note('C').PitchInOctave(4), Pitch.C4);
            Assert.AreEqual(new Note('C', Note.Sharp).PitchInOctave(2), Pitch.CSharp2);
            Assert.AreEqual(new Note('D', Note.Flat).PitchInOctave(2), Pitch.CSharp2);
            Assert.AreEqual(new Note('B').PitchInOctave(-2), (Pitch)(-1));

            Assert.AreEqual(new Note('C').PitchAtOrAbove(Pitch.C4), Pitch.C4);
            Assert.AreEqual(new Note('C', Note.Sharp).PitchAtOrAbove(Pitch.C4), Pitch.CSharp4);
            Assert.AreEqual(new Note('F', Note.Flat).PitchAtOrAbove(Pitch.C4), Pitch.E4);
            Assert.AreEqual(new Note('B', Note.Sharp).PitchAtOrAbove(Pitch.C4), Pitch.C4);
            Assert.AreEqual(new Note('C').PitchAtOrAbove(Pitch.B3), Pitch.C4);
            Assert.AreEqual(new Note('F').PitchAtOrAbove(Pitch.G3), Pitch.F4);

            Assert.AreEqual(new Note('C').PitchAtOrBelow(Pitch.C4), Pitch.C4);
            Assert.AreEqual(new Note('C', Note.Sharp).PitchAtOrBelow(Pitch.C4), Pitch.CSharp3);
            Assert.AreEqual(new Note('F', Note.Flat).PitchAtOrBelow(Pitch.C4), Pitch.E3);
            Assert.AreEqual(new Note('B', Note.Sharp).PitchAtOrBelow(Pitch.C4), Pitch.C4);
            Assert.AreEqual(new Note('C').PitchAtOrBelow(Pitch.B3), Pitch.C3);
            Assert.AreEqual(new Note('F').PitchAtOrBelow(Pitch.G3), Pitch.F3);
        }

        [Test]
        public void Semitones()
        {
            Assert.AreEqual(new Note('C').SemitonesUpTo(new Note('C')), 0);
            Assert.AreEqual(new Note('C').SemitonesUpTo(new Note('C', Note.Sharp)), 1);
            Assert.AreEqual(new Note('C', Note.Sharp).SemitonesUpTo(new Note('C')), 11);
            Assert.AreEqual(new Note('B').SemitonesUpTo(new Note('C')), 1);
            Assert.AreEqual(new Note('B').SemitonesUpTo(new Note('C', Note.Flat)), 0);
            Assert.AreEqual(new Note('G').SemitonesUpTo(new Note('F')), 10);

            Assert.AreEqual(new Note('C').SemitonesDownTo(new Note('C')), 0);
            Assert.AreEqual(new Note('C').SemitonesDownTo(new Note('C', Note.Sharp)), 11);
            Assert.AreEqual(new Note('C', Note.Sharp).SemitonesDownTo(new Note('C')), 1);
            Assert.AreEqual(new Note('B').SemitonesDownTo(new Note('C')), 11);
            Assert.AreEqual(new Note('B').SemitonesDownTo(new Note('C', Note.Flat)), 0);
            Assert.AreEqual(new Note('G').SemitonesDownTo(new Note('F')), 2);
        }
    }
}

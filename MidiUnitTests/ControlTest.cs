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
     /// <summary>Unit tests for the Control enum.</summary>
    [TestFixture]
    class ControlTest
    {
        [Test]
        public void Validity()
        {
            Assert.True(Control.Volume.IsValid());
            Control.Volume.Validate();
            Assert.True(Control.AllNotesOff.IsValid());
            Control.AllNotesOff.Validate();
            Assert.False(((Control)(-1)).IsValid());
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => ((Control)(-1)).Validate());
            Assert.False(((Control)(128)).IsValid());
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => ((Control)(128)).Validate());
        }

        [Test]
        public void Naming()
        {
            Assert.AreEqual(Control.SustainPedal.Name(), "Sustain pedal");
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => ((Control)(128)).Name());
        }
    }
}

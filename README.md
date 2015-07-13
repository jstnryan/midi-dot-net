Overview
---
This .NET library provides convenient, threadsafe access to MIDI devices. It does this by binding to the Win32 API with P/Invoke and then wrapping that in an object-oriented API which feels right at home in C# / .NET.

Features
---
**MIDI**

* Easy access to MIDI input and output devices.
* Register input handlers using C# events / delegates.
* Full support for sending and receiving Note On, Note Off, Control Change, Program Change, and Pitch Bend messages.
* Convenient enums for General MIDI Channels, Pitches, Instruments, Controls, and Percussions.

**Scheduling**

* A powerful scheduler that lets you schedule MIDI messages, pause, unpause, and adjust beats-per-minute while it runs.
* Support for callback messages, so you can schedule call-outs to your own code at specific instants. Callback messages are scheduled just like MIDI messages, so they are subject to pause, unpause, and beats-per-minute as well.
* Callback messages can schedule additional messages when triggered (self-propagating messages), which allows for a flexible continuation-passing style of control.
* Built-in NoteOnOffMessage schedules the Off message when the On message triggers.

**Music Theory**

* Octave-independent Note class which differentiates among enharmonic variants (eg, D♯; and E♭). Notes can be resolved to specific pitches as needed.
* Scale class built from tonic note and ascent pattern. Includes built-in patterns (Major, Melodic Minor Ascending, Chromatic, etc).
* Chord class built from tonic note, ascent pattern, and inversion. Includes built-in chord patterns (Major, Seventh, Diminished, etc).

Documentation
---------
* Interactive demo programs, both console and GUI.
Comprehensive API documentation.
* Unit tests.

Not-Yet-Features
-------
* ~~No support for extended MIDI messages such as System Exclusive ("sysex") messages.~~ [ Added in this repository by Justin Ryan. ]
* No MIDI file I/O.

Requirements
-----
* .NET Framework 3.5.
* Visual Studio C# 2008 (Express or Professional) or later.
* winmm.dll, the Win32 multimedia API which is standard on all modern Windows installs.

Reference
---
For information of the original project by Tom Lokovic, as well as the original documentation and samples, see the [midi-dot-net Google Code page](https://code.google.com/p/midi-dot-net/).

Copyright
---
Original code copyright (c) 2009 Tom Lokovic.
Additional modifications copyright Justin Ryan.

License
---

This library is distributed under the the New BSD License.  Each source file should have a
copy of the license in a comment at the top.  Under the terms of the license, redistribution of the
source code is allowed but requires, among other things, that the license text be retained.  For
details, see that license text or the official site: [http://www.opensource.org/licenses/bsd-license.php](http://www.opensource.org/licenses/bsd-license.php)
GBRead FAQ
Author: Silenthal
Email: Silenthal.Makram at GMail

[Table Of Contents]
1. Intro
2. Requirements
3. Usage

[1. Intro]
This program (GBRead) is meant to assist in the disassembly of GB/GBC files.
Since the normal disassembly process does not differentiate between what is
code and what is data, there are extra options available to help break up
disassembled code into discrete parts, to further understanding of the code.

This program can also insert data back into the original file, whether through
ASM, or through an external binary.

[2. Requirements]
.Net Framework 3.5 or greater.

[3. Usage]

a. Load a File
-From the File menu, select 'Load File'. When loaded, the program will print
some info about the file in the main box.

b. Disassemble
-Type in the start and end offsets (in hex) into the 'Start' and 'End' boxes,
and hit 'Print ASM' Of course, code and data are mixed together 
throughout the file, so you know that some of the ASM displayed may be wrong.
In that case...

c. Mark Functions, Data, and Variables
-For sections that you think are used as functions or data, you can add a 
marker to set them off. To do so, click [Add New...], enter the offset, name,
and optional comment, and then [OK]. You can add data sections the same 
way, as well as specify commonly used variables. Data sections are just areas
like tables of pointers, dialogue, and other places you are sure aren't data. 
Data sections can be defined as uncompressed tiles (images)as well.
Variables can be defined, for when you suspect a certain location in the
memory map is used repeatedly (e.g. [$FF41], the LCD Status register).

d. Insertion
You can insert ASM at a specified place, or a binary file. See 'about asm.txt'
for more details.

e. Saving
-In the File menu, you have several options for saving:

Save File: Saves a copy of the file you are working on. If you've altered the
file, you can save it this way.
Save Function/Data/Var List : Saves the contents of the function/data/var
boxes. Save format is as follows:

.label
_n:<name>
_o:<offset>
_c:<comment line>

.data
_n:<name>
_o:<offset>
_c:<comment line>
_d:<data row size>
_p1:<palette color 1 (optional)>
_p2:<palette color 2 (optional)>
_p3:<palette color 3 (optional)>
_p4:<palette color 4 (optional)>

.var
_n:<name>
_v:<value>
_c:<comment line>

label,data,var - Needed to specify between the two different types.
_c: Comment line. You can include multiple lines in a comment. Just prefix
each line with _c:

_d: Data Row Size. This determines where to place breaks when printing data
in the program. For example:

_d:3

db $56,$67,$07
db $08,$08,$09
db $09,$23,$54,
db $23

_d:8

db $56,$67,$07,$08,$08,$09,$09,$23
db $54,$23

_p1, 2, 3, 4: Palette colors. Each one is the 16-bit representation of the
color:
p[14:10] = Blue
p[9:5] = Green
p[4:0] = Red

Values are from 0-31 for each individual color, and the number itself can be
from 0 to 0x7FFF.

Entire File ASM: Will (try to) save the entire file in ASM form, with 
considerations made with respect to defined code/data sections, and variable
values.

f. Searching
There are some searches you can perform to save time.

-Find Called Functions
Adds an entry in the code label table for every called offset, adjusted so that
it points to the right bank, if possible. Some functions may be missed through
this method, and there will be tens to hundreds of invalid ones as well, so 
try to use it as more of a jumping off point.

g. Options
-Word Wrap
Controls the behavior of the main text window when printing text to either
wrap around long lines or not.

-Print Bytes
Controls whether to print the bytes that correspond to the instruction being
shown. Useful if you are trying to figure out what an instruction actually is.

-Print Offsets
Controls whether offsets are printed along with the instruction. Shows exactly
where in the file the instruction is. Disable this and the 'Print Bytes' 
option to just show ASM, labels, and comments.

-Offset Number Format
-Instruction Number Format
Controls the format for numbers that are printed in the offsets (controlled by
'Print Offsets') and in the instructions themselves.
--BB:OOOO - A format used in some emulators. First two numbers are the bank,
            last 4 are the offset, as represented by the Program Counter.
--Hexadecimal - Hex (base 16)
--Numeral - Regular numbers (base 10)

-Print Extended ASM Comments
As comments beside each line, the instruction will be repeated, in a less
abbreviated format.
Ex: rlc b ;Rotate Left Carry B
...not so useful if you know what everything does, though.

-Hide Defined Data
Instead of printing out the entire data section, a placeholder will
be printed : INCLUDE "blah.bin". Can be used to make looking at certain
sections easier.

-Highlighting
This option section controls the highlighting of certain key terms in the main
window. Can be useful for seeing the different parts of the code more clearly.
--Instructions
Controls the highlighting of instruction keywords, like ld, add, and so on.
--Numbers
Controls the highlighting of numbers in the instruction.
--Registers
Controls the highlighting of registers (a, b, c, hl), etc.
--Comments
Controls the highlighting of comments (lines preceded with ; or #)
--Labels
Controls the highlighting of labels(lines ending with :)
--Offsets
Controls the highlighting of the BB:OOOO format offsets.
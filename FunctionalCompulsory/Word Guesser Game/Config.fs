module Config

//this will be the character printed for those characters not guessed yet.
let HIDDEN = '*' 

//if this is false, the comparison must be non case-sensitive.
let CASE_SENSITIVE = false 

//if this is true, then the words may contain spaces, and they should be shown.
let ALLOW_BLANKS = false 

//if this is true, the program will make a correct guess, when the user can type Crtl-H
let HELP = true 

//if this is true, the user might enter more than one character; it will count as one guess. For instance; if the user enters“ab”, all substrings with “ab” will be visible.
let MULTIPLE = true 

//at list of strings. It is from this collection the program must get the random words to be guessed.
let WORDS = ["Words"; "Christmas Tree";"Santa Claus";"Presents";"Nissehue";"A Christmas Carol";"Yuletider"] 
module WordGuesser

open System;;
open Config;;
//Creates the hiddenword for the user
//Defines the hiddenword method for the user to guess
let HiddenWord (wordToFind:string) (listOfGuesses : string list) =

    //hides the word to guess for the user
    let HIDDEN = '*'
    
    //Compares two strings, and then checks the length of each string against each other.
    //It then creates a list for each string and adds the string to the list.
    let compare (firstWord:string) (secondWord:string) = 
        if firstWord.Length = secondWord.Length then 
            let firstWordList = firstWord |> Seq.toList |> List.map string
            let secondWordList = secondWord |> Seq.toList |> List.map string
            List.zip firstWordList secondWordList |> List.fold (fun s (f,l) -> s + (if f = HIDDEN.ToString() then l else f)) "" 
        else ""
    
    //Gets the hidden word from the list from one end and then inverses it to get it in the correct order.
    let getHidden (word:string) (guess:string) = 
        let inverseHidden = word.Replace(guess, HIDDEN |> string |> String.replicate (guess |> String.length))
        List.zip(word |> Seq.toList |> List.map string) (inverseHidden |> Seq.toList |> List.map string)
        |> List.fold (fun s (w,i) -> s + if w <> i || w = " " then w else HIDDEN.ToString()) ""

    //recursive function for finding the hidden word.
    let rec findHiddenWord = function
    | "",_ -> ""
    | s,[] -> compare (HIDDEN |> string |> String.replicate (s|> String.length) ) (HIDDEN |> string |> String.replicate (s|> String.length) )
    | s,[e] -> compare (getHidden s e) (HIDDEN |> string |> String.replicate (s|> String.length) )
    | s,e::l -> compare (getHidden s e) (findHiddenWord (s, l))

    findHiddenWord (wordToFind,listOfGuesses)


//Check if the input is valid, haven't been used before
let ValidateGuess (usedGuesses: string list) (currentGuess) =
    not (usedGuesses |> List.exists ((=) currentGuess))


//Have the program find a letter in the word that haven't been found before.
let GetHelp word used = 
    let currentKnownWord = HiddenWord word used
    let mutable moreKnownWord = currentKnownWord
    let mutable index = 0
    let mutable guess = ""
    while moreKnownWord = currentKnownWord do
        guess <- (word.Chars index).ToString()
        moreKnownWord <- HiddenWord word (guess::used)
        index <- index + 1
    guess

//Reads the user input and returns the input
let rec GetGuess wordToFind used =
    printf "Used %A. Guess: " used
    
    //defines the variables for input. Mutable in this case means the variable can be changed during run time.
    let mutable input = Console.ReadKey(true)
    let mutable fullString = ""
    let mutable help = false

    //while loop for the user's input.
    //Also checks the input key, and if its a backspace removes the most recently typed key.
    //If the inputkey is not a backspace then it adds it to the full string.
    while(input.Key <> ConsoleKey.Enter && not help) do 
        if input.Key = ConsoleKey.Backspace
        then fullString <- fullString.Remove(fullString.Length-1)
        else fullString <- fullString + input.KeyChar.ToString()
        if(HELP && input.Modifiers = ConsoleModifiers.Control && input.Key = ConsoleKey.H) then
                fullString <- GetHelp wordToFind used
                help <- true
        printf "%s" (input.KeyChar.ToString())
        if not help then input <- Console.ReadKey(true)
                
    printfn ""
    let input = if not CASE_SENSITIVE 
                then fullString.ToUpper()
                else fullString
    
    //Defines the current Guess
    let currentGuess =  if MULTIPLE 
                        then input
                        else (if input.Length > 0 then input.Chars(0).ToString() else " ")
       

    if ValidateGuess used (currentGuess)
    then currentGuess
    else GetGuess wordToFind used

//Recursive function comparing the hidden word against the guesses of the user, and prints the amount of guesses used
let rec play (wordToFind:string) (usedGuesses: string list) =
    let wordToFind = if not CASE_SENSITIVE then wordToFind.ToUpper() else wordToFind
    let hiddenWord = HiddenWord wordToFind usedGuesses
    printfn "%s" hiddenWord
    if wordToFind <> hiddenWord then 
        let guess = GetGuess wordToFind usedGuesses
        let used = guess::usedGuesses
        play wordToFind used
    else
        printfn "You guessed it! Using only %d guesses!" usedGuesses.Length


//Function for playing the game, aswell as the states of the game for stopping or playing it.
type playGameState = Play=0 | Stop=1 | Error=2
let StartGame = 
    
    //Mutable object that controls the game state
    let mutable playGame = playGameState.Play

    //while loop for our game
    while playGame = playGameState.Play do
        System.Console.Clear()
        printfn "Welcome to Word Guesser"    
        let wordlist = if(not MULTIPLE)
                        then WORDS |> List.filter(fun x -> not (x.Contains(" ")))
                        else WORDS
        let word = wordlist.[Random().Next(wordlist.Length)]
        printfn "The length of the word is %d" word.Length
        let usedWords = []
        do play word usedWords
        printfn "Do you want to play again? Y/N" 
        
        //Input for starting the game, if Y then the game state changes to playing, if N then game state changes to stop.
        //If its anything else it gives and error
        let input = Console.ReadKey(true).KeyChar |> Char.ToUpper
        playGame <- if input= 'Y' then playGameState.Play
                    elif input |> Char.ToUpper = 'N' then playGameState.Stop
                    else playGameState.Error

        //While loop for the error game state stating you have to chose between Y and N
        while playGame = playGameState.Error do            
            printfn "Error, please choose between Y or N"  
            let input = Console.ReadKey(true).KeyChar |> Char.ToUpper
            playGame <- if input= 'Y' then playGameState.Play
                        elif input |> Char.ToUpper = 'N' then playGameState.Stop
                        else playGameState.Error

module WordGuesser

open System;;
open Config;;

//Creates the hiddenword for the user
//let HiddenWord (wordToFind:string) (listOfGuesses : string list) =
//    String.iter(fun g -> wordToFind.Replace(g,HIDDEN.ToString())) listOfGuesses

   //wordToFind |> String.map (fun char -> 
   //   if Seq.exists ((=) char) listOfGuesses 
   //   then char 
   //   else HIDDEN);;
let HiddenWord (wordToFind:string) (listOfGuesses : string list) =
    let rec ReplaceWordFromStringList = function
    |("", []) -> ""
    |(s,[]) -> s
    |(s,n::l) -> ReplaceWordFromStringList (s.Replace(n, HIDDEN.ToString() |> String.replicate (String.length n)), l)

    let rec GetHiddenIndex = function
    |("", [],r) -> []
    |(s,[],r) -> r
    |(s,n::l,r) -> GetHiddenIndex (s.Replace(n, HIDDEN.ToString() |> String.replicate (String.length n)), l,r)
    

    //let wordToFind = "tritriangle"
    //let listOfGuesses = ["tri"; "ria";]
    //let foundIndex = listOfGuesses |> List.fold (fun s w ->  (wordToFind.IndexOf(w),wordToFind.IndexOf(w)+w.Length)::s) [] |> List.rev
    //let hiddenWord = foundIndex |> List.fold (fun s i -> s + if (i fst) > -1 then  ) ""
    


    let MakeHiddenWord word usedWords =
        List.zip(word   |> Seq.toList 
                        |> List.map string) 
                (ReplaceWordFromStringList (word,usedWords) |> Seq.toList 
                                                            |> List.map string)
    (MakeHiddenWord wordToFind listOfGuesses) |> List.fold (fun s (w,i) -> s + if w <> i || w = " " then w else HIDDEN.ToString()) "";;
//    let chars = s
//let upperChars = Seq.map System.Char.ToUpper chars
//let strChars = Seq.map string upperChars
//let result = String.concat "" strChars



//Check if the input is valid, haven't been used before
let ValidateGuess (usedGuesses: string list) (currentGuess) =
    not (usedGuesses |> List.exists ((=) currentGuess))

//Reads the user input and returns the input
let rec GetGuess used =
    printf "Used %A. Guess: " used
    
    let mutable input = Console.ReadKey(true)
    let mutable fullString = ""
    while(input.Key <> ConsoleKey.Enter) do 
        if input.Key = ConsoleKey.Backspace
        then fullString <- fullString.Remove(fullString.Length-1)
        else fullString <- fullString + input.KeyChar.ToString()
        printf "%s" (input.KeyChar.ToString())
        input <- Console.ReadKey(true)

        if(HELP && input.Modifiers = ConsoleModifiers.Control && input.Key = ConsoleKey.H) then
                Console.WriteLine("Help should arrive");

    let input = if not CASE_SENSITIVE 
                then fullString.ToUpper()
                else fullString
    
    let currentGuess =  if MULTIPLE 
                        then input
                        else (if input.Length > 0 then input.Chars(0).ToString() else "")
       

    if ValidateGuess used (currentGuess)
    then currentGuess
    else GetGuess used


let rec play (wordToFind:string) (usedGuesses: string list) =
    let wordToFind = if not CASE_SENSITIVE then wordToFind.ToUpper() else wordToFind
    let hiddenWord = HiddenWord wordToFind usedGuesses
    printfn "%s" hiddenWord
    if wordToFind <> hiddenWord then 
        let guess = GetGuess usedGuesses
        let used = guess::usedGuesses
        play wordToFind used
    else
        printfn "You guessed it! Using only %d guesses!" usedGuesses.Length
      //if wordToFind.IndexOf(guess) > 0
      //then play wordToFind used guesses
      //else play wordToFind used (guesses+1)


type playGameState = Play=0 | Stop=1 | Error=2
let StartGame = 
    let mutable playGame = playGameState.Play
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
        let input = Console.ReadKey(true).KeyChar |> Char.ToUpper
        playGame <- if input= 'Y' then playGameState.Play
                    elif input |> Char.ToUpper = 'N' then playGameState.Stop
                    else playGameState.Error
        while playGame = playGameState.Error do            
            printfn "Error, please choose between Y or N"  
            let input = Console.ReadKey(true).KeyChar |> Char.ToUpper
            playGame <- if input= 'Y' then playGameState.Play
                        elif input |> Char.ToUpper = 'N' then playGameState.Stop
                        else playGameState.Error

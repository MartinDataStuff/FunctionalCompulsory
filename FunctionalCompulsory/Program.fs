
// Learn more about F# at http://fsharp.org

open System
open WordGuesser
open FSharp.Control
open System.Threading

[<EntryPoint>]
let main argv =
    Console.TreatControlCAsInput <- true
    Console.WriteLine("Press any combination of CTRL, ALT, and SHIFT, and a console key.")
    Console.WriteLine("Press the Escape (Esc) key to quit: \n")

    //let mutable KeepGettingInput = true
    //while KeepGettingInput do
    //    let cki = Console.ReadKey(false)

    //    match cki.Modifiers with
    //    | ConsoleModifiers.Control  -> printf "CTRL+"
    //    | _ -> printf "not recognized modifier"

    //    let keyPressed = cki.Key.ToString()
    //    printfn "%s" keyPressed //Keys er tal, bogstaver, muligvis alle characters.

    

    
    do  StartGame

    printfn "Hello World from F#!"
    0 // return an integer exit code

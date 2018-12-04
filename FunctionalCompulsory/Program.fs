
// Learn more about F# at http://fsharp.org

open System
open WordGuesser
open FSharp.Control
open System.Threading

[<EntryPoint>]
let main argv =
    
    do  StartGame

    printfn "Hello World from F#!"
    0 // return an integer exit code

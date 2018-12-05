
// Learn more about F# at http://fsharp.org

open System
open WordGuesser
open FSharp.Control
open System.Threading

[<EntryPoint>]
let main argv =
    
    do  StartGame
    0 // return an integer exit code

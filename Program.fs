// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open DownDog.FileDownloader
open MapDownDogHistoryToLesson
open PlotYogaStats
open Microsoft.Extensions.Configuration
// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

let configuration = ConfigurationBuilder().AddJsonFile("appsettings.json").Build()

[<EntryPoint>]
let main argv =
    let message = from "F#" // Call the function
    printfn "Hello world %s" message
    downloadFileToDisk 
    plotLessonHistory yogaLessons 
    0 // return an integer exit code
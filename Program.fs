// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open DownDog.FileDownloader
open MapDownDogHistoryToLesson
open PlotYogaStats
open Microsoft.Extensions.Configuration
open ArgumentParser

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

let configuration = ConfigurationBuilder().AddJsonFile("appsettings.json").Build()

[<EntryPoint>]
let main argv =
    let plot = parseArguments argv
    downloadFileToDisk
    match plot with
       | PlotEnum.History ->  plotLessonHistory yogaLessons
       | PlotEnum.Music -> () 
       | _ -> ()
    0 // return an integer exit code
// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open DownDog.FileDownloader
open MapDownDogHistoryToLesson
open MapDownDogSongsToYogaMusicCharts
open PlotYogaStats
open Microsoft.Extensions.Configuration
open CommandLineArguments
open Argu
open System

[<EntryPoint>]
let main argv =

    let configuration =
        ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build()

    let errorHandler =
        ProcessExiter(
            colorizer =
                function
                | ErrorCode.HelpText -> None
                | _ -> Some ConsoleColor.Red
        )

    let parser =
        ArgumentParser.Create<PlotArguments>(programName = "donwDogPlot", errorHandler = errorHandler)

    let results = parser.ParseCommandLine argv

    let plot = results.GetResult(Plot)

    downloadFileToDisk

    match plot with
    | PlotEnum.History -> plotLessonHistory yogaLessons
    | PlotEnum.Music -> printyogaMusicCharts
    | _ -> ()

    0 // return an integer exit code

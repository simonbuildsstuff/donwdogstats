module ArgumentParser

open Argu
open System

type PlotEnum =
    | History = 0
    | Music = 1

type PlotArguments =
    | [<Mandatory; AltCommandLine("-p")>] Plot of PlotEnum

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Plot _ -> "Specify either the  history or music statistic to plot."

let parseArguments argv =
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

    results.GetResult(Plot)

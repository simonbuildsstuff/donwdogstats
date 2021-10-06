module CommandLineArguments

open Argu

type PlotEnum =
    | History = 0
    | Music = 1

type PlotArguments =
    | [<Mandatory; AltCommandLine("-p")>] Plot of PlotEnum

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Plot _ -> "Specify either the  history or music statistic to plot."

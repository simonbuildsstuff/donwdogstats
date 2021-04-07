module PlotYogaStats

open XPlot.Plotly
open XPlot.GoogleCharts

let plotLessonHistory (yogaLessons: List<Lesson>) : unit =

    let x =
        yogaLessons |> List.map (fun elem -> elem.date)

    let y =
        yogaLessons
        |> List.map (fun elem -> elem.duration)

    let layout =
        Layout(
            title = "Yoga lessons taken during covid",
            xaxis = Xaxis(tickfont = Font(size = 14., color = "rgb(107, 107, 107)")),
            yaxis = Yaxis(tickfont = Font(size = 14., color = "rgb(107, 107, 107)"))

        )

    XPlot.Plotly.Bar(x = x, y = y)
    |> XPlot.Plotly.Chart.Plot
    |> XPlot.Plotly.Chart.WithLayout layout
    |> XPlot.Plotly.Chart.WithWidth 1920
    |> XPlot.Plotly.Chart.WithHeight 1080
    |> XPlot.Plotly.Chart.Show


let plotYogaMusicCharts (yogaMusicCharts: list<Song * int>) : unit =


    let artist =
        yogaMusicCharts
        |> List.map (fun (x, _) -> (string x.title, string x.artist))
        |> List.map (fun (x, y) -> x, y :> value)

    let spotifyUrl =
        yogaMusicCharts
        |> List.map
            (fun (x, _) ->
                (string x.title,
                 match x.spotifyUrl with
                 | None -> ""
                 | Some u -> string u))
        |> List.map (fun (x, y) -> x, y :> value)

    let played =
        yogaMusicCharts
        |> List.map (fun (x, y) -> (string x.title, y))
        |> List.map (fun (x, y) -> x, y :> value)

    [ artist; spotifyUrl; played ]
    |> Chart.Table
    |> Chart.WithOptions(Options(showRowNumber = true))
    |> Chart.WithLabels [ "Title"; "Artist"; "Spotify URL"; "Played" ]
    |> Chart.Show

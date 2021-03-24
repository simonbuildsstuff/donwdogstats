module PlotYogaStats

open DomainTypes
open XPlot.Plotly

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

    Bar(x = x, y = y)
    |> Chart.Plot
    |> Chart.WithLayout layout
    |> Chart.WithWidth 1920
    |> Chart.WithHeight 1080
    |> Chart.Show

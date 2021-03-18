#r "nuget: SecretHub"
#r "nuget: Http.fs, 5.4.0"
#r "nuget: FSharp.Data"
#r "nuget: NodaTime"
#r "nuget: XPlot.Plotly, 4.0.0"
#r "nuget: XPlot.GoogleCharts, 3.0.1"
#r "nuget: XPlot.GoogleCharts.Deedle, 3.0.1"

open Hopac
open HttpFs.Client
open FSharp.Data
open System.IO
open NodaTime
open XPlot.Plotly

(*
    SecretHub: Get Cookie Credentials for DownDog
*)
let secretHubClient = new SecretHub.Client()

let secret =
    secretHubClient.Resolve("secrethub://simonbuildsstuff/downdog/cookiecreds")

(*
    Download DownDog History
*)
let body =
    Request.createUrl Post "https://www.downdogapp.com/json/history"
    |> Request.setHeader (
        ContentType
            (ContentType.parse "application/x-www-form-urlencoded")
                .Value
    )
    |> Request.setHeader (Custom("Cookie", secret))
    |> Request.responseAsString
    |> run


[<Literal>]
let fileName = "downdoghistory.json"

File.WriteAllText(fileName, body)

(*
    Get Typed Version of DownDog History with Typeprovider
*)
type DownDogHistory = JsonProvider<fileName, SampleIsList=true, ResolutionFolder=__SOURCE_DIRECTORY__>

let downDogHistory = DownDogHistory.GetSamples()
let historyItems = downDogHistory.[0].Items


(*
    Obtain Yoga Lessons from History
*)
type LessonId = string
type LessonCategory = string
type LessonLevel = string
type LessonDuration = int64
type LessonFocus = string
type LessonDate = Instant

type DownDowgLesson =
    { lessonId: LessonId
      category: LessonCategory
      level: LessonLevel
      focus: Option<LessonFocus>
      duration: LessonDuration
      date: LessonDate }

type ObtainSelectorValue = array<DownDogHistory.Selector> -> string -> string
type ObtainSelectorValueOption = array<DownDogHistory.Selector> -> string -> Option<string>
type ObtainDownDogLesson = DownDogHistory.Item -> DownDowgLesson

let obtainSelectorValue : ObtainSelectorValue =
    fun selectors selectorType ->
        selectors
        |> Array.find (fun elem -> elem.Type = selectorType)
        |> (fun selector -> selector.Type)

let obtainSelectorValueOption : ObtainSelectorValueOption =
    fun selectors selectorType ->
        selectors
        |> Array.tryFind (fun elem -> elem.Type = selectorType)
        |> (fun selectorOption ->
            match selectorOption with
            | None -> None
            | Some selector -> Some selector.Type)

let obtainLessonDate (timeStamp: DownDogHistory.Timestamp) =
    Instant.FromUnixTimeSeconds(int64 (floor timeStamp.Seconds))

let obtainLessonDuration (totalTime: DownDogHistory.TotalTime) = int64 (floor totalTime.Seconds)

let obtainDownDogLesson : ObtainDownDogLesson =
    fun item ->
        { lessonId = item.SequenceId
          category = obtainSelectorValue item.Selectors "CATEGORY"
          level = obtainSelectorValue item.Selectors "LEVEL"
          focus = obtainSelectorValueOption item.Selectors "FOCUS_AREA"
          duration = obtainLessonDuration item.TotalTime
          date = obtainLessonDate item.Timestamp }

let yogaLessons =
    historyItems
    |> Array.filter (fun elem -> elem.AppType = "ORIGINAL")
    |> Array.map (obtainDownDogLesson)
    |> Array.toList

for lesson in yogaLessons do
    printfn $"lessonId: {lesson.lessonId}"

    printfn $"Timestamp: {lesson.date.ToDateTimeUtc}"

(*
    Plot graphs
*)


let x = yogaLessons |> List.map (fun elem -> elem.date )
let y = yogaLessons |> List.map (fun elem -> Duration.FromSeconds(elem.duration).Minutes)

let layout = Layout(title = "Yoga lessons taken during covid")

let chart1 =
    Bar(
        x = x,
        y = y
    )
    |> Chart.Plot
    |> Chart.WithLayout layout
    |> Chart.WithWidth 700
    |> Chart.WithHeight 500
    |> Chart.Show
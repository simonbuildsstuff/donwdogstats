module MapDownDogHistoryToLesson

open NodaTime
open DownDog.JsonTypeProvider

let yogaLessons =

    let obtainSelectorValue (selectorType: string) (selectors: array<DownDogHistory.Selector>) : string =
        selectors
        |> Array.find (fun elem -> elem.Type = selectorType)
        |> (fun selector -> selector.Label)

    let obtainSelectorValueOption (selectorType: string) (selectors: array<DownDogHistory.Selector>) : Option<string> =
        selectors
        |> Array.tryFind (fun elem -> elem.Type = selectorType)
        |> (fun selectorOption ->
            match selectorOption with
            | None -> None
            | Some selector -> Some selector.Label)

    let obtainLessonDate (timeStamp: DownDogHistory.Timestamp) =
        Instant.FromUnixTimeSeconds(int64 (floor timeStamp.Seconds))

    let obtainLessonDuration (selectorLabel: string) =
        String.split ' ' selectorLabel
        |> List.first
        |> (fun x ->
            match x with
            | None -> 0
            | Some y -> int y)

    let obtainLessonDurationFromSelectors =
        obtainSelectorValue "LENGTH"
        >> obtainLessonDuration

    let obtainLesson (item: DownDogHistory.Item) : Lesson =
        { lessonId = item.SequenceId
          category = obtainSelectorValue "CATEGORY" item.Selectors
          level = obtainSelectorValue "LEVEL" item.Selectors
          focus = obtainSelectorValueOption "FOCUS_AREA" item.Selectors
          duration = obtainLessonDurationFromSelectors item.Selectors
          date = obtainLessonDate item.Timestamp }

    historyItems
    |> Array.filter (fun elem -> elem.AppType = "ORIGINAL")
    |> Array.map obtainLesson
    |> Array.toList

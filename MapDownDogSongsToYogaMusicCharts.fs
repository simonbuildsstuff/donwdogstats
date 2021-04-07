module MapDownDogSongsToYogaMusicCharts

open DownDog.JsonTypeProvider
open System

let obtainTitle (downDogTitle: DownDogHistory.IntOrBooleanOrString) =
    match downDogTitle.String with
    | None ->
        match downDogTitle.Boolean with
        | None ->
            match downDogTitle.Number with
            | None -> ""
            | Some n -> string n
        | Some b -> string b
    | Some t -> t

let obtainSpotifyUri (downDogSpotifyUri: string option) =
    match downDogSpotifyUri with
    | None -> None
    | Some s -> Some (new UriBuilder(s)).Uri

let obtainSong (downDogSong: DownDogHistory.Song) : Song =
    { id = downDogSong.Id
      artist = downDogSong.Artist
      title = obtainTitle downDogSong.Title
      spotifyUrl = obtainSpotifyUri downDogSong.SpotifyUrl }


let obtainYogaMusicCharts (historyItems: array<DownDogHistory.Item>) =
    historyItems
    |> Array.collect (fun element -> element.Songs)
    |> Array.map obtainSong
    |> Array.countBy (fun elem ->  if (elem = elem) then elem else elem  )

let yogaMusicCharts = obtainYogaMusicCharts historyItems


let printyogaMusicCharts =
    for item in yogaMusicCharts do
        printfn "%A" item

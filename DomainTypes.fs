[<AutoOpen>]
module DomainTypes

open NodaTime
open System

type LessonId = string
type LessonCategory = string
type LessonLevel = string
type LessonDuration = int
type LessonFocus = string
type LessonDate = Instant

type Lesson =
    { lessonId: LessonId
      category: LessonCategory
      level: LessonLevel
      focus: Option<LessonFocus>
      duration: LessonDuration
      date: LessonDate }



type SongId = string
type Artist = string
type Title = string
type SpotifyUrl = Uri option

type Song =
    { id: SongId
      artist: Artist
      title: Title
      spotifyUrl: SpotifyUrl }
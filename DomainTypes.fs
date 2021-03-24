module DomainTypes

open NodaTime

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

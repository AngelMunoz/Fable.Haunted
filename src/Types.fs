module Fable.Haunted.Types

[<Interface>]
type Context<'T> =
    abstract member Provider : obj
    abstract member Consumer : obj
    abstract member defaultValue : 'T

[<Interface>]
type Ref<'T> =
    abstract member current : 'T with get, set

type Reducer<'State, 'Action> = 'State -> 'Action -> 'State

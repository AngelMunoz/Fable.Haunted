module Fable.Haunted

open Fable.Core.JsInterop

open Lit

let defineComponent (name: string) (comp: obj) : unit = importMember "./interop.js"


type Haunted() =
    static member useState(init: 'T) : 'T * ('T -> unit) = importMember "haunted"
    static member useState(init: unit -> 'T) : 'T * ('T -> unit) = importMember "haunted"
    static member useEffect(effect: unit -> unit, deps: 'T []) : unit = importMember "haunted"
    static member useEffect(effect: unit -> unit -> unit, deps: 'T []) : unit = importMember "haunted"

    static member Component(renderFn: obj) : TemplateResult = import "component" "haunted"

    static member Component(renderFn: obj, ?opts: obj) : TemplateResult = import "component" "haunted"

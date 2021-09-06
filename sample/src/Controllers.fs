module Controllers

open System
open Fable.Core
open Browser.Types
open Browser.Dom
open Haunted.Types

[<AttachMembers>]
type MouseController(host: ReactiveControllerHost) as this =
    [<DefaultValue>]
    val mutable y: float

    [<DefaultValue>]
    val mutable x: float

    [<DefaultValue>]
    val mutable private _onMouseMove: Event -> unit

    do
        host.addController (this)
        this.x <- 0.
        this.y <- 0.

        this._onMouseMove <-
            fun (e: Event) ->
                let e = e :?> MouseEvent
                this.x <- e.x
                this.y <- e.y
                host.requestUpdate ()


    interface ReactiveController with
        member _.hostConnected() =
            window.addEventListener ("mousemove", this._onMouseMove)

        member _.hostDisconnected() =
            window.removeEventListener ("mousemove", this._onMouseMove)

        member _.hostUpdate() = ()

        member _.hostUpdated() = ()


[<AttachMembers>]
type ClockController(host: ReactiveControllerHost, speed: int) as this =
    [<DefaultValue>]
    val mutable time: DateTime

    [<DefaultValue>]
    val mutable speed: int


    let mutable intervalId: int option = None

    do
        host.addController (this)
        this.time <- DateTime.Now
        this.speed <- speed


    member this.updateSpeed(speed: int) =
        this.speed <- speed
        host.requestUpdate ()

        intervalId
        |> Option.map JS.clearInterval
        |> ignore

        intervalId <-
            JS.setInterval
                (fun _ ->
                    this.time <- DateTime.Now

                    host.requestUpdate ())
                this.speed
            |> Some


    interface ReactiveController with
        member _.hostConnected() =
            intervalId <-
                JS.setInterval
                    (fun _ ->
                        this.time <- DateTime.Now


                        host.requestUpdate ())
                    this.speed
                |> Some

        member _.hostDisconnected() =
            intervalId
            |> Option.map JS.clearInterval
            |> ignore

        member _.hostUpdate() = ()

        member _.hostUpdated() = ()

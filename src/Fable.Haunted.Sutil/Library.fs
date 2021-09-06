module Haunted.Sutil

open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
open Haunted.Types
open Sutil.DOM

let private haunted
    (opts: {| render: obj -> obj -> unit |})
    : {| ``component``: obj -> obj option -> SutilElement
         createContext: 'T -> Context<'T> |} =
    importDefault "haunted"

/// use the Sutil mountOn function rather than the default from haunted
let private customHaunted<'T> : {| ``component``: obj -> option<obj> -> SutilElement
                                   createContext: 'T -> Context<'T> |} =
    haunted
        {| render =
               fun sutilElement container ->
                   mountOn (exclusive (unbox sutilElement)) (unbox container)
                   |> ignore |}

type Haunted with
    /// <summary>
    /// Components are functions that contain state and return HTML via lit-html or hyperHTML.
    /// Through the component() they become connected to a lifecycle that keeps the HTML up-to-date when state changes.
    /// </summary>
    static member Component(renderFn: obj) =
        customHaunted.``component`` renderFn None

    /// <summary>
    /// Components are functions that contain state and return HTML via lit-html or hyperHTML.
    /// Through the component() they become connected to a lifecycle that keeps the HTML up-to-date when state changes.
    /// </summary>
    static member Component(renderFn: obj, ?opts: obj) =
        customHaunted.``component`` renderFn opts

    /// <summary>
    /// A helper function that returns a context value.
    /// </summary>
    /// <example>
    ///     let theme = Haunted.createContext "dark"
    ///
    ///     // register the provider and the consumer
    ///     defineComponent 'theme-provider' theme.Provider
    ///     defineComponent 'theme-provider' theme.Consumer
    ///     // use yout context
    /// </example>
    static member createContext(value: 'T) : Context<'T> = customHaunted.createContext value

namespace Haunted.Types

/// <summary>
/// To be used with `useContext`, once context exists, you can use it to register
/// consumers and providers as custom elements.
/// </summary>
/// <example>
///     let theme = Haunted.createContext "dark"
///
///     // register the provider and the consumer
///     defineComponent 'theme-provider' theme.Provider
///     defineComponent 'theme-provider' theme.Consumer
///
///     // (optional) create and define a custom consumer
///     let Consumer () =
///         let context = Haunted.useContext theme
///         context
///
///     defineComponent 'my-consumer' (Haunted.Component Consumer)
///
///     let renderValue value =
///         html $"""&lt;h1>{value}&lt;/h1>"""
///
///     let App () =
///
///         let theme, setTheme = Hautned.useState "light"
///
///         html $"""
///              &lt;select value={theme} @change={fun _ -> setTheme(event.target.value)}>
///               &lt;option value="dark">Dark&lt;/option>
///               &lt;option value="light">Light&lt;/option>
///             &lt;/select>
///
///             &lt;theme-provider .value={theme}>
///               &lt;my-consumer>&lt;/my-consumer>
///
///               &lt;!-- creates context with inverted theme -->
///               &lt;theme-provider .value={theme = 'dark' ? 'light' : 'dark'}>
///                 &lt;theme-consumer
///                   .render={renderValue}
///                 >&lt;/theme-consumer>
///               &lt;/theme-provider>
///             &lt;/theme-provider>
///        """
/// </example>
[<Interface>]
type Context<'T> =
    /// <summary>
    /// A render function that can be registered to produce a provider component.
    /// </summary>
    abstract member Provider : obj
    /// <summary>
    /// A render function that can be registered to produce a consumer component.
    /// </summary>
    abstract member Consumer : obj
    /// <summary>
    /// The context value.
    /// </summary>
    abstract member defaultValue : 'T

/// <summary>
/// a type returned by `useRef` provides a mutable reference to a value
/// that can be used update values without require a re-render of your component code.
/// </summary>
[<Interface>]
type Ref<'T> =
    /// <summary>
    ///  reference to the current value, updating this value this won't trigger a re-render
    /// </summary>
    abstract member current : 'T with get, set

/// <summary>
///  To be used with `useReducer` to do "elmish/redux" like updates to components.
/// </summary>
type Reducer<'State, 'Action> = 'State -> 'Action -> 'State

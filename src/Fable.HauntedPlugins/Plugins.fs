namespace Fable.Haunted

open Fable.AST

[<assembly:Fable.ScanForPlugins>]
do()

module internal Util =
    let makeImport (selector: string) (path: string) =
        Fable.Import({ Selector = selector
                       Path = path
                       IsCompilerGenerated = true }, Fable.Any, None)

    type MemberInfo(?info: Fable.MemberInfo,
                    ?isValue: bool) =
        let infoOr f v =
            match info with
            | Some i -> f i
            | None -> v
        let argOrInfoOr arg f v =
            match arg, info with
            | Some arg, _ -> arg
            | None, Some i -> f i
            | None, None -> v
        interface Fable.MemberInfo with
            member _.IsValue = argOrInfoOr isValue (fun i -> i.IsValue) false
            member _.Attributes = infoOr (fun i -> i.Attributes) Seq.empty
            member _.HasSpread = infoOr (fun i -> i.HasSpread) false
            member _.IsPublic = infoOr (fun i -> i.IsPublic) true
            member _.IsInstance = infoOr (fun i -> i.IsInstance) true
            member _.IsMutable = infoOr (fun i -> i.IsMutable) false
            member _.IsGetter = infoOr (fun i -> i.IsGetter) false
            member _.IsSetter = infoOr (fun i -> i.IsSetter) false
            member _.IsEnumerator = infoOr (fun i -> i.IsEnumerator) false
            member _.IsMangled = infoOr (fun i -> i.IsMangled) false

/// Generates a virtual component out of a render function.
/// See: https://hauntedhooks.netlify.app/docs/guides/virtual/
type VirtualComponentAttribute() =
    inherit Fable.MemberDeclarationPluginAttribute()
    override _.FableMinimumVersion = "3.0"
    override _.TransformCall(_,_,expr) = expr
    override _.Transform(_compiler, _file, decl) =
        let t = Fable.Any // TODO: Type
        let virtualFn = Util.makeImport "virtual" "haunted"
        let fn = Fable.Delegate(decl.Args, decl.Body, None)
        let body = Fable.CurriedApply(virtualFn, [fn], t, None)
        { decl with
            Info = Util.MemberInfo(decl.Info, isValue=true)
            Args = []
            Body = body }

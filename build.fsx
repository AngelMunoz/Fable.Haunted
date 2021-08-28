#!/usr/bin/env -S dotnet fsi
#r "nuget: Fake.DotNet.Cli, 5.20.4"
#r "nuget: Fake.IO.FileSystem, 5.20.4"
#r "nuget: Fake.Core.Target, 5.20.4"
#r "nuget: Fake.DotNet.MsBuild, 5.20.4"
#r "nuget: MSBuild.StructuredLogger, 2.1.507"
#r "nuget: System.IO.Compression.ZipFile, 4.3.0"
#r "nuget: System.Reactive"

open System
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

// https://github.com/fsharp/FAKE/issues/2517#issuecomment-727282959
Environment.GetCommandLineArgs()
|> Array.skip 2 // skip fsi.exe; build.fsx
|> Array.toList
|> Context.FakeExecutionContext.Create false "build.fsx"
|> Context.RuntimeContext.Fake
|> Context.setExecutionContext

let output = "./dist"

Target.initEnvironment ()
Target.create "Clean" (fun _ -> !! "dist" |> Shell.cleanDirs)

let publish proj =
    DotNet.pack
        (fun opts ->
            { opts with
                  Configuration = DotNet.BuildConfiguration.Release
                  OutputPath = Some $"{output}" })
        ("src/" + proj)

Target.create "Default" (fun _ -> publish "Fable.Haunted")

"Clean" ==> "Default"

Target.runOrDefault "Default"

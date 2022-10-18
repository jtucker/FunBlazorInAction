[<AutoOpen>]
module Hooks

open Fun.Blazor
type IComponentHook with

    member hook.LoadTrails () =
        Option.None
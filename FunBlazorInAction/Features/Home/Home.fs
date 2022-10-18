//hot-reload
[<AutoOpen>]
module Home

open Fun.Blazor
open Fun.Result
open FSharp.Data.Adaptive
open System.Net.Http
open System.Net.Http.Json
open Microsoft.AspNetCore.Components.WebAssembly
open Microsoft.AspNetCore.Components.Web

type Trail =
    { Id: int
      Name: string
      Description: string
      Image: string
      Location: string
      TimeInMinutes: int
      Length: int
      Route: RouteInstruction list }

and RouteInstruction = { Stage: int; Description: string }

let formatTime timeInMinutes =
    $"{timeInMinutes / 60}h {timeInMinutes % 60}m"

let trailDetails (trail: Trail) =
    Template.html
        $"""
        <div class="drawer-wrapper">
            <div class="drawer-mask"></div>
            <div class="drawer">
                <div class="drawer-content">
                    <img src="{trail.Image}" />
                    <div class="trail-details">
                        <h3>{trail.Name}</h3>
                        <h6 class="mb-3 text-muted">
                            <span class="oi oi-map-marker"></span>
                            {trail.Location}
                        </h6>
                    </div>
                </div>
                <div class="drawer-controls">
                    <button class="btn btn-secondary" >Close</button>
                </div>
            </div>
        </div>
        """

let trailCard (trail: Trail) =
    div {
        classes [ "card"; "shadow" ]
        childContent
            [ img {
                  src trail.Image
                  classes [ "card-img-top" ]
                  alt trail.Name
              }
              div {
                  class' "card-body"

                  childContent
                      [ h5 {
                            class' "card-title"
                            trail.Name
                        }
                        h6 {
                            classes [ "card-subtitle"; "mb-3"; "text-muted" ]
                            childContent [ span { classes [ "oi"; "oi-map-marker" ] }; html.text trail.Location ]
                        }
                        div {
                            classes [ "d-flex"; "justify-content-between" ]

                            childContent
                                [ span { classes [ "oi"; "oi-click"; "mr-2" ] }
                                  html.text (formatTime trail.TimeInMinutes) ]
                        }
                        adaptiview () {
                            button {
                                classes [ "btn"; "btn-primary" ]
                                onclick (fun (_) -> ())
                                childContent [ i { classes [ "bi"; "bi-binoculars" ] } ]
                            }
                        } ]
              } ]
    }

let templateExample (trail:Trail) =
    Template.html $"""
        <img src="{trail.Image}">
    """

let homePage =
    html.fragment
        [ PageTitle'() { "Blazing Trails - Home" }
          html.comp (fun (hook: IComponentHook, http: HttpClient) ->
              let trails = cval DeferredState<Trail list, string>.NotStartYet
              let selectedTrail = cval DeferredState<Trail, string>.NotStartYet

              hook.OnInitialized.Add(fun _ ->
                  task {
                      trails.Publish DeferredState.Loading
                      let! trailsFromJson = http.GetFromJsonAsync<Trail list>("trails/trail-data.json")
                      DeferredState.Loaded trailsFromJson |> trails.Publish
                  }
                  |> ignore)

              adaptiview () {
                  match! trails with
                  | DeferredState.NotStartYet
                  | DeferredState.Loading -> p { "Loading Trails..." }
                  | DeferredState.Reloading trails
                  | DeferredState.Loaded trails ->
                      
                      Template.html $"""
                        <div class="grid">
                        { 
                            html.fragment [ 
                                for trail in trails do 
                                    trailCard trail
                            ]
                        }
                        </div>
                    """
                   | DeferredState.LoadFailed msg -> span { msg }
              }) ]

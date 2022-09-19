//hot-reload
[<AutoOpen>]
module Home

open Fun.Blazor
open FSharp.Data.Adaptive
open System.Net.Http
open System.Net.Http.Json

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

let trailDetails (trail: Trail option) (setTrail: Trail option -> unit) = 
    match trail with
    | Some trail -> 
        Template.html $"""
        <div class="drawer-wrapper slide">
            <div class="drawer-mask"></div>
            <div class="drawer">
                <div class "drawer-content">
                    <img src="{ trail.Image }" />
                    <div class="trail-details">
                        <h3>{trail.Name}</h3>
                        <h6 class="mb-3 text-muted">
                            <span class="oi oi-map-marker"></span>
                            {trail.Location}
                        </h6>
                    </div>
                </div>
                <div class="drawer-controls">
                    <button class="btn btn-secondary" onclick={fun _ -> setTrail (None)}>Close</button>
                </div>
            </div>
        </div>
        """
    | None -> html.none

let trailCard (trail: Trail) (setSelectedTrail: (Trail option -> unit)) =
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
                                classes ["btn"; "btn-primary"]
                                onclick (fun _ -> setSelectedTrail (Some (trail)))
                                childContent [
                                    i {
                                        classes ["bi"; "bi-binoculars"]
                                    }
                                ]}
                        }]
              } ]
    }

let homePage =
    html.fragment
        [ PageTitle' () { "Blazing Trails - Home" }
          html.comp (fun (hook: IComponentHook, http: HttpClient) ->
              let trails = cval []
              let selectedTrail = cval (Option<Trail>.None)
              
              hook.OnInitialized.Add(fun _ ->
                  task {
                      let! trailsFromJson = http.GetFromJsonAsync<Trail list>("trails/trail-data.json")
                      trails.Publish trailsFromJson
                  }
                  |> ignore)

              adaptiview () {
                let! selectedTrail, setSelectedTrail = selectedTrail.WithSetter ()             
                match! trails with
                | [] -> p { "Loading Trails..." }
                | x ->
                    div {
                        class' "grid"

                        childContent
                            [ for trail in x do
                                trailCard trail setSelectedTrail ]
                    }
              }) ]

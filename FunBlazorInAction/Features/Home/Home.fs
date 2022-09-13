//hot-reload
[<AutoOpen>]
module Home

open Fun.Blazor
open FSharp.Data.Adaptive
open System.Net.Http
open System.Net.Http.Json

type Trail ={
    Id: int
    Name: string
    Description: string
    Image: string
    Location: string
    TimeInMinutes: int
    Length: int
    Route: RouteInstruction list }

and RouteInstruction = {
    Stage: int
    Description: string }

let formatTime timeInMinutes =
    $"{timeInMinutes / 60}h {timeInMinutes % 60}m"

let trailCard (trail: Trail) =
    div {
        classes ["card"; "shadow"]
        childContent [
            img {
                src trail.Image
                classes ["card-img-top"]
                alt trail.Name
            }
            div {
                class' "card-body"
                childContent [
                    h5 {
                        class' "card-title"
                        trail.Name
                    }
                    h6 {
                        classes ["card-subtitle"; "mb-3"; "text-muted"]
                        childContent [
                            span {
                                classes ["oi"; "oi-map-marker"]
                            }
                            html.text trail.Location
                        ]
                    }
                    div {
                        classes ["d-flex"; "justify-content-between"]
                        childContent [
                            span {      
                                classes ["oi"; "oi-click"; "mr-2"]
                            }
                            html.text (formatTime trail.TimeInMinutes)  
                        ]
                    }
                ]
            }
        ]
    }

let homePage = 
    html.comp (fun (hook: IComponentHook, http: HttpClient) -> 
        let trails = cval []       
        hook.OnInitialized.Add(fun _ ->
            task {
                let! trailsFromJson = http.GetFromJsonAsync<Trail list>("trails/trail-data.json")
                trails.Publish trailsFromJson
            } |> ignore
        )

        adaptiview () {
            match! trails with
            | [] -> p {
                "Loading Trails..." } 
            | x -> div {
                class' "grid"
                childContent [ for trail in x do trailCard trail ]}
        }
    )


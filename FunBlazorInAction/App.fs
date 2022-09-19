// hot-reload
[<AutoOpen>]
module FunBlazorInAction.App

open Fun.Blazor
open Fun.Blazor.Router

let navBar =
    nav {
        classes [ "navbar"; "mb-5"; "shadow" ]

        a {
            class' "navbar-brand"
            href "/"
            img { src "/images/logo.png" }
        }
    }

let routes = html.route [ routeCi "/" Home.homePage ]

let app =
    div {
        navBar

        main {
         classes [ "container"; "mt-5"; "mb-5" ]
         childContent routes
        }
    }

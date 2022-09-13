// hot-reload
// hot-reload is the flag to let cli know this file should be included
// It has dependency requirement: the root is the app which is used in the Startup.fs
// All other files which want have hot reload, need to drill down to that file, and all the middle file should also add the '// hot-reload' flag at the top of taht file
[<AutoOpen>]
module ModeratelySized.App

open Fun.Blazor
open Fun.Blazor.Router

let navBar = 
    nav {
        classes ["navbar"; "mb-5"; "shadow";]
        a {
          class' "navbar-brand"
          href "/"
          img {
            src "/images/logo.png"
          }
        }
    }

let routes = 
    html.route [
        routeCi "/" Home.homePage
    ]

let app =
    div {
        navBar
        main {
            classes ["container"; "mt-5"; "mb-5";]
            childContent routes
        }
    }

    
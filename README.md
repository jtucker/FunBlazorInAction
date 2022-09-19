# Fun.Blazor in Action

A repo where I write the the application example in [Blazor In Action](https://www.manning.com/books/blazor-in-action) using [Fun.Blazor](https://slaveoftime.github.io/Fun.Blazor.Docs/documents/About) project template. 

## Chapter 2 Branch

You are currently on the Chapter 2 branch which is the completed excercise from Chapter 2 of the book. This is really just a scaffold and get familiar with the framework type of excercise. 

## Fun.Blazor Items

In order to use existing Web Components (e.g. PageTitle) within the computational expression style, you need to generate them via the `fun-blazor` cli tool. 

1. Install the cli tool
    ```bash
    > dotnet tool restore
    ```
1. Add the following attributes to the assembly `PackageReference`: 
    - `FunBlazor`
    - `FunBlazorStyle`
    - `FunBlazorNamespace`

        For Example:
        ```xml
        <PackageReference FunBlazorStyle="CE" FunBlazorNamespace="Microsoft.AspNetCore.Components" Include="Microsoft.AspNetCore.Components.Web" Version="6.0.8" />
        ```
1. Run the cli tool: `dotnet fun-blazor generate .\FunBlazorInAction.fsproj`

This gave me access to a new `PageTitle'` computational expression and now the `title` get's updated.

## Notes

- [X] How to bring in existing Blazor component? I need to research this some more as it doesn't seem obvious to me
- [ ] Build scripts, probably `fake`

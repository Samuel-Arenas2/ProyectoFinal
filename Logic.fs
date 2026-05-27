module ProyectoJuego.Logic

open System
open ProyectoJuego.Types 

let actualizarTick state =
    { state with Tick = state.Tick + 1 }

let actualizarMisiles state =
    if state.Misiles <> [] then 
        state.Misiles
        |> Seq.map (fun misil -> { misil with X = misil.X + 1 })
        |> Seq.filter (fun misil -> misil.X < Console.BufferWidth - 2)
        |> Seq.toList
        |> fun nuevosMisiles ->
            { state with Misiles = nuevosMisiles; RedibujarPantalla = true } 
    else
        state

let actualizarMisilesEnemigos state =
    if state.MisilesEnemigos <> [] then 
        state.MisilesEnemigos
        |> Seq.map (fun misil -> { misil with X = misil.X - 1 })
        |> Seq.filter (fun misil -> misil.X >= 0)
        |> Seq.toList
        |> fun nuevosMisiles ->
            { state with MisilesEnemigos = nuevosMisiles; RedibujarPantalla = true } 
    else
        state

let actualizarDisparoEnemigo state =
    if state.EnemigoEstado = Alive && state.Tick % 10 = 0 then 
        let nuevoMisil = { X = state.EnemigoX - 2; Y = state.EnemigoY }
        { state with MisilesEnemigos = nuevoMisil :: state.MisilesEnemigos; RedibujarPantalla = true }
    else
        state

let actualizarEnemigo state =
    if state.EnemigoEstado = Alive && state.Tick % 4 = 0 then 
        let nuevaY = state.EnemigoY + state.EnemigoDir
        match nuevaY with 
        | y when y > Console.BufferHeight - 1 -> Console.BufferHeight - 1, -1
        | y when y < 0 -> 0, 1
        | y -> y, state.EnemigoDir
        |> fun (y, dir) ->
            { state with EnemigoY = y; EnemigoDir = dir; RedibujarPantalla = true }
    else
        state

let detectarColisionConAlien state =
    state.MisilesEnemigos
    |> List.filter (fun misil -> not (misil.X = state.AlienX + 1 && misil.Y = state.AlienY))
    |> fun nuevosMisiles ->
        if nuevosMisiles.Length <> state.MisilesEnemigos.Length then 
            let nuevasVidas = state.Vidas - 1
            if nuevasVidas <= 0 then
                { state with Vidas = 0; Pantalla = GameOver; RedibujarPantalla = true }
            else
                { state with 
                    AlienState = Hit
                    Vidas = nuevasVidas
                    MisilesEnemigos = nuevosMisiles
                    RedibujarPantalla = true
                    ColisionAlien = state.Tick }
        else
            state

let detectarColisionConEnemigo state =
    state.Misiles
    |> List.filter (fun misil -> not (misil.X = state.EnemigoX - 1 && misil.Y = state.EnemigoY))
    |> fun nuevosMisiles ->
        if nuevosMisiles.Length <> state.Misiles.Length then 
            { state with 
                EnemigoEstado = Hit
                Puntos = state.Puntos + 100 
                Misiles = nuevosMisiles
                RedibujarPantalla = true
                ColisionEnemigo = state.Tick }
        else
            state

let resetAlien state =
    if state.AlienState = Hit then 
        let tiempo = state.Tick - state.ColisionAlien
        if tiempo >= 160 then 
            { state with AlienState = Alive; RedibujarPantalla = true }
        else
            state
    else
        state

let resetEnemigo state =
    if state.EnemigoEstado = Hit then 
        let tiempo = state.Tick - state.ColisionEnemigo
        if tiempo >= 160 then 
            { state with EnemigoEstado = Alive; RedibujarPantalla = true }
        else
            state
    else
        state
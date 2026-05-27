module ProyectoJuego.Input

open System
open ProyectoJuego.Types
open ProyectoJuego.SaveLoad

let procesarTecladoMenus key state =
    match key with
    | ConsoleKey.UpArrow -> { state with MenuSeleccion = max 0 (state.MenuSeleccion - 1); RedibujarPantalla = true }
    | ConsoleKey.DownArrow -> 
        // Límite de opciones dependiendo de la pantalla (Menú Inicio tiene 3, Pausa tiene 2)
        let limite = if state.Pantalla = MenuInicio then 2 else 1
        { state with MenuSeleccion = min limite (state.MenuSeleccion + 1); RedibujarPantalla = true }
    | ConsoleKey.Enter ->
        match state.Pantalla with
        | MenuInicio ->
            match state.MenuSeleccion with
            | 0 -> { state with Pantalla = Jugando; RedibujarPantalla = true }
            | 1 -> cargarPartida state
            | 2 -> { state with ProgramState = Terminated }
            | _ -> state
        | Pausa ->
            match state.MenuSeleccion with
            | 0 -> 
                // Seleccionó Guardar: guardamos y devolvemos al juego
                let estadoGuardado = guardarPartida state
                { estadoGuardado with Pantalla = Jugando; RedibujarPantalla = true }
            | 1 -> 
                // Seleccionó Salir: reiniciamos al menú principal
                { estadoInicial with RedibujarPantalla = true } 
            | _ -> state
        | GameOver ->
            // Al presionar ENTER en Game Over, reseteamos el juego al menú principal
            { estadoInicial with RedibujarPantalla = true }
        | _ -> state
    | ConsoleKey.Escape ->
        // Si ya estás en pausa y presionas ESC, vuelves al juego
        if state.Pantalla = Pausa then { state with Pantalla = Jugando; RedibujarPantalla = true } else state
    | _ -> state

let procesarTecladoAlien key state =
    match key with
    | ConsoleKey.Escape -> 
        // ¡Al presionar ESC mientras juegas, pasas a Pausa!
        { state with Pantalla = Pausa; MenuSeleccion = 0; RedibujarPantalla = true }
    | ConsoleKey.UpArrow -> { state with AlienY = max 0 (state.AlienY - 1); RedibujarPantalla = true }
    | ConsoleKey.DownArrow -> { state with AlienY = min (Console.BufferHeight - 1) (state.AlienY + 1); RedibujarPantalla = true }
    | ConsoleKey.LeftArrow -> { state with AlienX = max 0 (state.AlienX - 1); RedibujarPantalla = true }
    | ConsoleKey.RightArrow -> { state with AlienX = min (Console.BufferWidth - 2) (state.AlienX + 1); RedibujarPantalla = true }
    | ConsoleKey.Spacebar -> 
        let nuevoMisil = { X = state.AlienX + 2; Y = state.AlienY } // Projectile Range
        { state with Misiles = nuevoMisil :: state.Misiles; RedibujarPantalla = true }
    | _ -> state

let procesarTeclado state =
    if Console.KeyAvailable then
        let k = Console.ReadKey true
        match state.Pantalla with
        | Jugando -> procesarTecladoAlien k.Key state
        | _ -> procesarTecladoMenus k.Key state // Menú, Pausa y Game Over usan la misma lógica
    else state
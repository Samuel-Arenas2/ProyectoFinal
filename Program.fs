module Program

open System
open System.Threading
open ProyectoJuego.Types
open ProyectoJuego.Utils
open ProyectoJuego.Input
open ProyectoJuego.Logic // ¡Aquí traemos toda la física del juego!

let redibujarPantalla state =
    if state.RedibujarPantalla then
        match state.Pantalla with
        | MenuInicio -> dibujarMenuInicio state
        | Pausa -> dibujarPausa state
        | Jugando ->
            Console.Clear()
            dibujarHUD state
            dibujarAlien state
            dibujarEnemigo state
            
            // Dibujar los misiles aliados y enemigos
            state.Misiles |> List.iter (fun m -> mostrarMensaje m.X m.Y ConsoleColor.Cyan "-")
            state.MisilesEnemigos |> List.iter (fun m -> mostrarMensaje m.X m.Y ConsoleColor.Red "<")
            
        | GameOver -> 
            Console.Clear()
            dibujarTextoCentrado 10 ConsoleColor.Red "=== GAME OVER ==="
            dibujarTextoCentrado 12 ConsoleColor.White $"Puntos Finales: {state.Puntos}"
            dibujarTextoCentrado 14 ConsoleColor.Gray "Presiona ENTER para volver al menú"
        | _ -> ()
        { state with RedibujarPantalla = false }
    else state

let rec mainLoop state =
    // 1. Sentidos: Leer el teclado
    let estadoConInput = procesarTeclado state
    
    // 2. Cerebro: Procesar la física (¡Solo si estamos en la pantalla de juego!)
    let estadoActualizado =
        match estadoConInput.Pantalla with
        | Jugando ->
            estadoConInput
            |> actualizarTick
            |> actualizarMisiles
            |> actualizarMisilesEnemigos
            |> actualizarDisparoEnemigo
            |> actualizarEnemigo
            |> detectarColisionConAlien
            |> detectarColisionConEnemigo
            |> resetAlien
            |> resetEnemigo
        | _ -> estadoConInput
        
    // 3. Vista: Dibujar en la consola
    let estadoFinal = redibujarPantalla estadoActualizado

    // 4. Ciclo: Repetir todo
    if estadoFinal.ProgramState <> Terminated then
        Thread.Sleep 25
        mainLoop estadoFinal

[<EntryPoint>]
let main argv =
    Console.CursorVisible <- false
    mainLoop estadoInicial
    0
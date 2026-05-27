module ProyectoJuego.Utils

open System
open ProyectoJuego.Types 

let mostrarMensaje x y color (msg:string) =
    if x >= 0 && x < Console.BufferWidth && y >= 0 && y < Console.BufferHeight then
        Console.SetCursorPosition(x, y)
        Console.ForegroundColor <- color
        msg |> Console.Write
        Console.ResetColor()

let dibujarTextoCentrado y color texto =
    let x = (Console.BufferWidth / 2) - (String.length texto / 2)
    mostrarMensaje x y color texto

let dibujarHUD state =
    mostrarMensaje 2 0 ConsoleColor.White $"Puntos: {state.Puntos} | Vidas: {state.Vidas}"


let dibujarAlien state =
    let sprite = if state.AlienState = Alive then "👽" else "💥"
    mostrarMensaje state.AlienX state.AlienY ConsoleColor.Red sprite

let dibujarEnemigo state =
    let sprite = if state.EnemigoEstado = Alive then "👾" else "💥"
    mostrarMensaje state.EnemigoX state.EnemigoY ConsoleColor.Magenta sprite

let dibujarMenuInicio state =
    Console.Clear()
    dibujarTextoCentrado 5 ConsoleColor.Green "=== ALIEN ATTACK ==="
    let opciones = [ "Jugar"; "Cargar"; "Salir" ]
    opciones |> List.iteri (fun i texto ->
        let color = if i = state.MenuSeleccion then ConsoleColor.Green else ConsoleColor.Gray
        dibujarTextoCentrado (10 + i) color (if i = state.MenuSeleccion then "> " + texto else "  " + texto)
    )

let dibujarPausa state =
    Console.Clear()
    dibujarTextoCentrado 10 ConsoleColor.Yellow "=== JUEGO EN PAUSA ==="
    let opciones = [ "Guardar y Continuar"; "Salir al Menú Principal" ]
    opciones |> List.iteri (fun i texto ->
        let color = if i = state.MenuSeleccion then ConsoleColor.Green else ConsoleColor.Gray
        dibujarTextoCentrado (13 + i) color (if i = state.MenuSeleccion then "> " + texto else "  " + texto)
    )

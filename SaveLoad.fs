module ProyectoJuego.SaveLoad

open System.IO
open ProyectoJuego.Types

let archivoGuardado = "partida.txt"

let guardarPartida state =
    try
        
        let datos = $"{state.Vidas},{state.Puntos}"
        File.WriteAllText(archivoGuardado, datos)
        state 
    with
    | _ -> state // Si hay un error (ej. falta de permisos), evitamos que el juego crashee

let cargarPartida state =
    try
        if File.Exists(archivoGuardado) then
            
            let datos = File.ReadAllText(archivoGuardado).Split(',')
            let vidasGuardadas = int datos.[0]
            let puntosGuardados = int datos.[1]
            
            
            { state with 
                Vidas = vidasGuardadas
                Puntos = puntosGuardados
                Pantalla = Jugando
                RedibujarPantalla = true }
        else
            state // Si el archivo no existe, no hace nada
    with
    | _ -> state

module ProyectoJuego.SaveLoad

open System.IO
open ProyectoJuego.Types

let archivoGuardado = "partida.txt"

let guardarPartida state =
    try
        // Guardamos las vidas y los puntos separados por una coma
        let datos = $"{state.Vidas},{state.Puntos}"
        File.WriteAllText(archivoGuardado, datos)
        state // Devolvemos el mismo estado para que el juego siga corriendo
    with
    | _ -> state // Si hay un error (ej. falta de permisos), evitamos que el juego crashee

let cargarPartida state =
    try
        if File.Exists(archivoGuardado) then
            // Leemos el texto, lo separamos por la coma y lo convertimos a enteros
            let datos = File.ReadAllText(archivoGuardado).Split(',')
            let vidasGuardadas = int datos.[0]
            let puntosGuardados = int datos.[1]
            
            // Reconstruimos el estado con los datos cargados y mandamos a la pantalla de juego
            { state with 
                Vidas = vidasGuardadas
                Puntos = puntosGuardados
                Pantalla = Jugando
                RedibujarPantalla = true }
        else
            state // Si el archivo no existe, no hace nada
    with
    | _ -> state
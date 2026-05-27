module ProyectoJuego.SaveLoad

open System.IO
open ProyectoJuego.Types

let archivoGuardado = "partida.txt"

let guardarPartida state =
  
        
        let datos = $"{state.Vidas},{state.Puntos}"
        File.WriteAllText(archivoGuardado, datos)
        state 
    

let cargarPartida state =
   
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
            state 


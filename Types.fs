module ProyectoJuego.Types 

open System

type ProgramState = Running | Terminated
type SpriteState = Alive | Hit
type Pantalla = MenuInicio | Jugando | Pausa | GameOver
type Misil = { X: int; Y: int }

type State = {
    ProgramState: ProgramState
    Pantalla: Pantalla
    MenuSeleccion: int
    Vidas: int
    Puntos: int
    AlienX: int; AlienY: int
    AlienState: SpriteState
    RedibujarPantalla: bool
    Tick: int
    Misiles: Misil list
    EnemigoX: int; EnemigoY: int; EnemigoDir: int
    EnemigoEstado: SpriteState
    MisilesEnemigos: Misil list
    ColisionAlien: int; ColisionEnemigo: int
}

let estadoInicial = {
    ProgramState = Running; Pantalla = MenuInicio; MenuSeleccion = 0; Vidas = 3; Puntos = 0
    AlienX = 40; AlienY = 12; AlienState = Alive; RedibujarPantalla = true; Tick = -1
    Misiles = []; EnemigoX = Console.BufferWidth - 4; EnemigoY = 5; EnemigoDir = 1; EnemigoEstado = Alive
    MisilesEnemigos = []; ColisionAlien = 0; ColisionEnemigo = 0
}

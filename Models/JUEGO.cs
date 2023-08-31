using System.Collections.Generic;

namespace TP_PREGUNTADORT;
public class JUEGO
{
    private static string _username;
    private static int _puntajeActual;
    private static int _cantidadPreguntasCorrectas;
    private static int _contadorPreguntas;
    private static List<PREGUNTA> _preguntas;
    private static List<RESPUESTA> _respuestas;

    public static void InicializarJuego(){
    _username = null;
    _puntajeActual = 0;
    _cantidadPreguntasCorrectas = 0;
    _contadorPreguntas = 1;
    } 

    public static List<CATEGORIA> ObtenerCategorias(){
    return BD.ObtenerCategorias();
    }
    public static List<DIFICULTAD> ObtenerDificultades(){
    return BD.ObtenerDificultades();
    }
    public static void CargarPartida(string username, int dificultad, int categoria){
        _username = username;
        _preguntas = BD.ObtenerPreguntas(dificultad, categoria);
        _respuestas = BD.ObtenerRespuestas(_preguntas);
    }
    public static PREGUNTA ObtenerProximaPregunta()
    {
        if(_preguntas.Count != 0)
        {
            Random rd = new Random();        
            int indiceAleatorio = rd.Next(0, _preguntas.Count);
            return _preguntas[indiceAleatorio];
        }
        return null;
    }
    public static List<RESPUESTA> ObtenerProximasRespuestas(int idPregunta){
        List<RESPUESTA> proximasRespuestas = new List<RESPUESTA>();
        foreach (RESPUESTA item in _respuestas)
        {
            if(item.IdPregunta == idPregunta){
                proximasRespuestas.Add(item);
            }
        }
        return proximasRespuestas;
    }

    public static int ObtenerRespuestaCorrecta(List<RESPUESTA> proximasRespuestas){
        int respuestaCorrecta = 0;
        foreach (RESPUESTA item in proximasRespuestas)
        {
            if(item.Correcta == true){
                respuestaCorrecta = item.IdRespuesta;
            }
        }
        return respuestaCorrecta;
    }

    public static string VerificarRespuesta(int idPregunta, int idRespuesta){
        string mensajeRespuesta = "";
        _contadorPreguntas++;
        foreach (RESPUESTA item in _respuestas)
        {
            if((item.IdPregunta == idPregunta && item.IdRespuesta == idRespuesta) || idRespuesta == 0) {
                if(idRespuesta == 0){
                    mensajeRespuesta = "¡Te Quedaste sin Tiempo!";
                }
                else if(item.Correcta == true){
                    _puntajeActual++;
                    _cantidadPreguntasCorrectas++;
                    mensajeRespuesta = "¡Acertaste!";
                }
                else{
                    mensajeRespuesta = "¡Te Equivocaste!";
                }
            }
        }
        
        _preguntas.Remove(_preguntas.Find(PREGUNTA => PREGUNTA.IdPregunta == idPregunta));
        
        return mensajeRespuesta;
        
    }

    public static int ObtenerPuntaje(){
        return _puntajeActual;
    }
    public static int ObtenerNumeroPregunta(){
        return _contadorPreguntas;
    }

    
}
       

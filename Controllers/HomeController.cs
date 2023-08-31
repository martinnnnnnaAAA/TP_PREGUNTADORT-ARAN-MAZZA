using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP_PREGUNTADORT.Models;

namespace TP_PREGUNTADORT.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ConfigurarJuego()
    {
        JUEGO.InicializarJuego();
        ViewBag.categorias = BD.ObtenerCategorias();
        ViewBag.dificultades = BD.ObtenerDificultades();
        return View();
    }

    public IActionResult Comenzar(string username, int dificultad, int categoria)
    {
        JUEGO.CargarPartida(username, dificultad, categoria);
        if (BD.ObtenerPreguntas(dificultad, categoria).Count() > 0)
        {

            return RedirectToAction("Jugar", new { Username = username });
        }
        else
        {
            return RedirectToAction("ConfigurarJuego");
        }
    }
    public IActionResult Jugar(string Username)
    {
        //falta numero de pregunta
        ViewBag.username = Username;
        ViewBag.NumeroPregunta = JUEGO.ObtenerNumeroPregunta();
        ViewBag.PreguntaActual = JUEGO.ObtenerProximaPregunta();
        ViewBag.puntajeActual = JUEGO.ObtenerPuntaje();

        if (ViewBag.PreguntaActual != null)
        {
            ViewBag.categoriaelegida = BD.traerCategoria(ViewBag.PreguntaActual.IdCategoria);
            ViewBag.RespuestasActual = JUEGO.ObtenerProximasRespuestas(ViewBag.PreguntaActual.IdPregunta);
            return View("Juego");
        }
        else
        {
            BD.AgregarHighScore(Username, ViewBag.puntajeActual);
            return View("Fin");
        }


    }

    [HttpPost]
    public IActionResult VerificarRespuesta(int idPregunta, int idRespuesta, string Username, PREGUNTA PreguntaActual)
    {
        ViewBag.username = Username;
        ViewBag.NumeroPregunta = JUEGO.ObtenerNumeroPregunta();
        ViewBag.PreguntaActual = PreguntaActual;
        ViewBag.categoriaelegida = BD.traerCategoriadePregunta(idPregunta);
        ViewBag.puntajeActual = JUEGO.ObtenerPuntaje();
        ViewBag.IdRespuestaElegida = idRespuesta;
        ViewBag.RespuestasActual = JUEGO.ObtenerProximasRespuestas(idPregunta);
        ViewBag.mensajeRespuesta = JUEGO.VerificarRespuesta(idPregunta, idRespuesta);
        ViewBag.puntaje = JUEGO.ObtenerPuntaje();
        ViewBag.RespuestaCorrecta = JUEGO.ObtenerRespuestaCorrecta(JUEGO.ObtenerProximasRespuestas(idPregunta));
        return View("Respuesta");
    }
    public IActionResult HighScore()
    {
        ViewBag.Ranking = BD.TraerHighScore();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}

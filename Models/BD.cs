using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;

namespace TP_PREGUNTADORT;

public static class BD
{
    private static string _connectionString = @"Server=localhost;DataBase=DB_PREGUNTADORT;Trusted_Connection=True;";
    public static List<CATEGORIA> ObtenerCategorias()
    {
        using (SqlConnection db = new SqlConnection(_connectionString))
        {
            string SQL = "SELECT * FROM Categorias";
            List<CATEGORIA> ListadoCategorias = db.Query<CATEGORIA>(SQL).ToList();
            return ListadoCategorias;
        }
    }

    public static List<DIFICULTAD> ObtenerDificultades()
    {
        using (SqlConnection db = new SqlConnection(_connectionString))
        {
            string SQL = "SELECT * FROM Dificultades";
            List<DIFICULTAD> ListadoDificultades = db.Query<DIFICULTAD>(SQL).ToList();
            return ListadoDificultades;
        }
    }

    public static List<PREGUNTA> ObtenerPreguntas(int dificultad, int categoria)
    {
        List<PREGUNTA> ListadoPreguntas = new List<PREGUNTA>();
        if (dificultad == -1 && categoria == -1)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string SQL = "SELECT * FROM Preguntas";
                ListadoPreguntas = db.Query<PREGUNTA>(SQL).ToList();
            }
            return ListadoPreguntas;
        }
        else if (dificultad == -1 && categoria != -1)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string SQL = "SELECT * FROM Preguntas WHERE IdCategoria = @pcategoria";
                ListadoPreguntas = db.Query<PREGUNTA>(SQL, new { pcategoria = categoria }).ToList();
            }
            return ListadoPreguntas;
        }
        else if (dificultad != -1 && categoria == -1)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string SQL = "SELECT * FROM Preguntas WHERE IdDificultad = @pdificultad";
                ListadoPreguntas = db.Query<PREGUNTA>(SQL, new { pdificultad = dificultad }).ToList();
            }
            return ListadoPreguntas;
        }
        else
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string SQL = "SELECT * FROM Preguntas WHERE IdDificultad = @pdificultad AND IdCategoria = @pcategoria";
                ListadoPreguntas = db.Query<PREGUNTA>(SQL, new { pcategoria = categoria, pdificultad = dificultad }).ToList();
            }
            return ListadoPreguntas;
        }
    }
    public static List<RESPUESTA> ObtenerRespuestas(List<PREGUNTA> preguntas)
    {
        List<RESPUESTA> ListaTodasLasRespuestas = new List<RESPUESTA>();
        using (SqlConnection db = new SqlConnection(_connectionString))
        {
            foreach (PREGUNTA item in preguntas)
            {
                string SQL = "SELECT * FROM Respuestas WHERE IdPregunta = @pidPregunta";

                ListaTodasLasRespuestas.AddRange(db.Query<RESPUESTA>(SQL, new { pIdPregunta = item.IdPregunta }).ToList());
            }
        }
        return ListaTodasLasRespuestas;
    }

    public static CATEGORIA traerCategoria(int idCategoria)
    {
        CATEGORIA categoriaelegida = null;
        using (SqlConnection db = new SqlConnection(_connectionString))
        {
            string SQL = "SELECT * FROM Categorias WHERE IdCategoria = @pidCategoria";
            categoriaelegida = db.QueryFirstOrDefault<CATEGORIA>(SQL, new { pidCategoria = idCategoria });
            return categoriaelegida;
        }
    }
    public static CATEGORIA traerCategoriadePregunta(int idPregunta)
    {
        CATEGORIA categoriaelegida = null;
        using (SqlConnection db = new SqlConnection(_connectionString))
        {
            string SQL = "SELECT Categorias.* FROM Categorias INNER JOIN Preguntas ON Categorias.IdCategoria = Preguntas.IdCategoria WHERE IdPregunta = @pidpregunta";
            categoriaelegida = db.QueryFirstOrDefault<CATEGORIA>(SQL, new { pidpregunta = idPregunta });
            return categoriaelegida;
        }
    }
    public static void AgregarHighScore(string Username, int Puntaje)
    {
        string SQL = "INSERT INTO HighScore(Username,Puntaje,FechaHora) VALUES(@pUsername,@pPuntaje,@pFechaHora)";
        using (SqlConnection db = new SqlConnection(_connectionString))
        {
            db.Execute(SQL, new { pUsername = Username, pPuntaje = Puntaje, pFechaHora = DateTime.Now });
        }
    }
    public static List<HighScore> TraerHighScore() {
    using(SqlConnection db = new SqlConnection(_connectionString)){
        string SQL = "SELECT * FROM HighScore Order By Puntaje desc";
        List<HighScore> tabla = db.Query<HighScore>(SQL).ToList();
        return tabla;
    }
  }
}






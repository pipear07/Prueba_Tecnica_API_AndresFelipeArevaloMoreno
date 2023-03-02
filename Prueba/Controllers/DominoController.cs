using Microsoft.AspNetCore.Mvc;
using Prueba.Models;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
/*
* * Realizado por: Andres Felipe Arevalo Moreno
*   Fecha: 28 de febrero de 2023
*   Objetivo: Insertar fichas de domino y organizarlas de acuerdo a los requerimientos 
*   planteados, los cuales son que el ultimo y primero coincidan y que los medios de las
*   fichas vecinas coincidan ejemplo [1|2] [2|1]. Es una API basada en .net core 6 y MVC
* */
namespace Prueba.Controllers
{
    [ApiController]
    [Route("domino")]
    public class DominoController : Controller
    {
        [HttpPost] //Respuesta post de insertar o guardar
        [Route("Insertar_Fichas")] //Nombre de la funcion en la API
        public dynamic guardarDomino(Domino domino)
        {          
            try // todo el programa esta protegido por try  y catch para controlar las excepciones de fallo
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                var rToken = Jwt.validarToken(identity); //como la variable es estatica puedo acceder a ella

                if (!rToken.success) return rToken;

                Usuario usuario = rToken.result;

                if (usuario.rol != "administrador")
                {
                    return new
                    { //El return es lo que retornara en el json de la API
                        success = false,
                        message = "No tienes permisos para insertar fichas",
                        result = ""
                    };
                }

                List<Domino> Fichas = new List<Domino>
                {
                    new Domino //instanciamos las propiedades de dominio, aqui como no hay constructor no hay problema
                    {
                    }
                };
                
                if (domino.Ficha.Length >= 2 && domino.Ficha.Length <= 6)
                {
                    //Despedazar la ficha, aqui le quitamos los corchetes y separador [2|2]
                    foreach (string s in domino.Ficha)
                    {
                        domino.data1 = s.Substring(0, s.Length - 1); //Variable para eliminar el ultimo corchete ]
                        domino.data2 = domino.data1.Substring(1); //Variable para eliminar el primer corchete [
                        domino.data3 = domino.data2.Replace("|", string.Empty); //Variable para eliminar el medio |
                        domino.Lista.Add(Convert.ToInt32(domino.data3)); //Agreguelos a la lista y conviertalos en enteros
                    }
                    //Aqui instanciamos unos arreglos dimencionales de 2D 
                    int[] acumulador = new int[domino.Lista.Count()];    //Nos sirve como vector para intercambiar filas o columnas
                    int[,] separador = new int[2, domino.Lista.Count()]; //Vector dimencional o matriz que contendra los valores organizados por par de fichas dimenciones de 2x2, 2x3, 2x4, 2x5, 2x6
                    string[,] posicion = new string[2, domino.Lista.Count()]; //captura la posicion de la matriz                

                   //Aqui colocamos la lista de numeros enteros a un vector dimencional 2d que es lo mismo que una matriz
                    for (int i = 0; domino.m < acumulador.Length; i++)
                    {
                        domino.k = 0;
                        for (int j = 0; j < 2; j++)
                        {
                            if (j < 1)
                            {
                                separador[domino.k, domino.m] = Convert.ToInt32(domino.Lista[domino.m].ToString().Substring(0, 1));
                                posicion[domino.k, domino.m] = (domino.k + "" + domino.m).ToString();
                            }
                            else
                            {
                                separador[domino.k, domino.m] = Convert.ToInt32(domino.Lista[domino.m].ToString().Substring(1, 1));
                                posicion[domino.k, domino.m] = (domino.k + "" + domino.m).ToString();
                            }
                            domino.Lista2.Add(separador[domino.k, domino.m]);
                            i++;
                            domino.k++;
                        }
                        domino.m++;
                    }

                    domino.Mensaje = "Fichas Registradas"; //Mensaje de que se registraron exitosamente

                    //Comparar posiciones de matriz - diagonal
                    for (int i = 1; i < 2; i++) //filas 2
                    {
                        for (int j = 0; j + 1 < domino.Lista.Count(); j++)
                        {                        
                            while (separador[domino.v, j] != separador[domino.b, domino.w]) //El ciclo while me permite desplazarme por las filas y columnas sin afectar el ciclo primario de la matriz, osea son dos ciclos que hace por columnas y filas internamente, el que cambia de pivote y el que cambia por columna
                            {
                                //intercambie filas
                                domino.guardart = separador[domino.b + 1, domino.w];
                                separador[domino.b + 1, domino.w] = separador[domino.b, domino.w];
                                separador[domino.b, domino.w] = domino.guardart;

                                //intercambie columnas
                                if (separador[domino.v, j] != separador[domino.b, domino.w])
                                {   // intercambie la primera fila de la columna
                                    domino.guardart = separador[domino.b, domino.w];
                                    separador[domino.b,domino.w] = separador[domino.b, domino.w + 1];
                                    separador[domino.b, domino.w + 1] = domino.guardart;
                                    // intercambie la segunda fila de la columna
                                    domino.guardarr = separador[domino.b + 1, domino.w];
                                    separador[domino.b + 1, domino.w] = separador[domino.b + 1, domino.w + 1];
                                    separador[domino.b + 1, domino.w + 1] = domino.guardarr;
                                }
                                //Verificador de que si encuentra el pivote (osea la diagonal invertida igual) se saldra del ciclo while y pasara de pivote                            
                                if (separador[domino.v, j] == separador[domino.b, j+1])
                                {
                                    domino.w = domino.h;
                                    break;
                                }

                                if (domino.w >= Convert.ToInt32(domino.Lista.Count() - 1))
                                {
                                    j = 0;
                                    domino.w = 1;
                                }

                                if (domino.interruptor == true && domino.w == 2 &&  Convert.ToInt32(domino.Lista.Count()) == 4)
                                { //condicional para que solo suceda una vez cuando es dimencion 2x4 ((arreglo de bug))
                                    domino.w = 1;
                                    domino.interruptor = false;
                                    domino.interruptor2 = true;
                                }

                                if (separador[domino.v, j] == separador[domino.b, domino.w])
                                {
                                    domino.w++; //posible solucion
                                    if (domino.w >= Convert.ToInt32(domino.Lista.Count() - 1))
                                    {
                                        j = 0;
                                        domino.w = 1;
                                        if (separador[domino.v, j] == separador[domino.b, domino.w])
                                        {
                                            break;
                                        }
                                    }                                  
                                }
                                
                                if (separador[domino.v, j] != separador[domino.b, domino.w])
                                {
                                    domino.w++;
                                }

                                if (domino.interruptor2 == true && domino.w == 2 && Convert.ToInt32(domino.Lista.Count()) == 4)
                                { //condicional para que solo suceda una vez cuando es dimencion 2x4 ((arreglo de bug))
                                    domino.w = 1;
                                    domino.interruptor2 = false;
                                }

                                if (domino.w >= Convert.ToInt32(domino.Lista.Count() - 1))
                                {
                                    j = 0;
                                    domino.w = 1;
                                }
                            }
                           

                            if (separador[domino.v, domino.Lista.Count() -2] == separador[domino.b, domino.Lista.Count() - 1] && domino.w >= Convert.ToInt32(domino.Lista.Count() - 1))
                            {
                                j = domino.Lista.Count();
                                domino.w = 1;
                            }

                            domino.w++; //aumente la posicion de la fila
                            domino.h++; //aumente la posicion interna de la fila
                        }
                    }
                    //imprimir fichas de domino organizadas                
                    for (int i = 0; i < 2; i++)
                    {                     
                        for (int j = 0; j < acumulador.Length; j++)
                        {
                            domino.Fichas_organizadas = domino.Fichas_organizadas + " "+"[" +(separador[i, j]+ "|" + separador[i+1, j]+"]").ToString();                       
                        }
                        break; //cuando termine de contar la cantidad de fichas, salirse del bucle for                  
                    }
                }
                else
                { // en caso de ser mayor a 6 fichas o menor a 2 fichas, procedera a ejecutar esto
                    domino.Mensaje = "Debe ser minimo 2 fichas y maximo 6 fichas";
                    domino.aceptado = false;
                    domino.Fichas_Ingresadas = 0;                
                }              
            }
            catch (Exception ex)
            { // en caso de presentarse alguna falla anormal, la API no expondra el error al usuario pero tambien podra ser por la validacion de las cadenas de entrada
                domino.Mensaje = "Ops ocurrio un problema, puede ser un error o que las fichas en cadena no son validas";
                domino.aceptado = false;    //false cuando ocurra algo malo
                domino.Fichas_Ingresadas = 0;                            
            }

            return new
            { //El return es lo que retornara en el json de la API
                success = domino.aceptado, //Conexion verificada
                message = domino.Mensaje, //Mensaje de registrado
                Fichas_Ingresadas = domino.Ficha, //Imprimir las fichas ingresadas en ese orden
                Fichas_organizadas = domino.Fichas_organizadas //Imprimir las fichas organizadas
            };
        }
    }
}

using System;

class Programa
{
    int totalEvaluados = 0;
    int publicados = 0;
    int publicadosConAjustes = 0;
    int rechazados = 0;
    int enRevision = 0;

    int impactoBajo = 0;
    int impactoMedio = 0;
    int impactoAlto = 0;

    static void Main()
    {
        Programa p = new Programa();
        p.IniciarSistema();
    }

    void IniciarSistema()
    {
        int opcion = 0;

        do
        {
            Console.WriteLine("SIMULADOR DE DECISIONES PARA PLATAFORMA DE STREAMING");
            Console.WriteLine("1. Evaluar nuevo contenido");
            Console.WriteLine("2. Mostrar reglas del sistema");
            Console.WriteLine("3. Mostrar estadisticas de la sesion");
            Console.WriteLine("4. Reiniciar estadisticas");
            Console.WriteLine("5. Salir");

            opcion = LeerEntero("Seleccione una opcion (1-5): ");

            switch (opcion)
            {
                case 1:
                    EvaluarContenido();
                    break;

                case 2:
                    MostrarReglas();
                    break;

                case 3:
                    MostrarEstadisticas();
                    break;

                case 4:
                    ReiniciarEstadisticas();
                    break;

                case 5:
                    Console.WriteLine("Resumen final de la sesion:");
                    MostrarEstadisticas();
                    Console.WriteLine("Gracias por usar el sistema.");
                    break;

                default:
                    Console.WriteLine("Opcion invalida.");
                    break;
            }

            if (opcion != 5)
            {
                Console.WriteLine();
                Console.WriteLine("Presione una tecla para volver al menu...");
                Console.ReadKey();
                Console.Clear();
            }

        } while (opcion != 5);
    }

    void EvaluarContenido()
    {
        Console.Clear();
        Console.WriteLine("EVALUACION DE NUEVO CONTENIDO");

        string tipo = LeerTipoContenido();
        int duracion = LeerEntero("Ingrese la duracion en minutos: ");
        string clasificacion = LeerClasificacion();
        int hora = LeerHora();
        string produccion = LeerProduccion();

        string razonTecnica = ValidarTecnica(tipo, duracion, clasificacion, hora, produccion);

        totalEvaluados = totalEvaluados + 1;

        if (razonTecnica != "OK")
        {
            rechazados = rechazados + 1;

            Console.WriteLine();
            Console.WriteLine("DECISION FINAL: RECHAZAR");
            Console.WriteLine("Razon: " + razonTecnica);
            return;
        }

        string impacto = CalcularImpacto(duracion, hora, produccion);
        ActualizarContadorImpacto(impacto);

        string decision = ObtenerDecisionFinal(tipo, duracion, clasificacion, hora, produccion, impacto);
        string razonDecision = ObtenerRazonDecision(tipo, duracion, clasificacion, hora, produccion, impacto, decision);

        if (decision == "Publicar")
        {
            publicados = publicados + 1;
        }
        else if (decision == "Publicar con ajustes")
        {
            publicadosConAjustes = publicadosConAjustes + 1;
        }
        else if (decision == "Enviar a revision")
        {
            enRevision = enRevision + 1;
        }

        Console.WriteLine();
        Console.WriteLine("Impacto detectado: " + impacto);
        Console.WriteLine("DECISION FINAL: " + decision);
        Console.WriteLine("Razon: " + razonDecision);
    }

    string ValidarTecnica(string tipo, int duracion, string clasificacion, int hora, string produccion)
    {
        if (clasificacion == "Todo publico")
        {
        }
        else if (clasificacion == "+13")
        {
            if (hora < 6 || hora > 22)
            {
                return "La clasificacion +13 solo puede programarse entre las 6 y las 22 horas.";
            }
        }
        else if (clasificacion == "+18")
        {
            if (!(hora >= 22 || hora <= 5))
            {
                return "La clasificacion +18 solo puede programarse entre las 22 y las 5 horas.";
            }
        }

        if (tipo == "Pelicula")
        {
            if (duracion < 60 || duracion > 180)
            {
                return "La pelicula debe durar entre 60 y 180 minutos.";
            }
        }
        else if (tipo == "Serie")
        {
            if (duracion < 20 || duracion > 90)
            {
                return "La serie debe durar entre 20 y 90 minutos.";
            }
        }
        else if (tipo == "Documental")
        {
            if (duracion < 30 || duracion > 120)
            {
                return "El documental debe durar entre 30 y 120 minutos.";
            }
        }
        else if (tipo == "Evento en vivo")
        {
            if (duracion < 30 || duracion > 240)
            {
                return "El evento en vivo debe durar entre 30 y 240 minutos.";
            }
        }

        if (produccion == "Baja")
        {
            if (clasificacion == "+18")
            {
                return "La produccion baja no es valida para contenidos +18.";
            }
        }

        return "OK";
    }

    string CalcularImpacto(int duracion, int hora, string produccion)
    {
        if (produccion == "Alta" || duracion > 120 || (hora >= 20 && hora <= 23))
        {
            return "Alto";
        }
        else if (produccion == "Media" || (duracion >= 60 && duracion <= 120))
        {
            return "Medio";
        }
        else
        {
            return "Bajo";
        }
    }

    string ObtenerDecisionFinal(string tipo, int duracion, string clasificacion, int hora, string produccion, string impacto)
    {
        if (impacto == "Alto")
        {
            return "Enviar a revision";
        }
        else if (NecesitaAjustes(tipo, duracion, clasificacion, hora))
        {
            return "Publicar con ajustes";
        }
        else
        {
            return "Publicar";
        }
    }

    string ObtenerRazonDecision(string tipo, int duracion, string clasificacion, int hora, string produccion, string impacto, string decision)
    {
        if (decision == "Enviar a revision")
        {
            return "Cumple la validacion tecnica, pero el impacto es alto.";
        }
        else if (decision == "Publicar con ajustes")
        {
            if (tipo == "Pelicula" && (duracion == 60 || duracion == 180))
            {
                return "Cumple las reglas, pero la duracion esta en un limite del rango permitido.";
            }
            else if (tipo == "Serie" && (duracion == 20 || duracion == 90))
            {
                return "Cumple las reglas, pero la duracion esta en un limite del rango permitido.";
            }
            else if (tipo == "Documental" && (duracion == 30 || duracion == 120))
            {
                return "Cumple las reglas, pero la duracion esta en un limite del rango permitido.";
            }
            else if (tipo == "Evento en vivo" && (duracion == 30 || duracion == 240))
            {
                return "Cumple las reglas, pero la duracion esta en un limite del rango permitido.";
            }
            else if (clasificacion == "+13" && (hora == 6 || hora == 22))
            {
                return "Cumple las reglas, pero el horario esta en el borde permitido para +13.";
            }
            else
            {
                return "Cumple las reglas, pero requiere un ajuste menor antes de publicarse.";
            }
        }
        else
        {
            return "Cumple la validacion tecnica y su impacto es " + impacto.ToLower() + ".";
        }
    }

    bool NecesitaAjustes(string tipo, int duracion, string clasificacion, int hora)
    {
        if (tipo == "Pelicula")
        {
            if (duracion == 60 || duracion == 180)
            {
                return true;
            }
        }
        else if (tipo == "Serie")
        {
            if (duracion == 20 || duracion == 90)
            {
                return true;
            }
        }
        else if (tipo == "Documental")
        {
            if (duracion == 30 || duracion == 120)
            {
                return true;
            }
        }
        else if (tipo == "Evento en vivo")
        {
            if (duracion == 30 || duracion == 240)
            {
                return true;
            }
        }

        if (clasificacion == "+13")
        {
            if (hora == 6 || hora == 22)
            {
                return true;
            }
        }

        return false;
    }

    void ActualizarContadorImpacto(string impacto)
    {
        if (impacto == "Bajo")
        {
            impactoBajo = impactoBajo + 1;
        }
        else if (impacto == "Medio")
        {
            impactoMedio = impactoMedio + 1;
        }
        else
        {
            impactoAlto = impactoAlto + 1;
        }
    }

    void MostrarReglas()
    {
        Console.Clear();
        Console.WriteLine("REGLAS DEL SISTEMA");

        Console.WriteLine("1. Reglas de clasificacion y horario");
        Console.WriteLine("   - Todo publico: cualquier hora");
        Console.WriteLine("   - +13: entre 6 y 22 horas");
        Console.WriteLine("   - +18: entre 22 y 5 horas");
        Console.WriteLine();

        Console.WriteLine("2. Reglas de duracion por tipo");
        Console.WriteLine("   - Pelicula: 60 a 180 minutos");
        Console.WriteLine("   - Serie: 20 a 90 minutos");
        Console.WriteLine("   - Documental: 30 a 120 minutos");
        Console.WriteLine("   - Evento en vivo: 30 a 240 minutos");
        Console.WriteLine();

        Console.WriteLine("3. Reglas de produccion");
        Console.WriteLine("   - Produccion baja solo valida para Todo publico o +13");
        Console.WriteLine("   - Produccion media o alta valida para cualquier clasificacion");
        Console.WriteLine();

        Console.WriteLine("4. Clasificacion de impacto");
        Console.WriteLine("   - Alto: produccion alta, duracion mayor a 120 o horario entre 20 y 23");
        Console.WriteLine("   - Medio: produccion media o duracion entre 60 y 120");
        Console.WriteLine("   - Bajo: produccion baja y duracion menor a 60");
        Console.WriteLine();

        Console.WriteLine("5. Decisiones");
        Console.WriteLine("   - Publicar");
        Console.WriteLine("   - Publicar con ajustes");
        Console.WriteLine("   - Enviar a revision");
        Console.WriteLine("   - Rechazar");
    }

    void MostrarEstadisticas()
    {
        Console.WriteLine("ESTADISTICAS DE LA SESION");

        Console.WriteLine("Total evaluados: " + totalEvaluados);
        Console.WriteLine("Publicados: " + publicados);
        Console.WriteLine("Publicados con ajustes: " + publicadosConAjustes);
        Console.WriteLine("Rechazados: " + rechazados);
        Console.WriteLine("En revision: " + enRevision);
        Console.WriteLine("Impacto predominante: " + ObtenerImpactoPredominante());
        Console.WriteLine("Porcentaje de aprobacion: " + CalcularPorcentajeAprobacion() + "%");
    }

    void ReiniciarEstadisticas()
    {
        string respuesta = "";

        while (respuesta != "S" && respuesta != "N")
        {
            Console.Write("Desea reiniciar las estadisticas? (S/N): ");
            respuesta = Console.ReadLine();

            if (respuesta != null)
            {
                respuesta = respuesta.Trim().ToUpper();
            }
            else
            {
                respuesta = "";
            }
        }

        if (respuesta == "S")
        {
            totalEvaluados = 0;
            publicados = 0;
            publicadosConAjustes = 0;
            rechazados = 0;
            enRevision = 0;
            impactoBajo = 0;
            impactoMedio = 0;
            impactoAlto = 0;

            Console.WriteLine("Las estadisticas fueron reiniciadas.");
        }
        else
        {
            Console.WriteLine("Las estadisticas no se modificaron.");
        }
    }

    string ObtenerImpactoPredominante()
    {
        if (impactoBajo == 0 && impactoMedio == 0 && impactoAlto == 0)
        {
            return "Sin datos";
        }

        if (impactoBajo >= impactoMedio && impactoBajo >= impactoAlto)
        {
            return "Bajo";
        }
        else if (impactoMedio >= impactoBajo && impactoMedio >= impactoAlto)
        {
            return "Medio";
        }
        else
        {
            return "Alto";
        }
    }

    double CalcularPorcentajeAprobacion()
    {
        if (totalEvaluados == 0)
        {
            return 0;
        }

        double aprobados = publicados + publicadosConAjustes;
        return (aprobados * 100) / totalEvaluados;
    }

    string LeerTipoContenido()
    {
        int opcion = 0;

        while (opcion < 1 || opcion > 4)
        {
            Console.WriteLine("Tipo de contenido:");
            Console.WriteLine("1. Pelicula");
            Console.WriteLine("2. Serie");
            Console.WriteLine("3. Documental");
            Console.WriteLine("4. Evento en vivo");
            opcion = LeerEntero("Seleccione una opcion (1-4): ");
        }

        if (opcion == 1)
        {
            return "Pelicula";
        }
        else if (opcion == 2)
        {
            return "Serie";
        }
        else if (opcion == 3)
        {
            return "Documental";
        }
        else
        {
            return "Evento en vivo";
        }
    }

    string LeerClasificacion()
    {
        int opcion = 0;

        while (opcion < 1 || opcion > 3)
        {
            Console.WriteLine("Clasificacion:");
            Console.WriteLine("1. Todo publico");
            Console.WriteLine("2. +13");
            Console.WriteLine("3. +18");
            opcion = LeerEntero("Seleccione una opcion (1-3): ");
        }

        if (opcion == 1)
        {
            return "Todo publico";
        }
        else if (opcion == 2)
        {
            return "+13";
        }
        else
        {
            return "+18";
        }
    }

    string LeerProduccion()
    {
        int opcion = 0;

        while (opcion < 1 || opcion > 3)
        {
            Console.WriteLine("Nivel de produccion:");
            Console.WriteLine("1. Baja");
            Console.WriteLine("2. Media");
            Console.WriteLine("3. Alta");
            opcion = LeerEntero("Seleccione una opcion (1-3): ");
        }

        if (opcion == 1)
        {
            return "Baja";
        }
        else if (opcion == 2)
        {
            return "Media";
        }
        else
        {
            return "Alta";
        }
    }

    int LeerHora()
    {
        int hora = -1;

        while (hora < 0 || hora > 23)
        {
            hora = LeerEntero("Ingrese la hora programada (0-23): ");
        }

        return hora;
    }

    int LeerEntero(string mensaje)
    {
        int numero = 0;
        string texto = "";
        bool valido = false;

        while (!valido)
        {
            Console.Write(mensaje);
            texto = Console.ReadLine();

            valido = int.TryParse(texto, out numero);

            if (!valido)
            {
                Console.WriteLine("Entrada invalida. Debe ingresar un numero entero.");
            }
        }

        return numero;
    }
}
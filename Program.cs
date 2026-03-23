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

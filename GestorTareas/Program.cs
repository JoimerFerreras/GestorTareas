using GestorTareas.Clases;
using System.IO;
using System.Linq;
using System.Text.Json;

string rutaArchivo = "tareas.json";

// Cargar tareas existentes al iniciar
List<Tarea> tareas = CargarTareasDesdeJson();

bool salir = false;

while (!salir)
{
    Console.Clear();
    Console.WriteLine("===== GESTOR DE TAREAS =====");
    Console.WriteLine("1. Crear nueva tarea");
    Console.WriteLine("2. Listar tareas");
    Console.WriteLine("3. Eliminar tarea");
    Console.WriteLine("4. Salir");
    Console.Write("Elige una opción: ");

    string opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            // CREAR
            Tarea nueva = CrearTareaDesdeConsola();
            tareas.Add(nueva);
            GuardarTareasEnJson(tareas);
            Console.WriteLine("Tarea creada y guardada correctamente.");
            Pausar();
            break;

        case "2":
            // LISTAR
            ListarTareas(tareas);
            Pausar();
            break;

        case "3":
            // ELIMINAR
            EliminarTarea(tareas);
            GuardarTareasEnJson(tareas);
            Pausar();
            break;

        case "4":
            salir = true;
            break;

        default:
            Console.WriteLine("Opción no válida.");
            Pausar();
            break;
    }
}

Console.WriteLine("Saliendo del programa...");


// FUNCIONES 

Tarea CrearTareaDesdeConsola()
{
    Console.Clear();
    Console.WriteLine("=== Crear nueva tarea ===");

    Console.Write("Id: ");
    int id = int.Parse(Console.ReadLine() ?? "0");

    Console.Write("Título: ");
    string titulo = Console.ReadLine();

    Console.Write("Descripción: ");
    string descripcion = Console.ReadLine();

    Console.Write("Prioridad (Alta/Media/Baja): ");
    string prioridad = Console.ReadLine();

    Console.Write("Fecha de vencimiento (yyyy-MM-dd) o vacío si no tiene: ");
    string fechaTexto = Console.ReadLine();
    DateTime? fechaVencimiento = null;

    if (!string.IsNullOrWhiteSpace(fechaTexto) &&
        DateTime.TryParse(fechaTexto, out DateTime fecha))
    {
        fechaVencimiento = fecha;
    }

    return new Tarea
    {
        Id = id,
        Titulo = titulo,
        Descripcion = descripcion,
        Prioridad = prioridad,
        FechaVencimiento = fechaVencimiento,
        Completada = false
    };
}

void ListarTareas(List<Tarea> lista)
{
    Console.Clear();
    Console.WriteLine("=== Listado de tareas ===");

    if (lista.Count == 0)
    {
        Console.WriteLine("No hay tareas registradas.");
        return;
    }

    foreach (var t in lista)
    {
        Console.WriteLine($"Id: {t.Id}");
        Console.WriteLine($"Título: {t.Titulo}");
        Console.WriteLine($"Descripción: {t.Descripcion}");
        Console.WriteLine($"Prioridad: {t.Prioridad}");
        Console.WriteLine($"Vence: {(t.FechaVencimiento.HasValue ? t.FechaVencimiento.Value.ToString("yyyy-MM-dd") : "Sin fecha")}");
        Console.WriteLine($"Completada: {(t.Completada ? "Sí" : "No")}");
        Console.WriteLine(new string('-', 30));
    }
}

void GuardarTareasEnJson(List<Tarea> lista)
{
    var opciones = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    string json = JsonSerializer.Serialize(lista, opciones);
    File.WriteAllText(rutaArchivo, json);
}

List<Tarea> CargarTareasDesdeJson()
{
    if (!File.Exists(rutaArchivo))
        return new List<Tarea>();

    string json = File.ReadAllText(rutaArchivo);
    var lista = JsonSerializer.Deserialize<List<Tarea>>(json);

    return lista ?? new List<Tarea>();
}

void EliminarTarea(List<Tarea> lista)
{
    Console.Clear();
    Console.WriteLine("=== Eliminar tarea ===");
    
    if (lista.Count == 0)
    {
        Console.WriteLine("No hay tareas para eliminar.");
        return;
    }

    Console.Write("Ingresa el ID de la tarea a eliminar: ");
    if (int.TryParse(Console.ReadLine(), out int id))
    {
        var tarea = lista.FirstOrDefault(t => t.Id == id);
        if (tarea != null)
        {
            lista.Remove(tarea);
            Console.WriteLine($"Tarea '{tarea.Titulo}' eliminada correctamente.");
        }
        else
        {
            Console.WriteLine($"No se encontró una tarea con ID {id}.");
        }
    }
    else
    {
        Console.WriteLine("ID inválido.");
    }
}

void Pausar()
{
    Console.WriteLine();
    Console.WriteLine("Presiona cualquier tecla para continuar...");
    Console.ReadKey();
}

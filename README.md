# RecorredorNonograma

Solucionador de Nonogramas desarrollado en **C# / .NET 10** para el Proyecto N.º 1 de Inteligencia Artificial de la Universidad del Bío-Bío.

El programa utiliza búsqueda en profundidad (**DFS**) con retroceso (*backtracking*) y poda por restricciones para resolver Nonogramas a partir de pistas cargadas desde archivos de texto.

## Requisitos

- .NET SDK 10.0
- Terminal compatible con UTF-8

## Ejecución

Desde la raíz del proyecto, ejecutar:

```powershell
dotnet run -c Release -- Puzzles/cross5.txt
```

También se incluyen los siguientes tableros de prueba:

```powershell
dotnet run -c Release -- Puzzles/test10.txt
dotnet run -c Release -- Puzzles/test15.txt
```

Para ejecutar los experimentos automatizados:

```powershell
dotnet run -c Release -- --benchmark
```

## Formato de entrada

Los Nonogramas se cargan mediante archivos de texto con la siguiente estructura:

```text
5 5
ROWS
1
3
5
3
1
COLUMNS
1
3
5
3
1
```

- La primera línea indica ancho y alto.
- `ROWS` contiene las pistas de cada fila.
- `COLUMNS` contiene las pistas de cada columna.
- Los bloques múltiples se separan mediante espacios, por ejemplo `2 1 3`.
- El símbolo `-` representa una fila o columna sin bloques.

El programa admite tableros rectangulares, siempre que las pistas sean válidas.

## Algoritmo

El solucionador utiliza DFS con backtracking:

1. Genera todas las configuraciones válidas para cada fila y columna.
2. Comienza con un tablero sin filas asignadas.
3. Agrega una configuración válida a la siguiente fila.
4. Comprueba que todas las columnas parciales puedan completarse.
5. Descarta los estados incompatibles.
6. Continúa recursivamente hasta encontrar una solución o agotar las alternativas.

Las configuraciones se precalculan para evitar generarlas repetidamente durante la búsqueda.

## Métricas

El programa registra:

- Tiempo de ejecución de la búsqueda, en milisegundos.
- Bytes asignados en memoria administrada durante la búsqueda.
- Cantidad de nodos expandidos.

El modo benchmark realiza cinco ejecuciones de calentamiento y treinta repeticiones por tablero, calculando los promedios de tiempo y memoria.

Estas mediciones excluyen la lectura de archivos, el preprocesamiento de patrones y la validación final.

## Pruebas automatizadas

Las pruebas utilizan xUnit v3 y pueden ejecutarse con:

```powershell
dotnet run --project Tests/Tests.csproj -c Release
```

La batería incluye diez pruebas para verificar la resolución, generación de patrones, validación de entradas y lectura de archivos.

**Resultado obtenido:** 10 pruebas correctas, 0 fallidas.

## Representación de resultados

Las soluciones se muestran en consola utilizando caracteres Unicode:

- ⬛ Celda pintada.
- ⬜ Celda vacía.

Si el tablero no tiene solución, el programa informa que no se encontró ninguna configuración válida.
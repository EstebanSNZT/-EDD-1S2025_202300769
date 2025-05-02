# Manual de Usuario

## Objetivos del sistema

### General
Proporcionar una guía a los usuarios interesado en utilizar el sistema integral de gestión AutoGest Pro describiendo sus características y principales funcionalidades.

### Específicos
- Describir de manera detallada las principales funcionalidades del programa para facilitar su manejo para los usuarios.

- Ofrecer un resumen de las principales características del programa, con el propósito de brindar una compresión precisa de sus capacidades.2

## Introducción
El propósito de este manual es de ofrecer una guía a los usuarios para el uso adecuado del sistema integral de gestión AutoGest Pro. De esta manera facilitar la manipulación de las funcionalidades las principales funcionalidades del programa y mejorar su eficiencia de uso.

El software fue diseñado para simplificar y optimizar las tareas de gestión para un taller de vehiculos. Siendo administrador se permite realizar cargas masivas, de usuarios, vehiculos, repuestos y para esta tercera fase se permitio la carga masiva de servicios. Puede actualizar los repuestos, ver los repuesto en distinto recorridos como tambien puede generar servicios, generar reporte del login y reportes graficos de las estructuras. Como usuario de la aplicación puedes insertar vehiculos, ver tus servicios en diferentes recorrido y ver tus facturas por cancelar. En esta fase se volvió a agregar la opción de inserción y visualización de usuarios.

## Información del sistema

AutoGest pro es un programa de con interfaz grafica diseñado con el lenguaje de programación C#.

Para la interfaz gráfica se utilizo la libreria para interfaces en linux GTK.

Para almacenar los datos en memoria se utilizaron estructuras de datos abstractas en esta ocasión algunas con mayor seguridad y respaldo. Se utilizo el blockchain para almacenar los usuarios de forma segura, la lista doblemente enlazada para los vehículos, el árbol AVL para los repuestos, el árbol binario de búsqueda para los servicios y el arból de merkle para las facturas. Tambien se incluyó un grafo no dirigido para mostrar las relaciones entre cada vehículo y repuesto a la hora de crear servicios. Tambien para el administrador se le permitio la creación de backups de las entidades de usuarios, vehículos y repuestos para posteriormente cuando termine la ejecución del programa pueda cargar el backup.

## Especificación técnica

### Requisitos de hardware

- Procesador de al menos 2 GHz de velocidad.

- Memoria RAM de al menos 2 GB.

- Espacio de almacenamiento disponible de al menos 500 MB.

### Requisitos de software

- Sistema operativo compatible: Una distribución de Linux compatible.

- .NET SDK 6.0 o superior.

- GTK 4 y sus dependencias instaladas.

- Compilador de C# como dotnet o mono.

- Editor de código como Visual Studio Code o JetBrains Rider.

## Flujo de las funcionalidades del sistema

1. Debe iniciar sesión con el usuario Admin con el correo admin@usac.com y la contraseña admin123.

    ![imagen1](Imagenes/imagen1.png)
    ![imagen2](Imagenes/imagen2.png)

2. Al iniciar sesión con el admin nos redirigira al menú de Admin.

    ![imagen3](Imagenes/imagen3.png)

3. Podemos ahora realizar la carga masiva de los usuarios, vehículos, repuestos y en esta fase tambien de servicios.

    ![imagen4](Imagenes/imagen4.png)
    ![imagen5](Imagenes/imagen5.png)
    ![imagenN1](Imagenes/imagenN1.png)
    ![imagenN2](Imagenes/imagenN2.png)

4. Otra manera de insertar usuarios es en la opción de inserción de usuario.

    ![imagenN3](Imagenes/imagenN3.png)
    ![imagenN4](Imagenes/imagenN4.png)

5. Podemos visualizar los datos de los usuarios en la parte de visualización de usuarios.

    ![imagenN5](Imagenes/imagenN5.png)
    ![imagenN6](Imagenes/imagenN6.png)

6. Podemos actualizar los datos de un repuesto buscandolo previamente y luego actualizando los campos que queramos actualizar

    ![imagen10](Imagenes/imagen10.png)
    ![imagen11](Imagenes/imagen11.png)
    ![imagen12](Imagenes/imagen12.png)
    ![imagen13](Imagenes/imagen13.png)

7. Podemos generar servicios en la parte de generar servicio

    ![imagenN7](Imagenes/imagenN7.png)
    ![imagenN8](Imagenes/imagenN8.png)

8. Podemos visualizar los repuestos en visualización de repuestos con distintos tipos de recorrido

    ![imagen17](Imagenes/imagen17.png)

9. Ahora ya podemos ingresar a la parte del usuario ya que cargamos los usuarios en la carga masiva o en inserción de usuarios

    ![imagen18](Imagenes/imagen18.png)
    ![imagen19](Imagenes/imagen19.png)

10.  Se redireccionara a la ventana de usuario.

    ![imagenN9](Imagenes/imagenN9.png)

11. Podemos registrar vehiculos del usuario con el que iniciamos sesión.
    
    ![imagen22](Imagenes/imagen22.png)
    ![imagen23](Imagenes/imagen23.png)

12. Podemos visualizar los vehículos con los que cuenta el usuario.

    ![imagenN10](Imagenes/imagenN10.png)

13. Podemos visualizar los servicios con los que cuentan los vehiculos del usuario en distintos tipos de recorridos.

    ![imagenN11](Imagenes/imagenN11.png)

14. Podemos visualizar las facturas del usuario pendientes de pagar.

    ![imagenN12](Imagenes/imagenN12.png)

15. Devuelta en el admin podemos generar un json que contenga el control de los inicios de sesión.

    ![imagenN13](Imagenes/imagenN13.png)
    ![imagenN14](Imagenes/imagenN14.png)

16. Podemos generar un reporte gráfico de todas las estructuras del programa si contienen datos.

    ![imagenN15](Imagenes/imagenN15.png)
    ![imagenN16](Imagenes/imagenN16.png)
    ![imagenN17](Imagenes/imagenN17.png)

17. El administrador puede generar el backup de los datos de las entidades usuario, vehículo y repuesto. El json de los bloques y los archivos comprimidos .edd junto con los json de los arboles para descompresión se guardan en la carpeta backup.

    ![imagenN18](Imagenes/imagenN18.png)
    ![imagenN19](Imagenes/imagenN19.png)

18. Una vez cerrado el programa y vuelto a abrir en el menú de admin se puede cargar el backup de los datos que se tenian con anterioridad y deberian de estar presentes de nuevo.

    ![imagenN20](Imagenes/imagenN20.png)
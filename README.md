# Proyecto de Gestión de Cine - ASP.NET Core MVC

Este repositorio contiene un proyecto academico evolutivo de gestión de cine desarrollado utilizando ASP.NET Core MVC y Entity Framework Core como ORM para la persistencia de datos.
Version anterior: `https://github.com/LucasRodriguezOtero/CineManagerEF`

## Descripción del Proyecto

Este proyecto es una aplicación web que permite gestionar un sistema de cine, donde los usuarios pueden realizar diversas acciones como comprar entradas, ver información sobre películas y funciones, y gestionar sus cuentas. El objetivo principal es aplicar los conocimientos adquiridos en la materia, utilizando tecnologías como ASP.NET Core MVC y Entity Framework Core.

## Características Principales

- **Autenticación y Autorización:** Los usuarios pueden iniciar sesión en la aplicación y se implementa un sistema de bloqueo de cuentas después de 3 intentos fallidos de inicio de sesión. Los administradores tienen acceso a funcionalidades adicionales de administración.

- **Gestión de Usuarios:** Los administradores pueden realizar operaciones de alta, baja y modificación (ABM) de usuarios, así como el desbloqueo de cuentas bloqueadas.

- **Validación de Inputs:** Se implementa validación de los datos ingresados por los usuarios en formularios, incluyendo la validación de campos obligatorios y formatos de datos específicos.

## Tecnologías Utilizadas

- **ASP.NET Core MVC:** Framework utilizado para el desarrollo de la aplicación web, siguiendo el patrón Modelo-Vista-Controlador para separar las responsabilidades del proyecto.

- **Entity Framework Core:** ORM utilizado para mapear objetos a la base de datos relacional, permitiendo interactuar con la base de datos utilizando clases de entidad y LINQ.

- **C#:** Lenguaje de programación utilizado para el desarrollo del backend de la aplicación web, incluyendo la lógica de negocio y la interacción con la base de datos.

- **HTML/CSS:** Utilizados para la estructura y el diseño visual de las páginas web de la aplicación.

- **JavaScript:** Utilizado para agregar algunas interactividad a las páginas web.

## Configuración y Uso

1. **Clonar el Repositorio:** 
   Clona este repositorio en tu máquina local utilizando el siguiente comando en tu terminal: `git clone https://github.com/LucasRodriguezOtero/Proyecto-de-Gestion-de-Cine--ASP.NET-Core-MVC.git`

2. **Abrir el Proyecto en Visual Studio:**
Abre Visual Studio y selecciona la opción "Abrir Proyecto/Solución". Navega hasta el directorio donde clonaste el repositorio y selecciona el archivo de solución (.sln).

3. **Configurar la Cadena de Conexión a la Base de Datos:**
Abre el archivo `appsettings.json` en tu proyecto y modifica la cadena de conexión bajo la sección `"ConnectionStrings"` para que coincida con tu instancia de SQL Server.

4. **Ejecutar la Aplicación:**
Ejecuta la aplicación desde Visual Studio haciendo clic en el botón de "Iniciar" o presionando `F5`. Esto compilará el proyecto y ejecutará la aplicación web de gestión de cine en tu navegador predeterminado.

## Gracias!

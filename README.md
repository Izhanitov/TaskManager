# TaskManager

Для разработки использовались:
MS SQL Server 15.0.2000.5,
Microsoft Visual Studio 2019 Version 16.9.4

Для запуска проекта:
1. Сгенерировать базу данных, путем выполнения скрипта DBScripts/TaskManagerScript.sql в SQL Managment Studio
2. В TaskManager/appsettings.json изменить строку подключения "DefaultConnection" на строку подключения к сгенерированной базе данных.

Для сборки js использовался webpack. Исходные файлы JS лежат в TaskManager/wwwroot/js/src

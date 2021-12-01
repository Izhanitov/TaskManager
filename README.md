# TaskManager

Для разработки использовались:
MS SQL Server 15.0.2000.5
Microsoft Visual Studio 2019 Version 16.9.4

Для запуска проекта:
1. Сгенерировать базу данных, путем выполнения скрипта DBScripts/TaskManagerScript.sql в SQL Managment Studio
2. В TaskManager/appsettings.json изменить строку подключения "DefaultConnection" на строку подключения к сгенерированной базе данных.

Для редактирования JS:
1. Перейти в каталог /TaskManager/wwwroot/js/
2. Установить через менеджер пакетов npm в проект webpack:
   npm init -y
   npm i webpack webpack-cli
3. Запусть webpack:
   npx webpack
4. Исходные файлы JS лежат в подкаталоге src. Собираются и минимизируются в dist/js/bundle.js

@echo off
 
@rem Check administrative permissions using net session command.
net session >nul 2>&1
if %errorlevel% == 0 (
    echo Successfully checked administrative permissions.
) else (
    echo Run this script with administrative permissions.
    exit /b 1
)

@rem Check if Docker desktop is running
docker ps 2>&1 | find /I /C "error" > NUL
if %errorlevel% EQU 0 (
    ECHO IMPORTANT: Run DOCKER DESKTOP before you start this script!
    ECHO IMPORTANT: I wait 30 seconds for DOCKER DESKTOP to launch, then continue.
    timeout 30
)

echo Running docker compose files.

docker-compose --env-file .env -f kafka-docker-compose.yaml up

echo Done!

@echo off
 
@rem Check administrative permissions using net session command.
net session >nul 2>&1
if %errorlevel% == 0 (
    echo Successfully checked administrative permissions.
) else (
    echo Run this script with administrative permissions.
    exit /b 1
)

setlocal EnableDelayedExpansion

@rem Create directory where zookeeper data will be stored
@rem If you change this directory remember to set same path at .env file at ZOOKEEPER_DATA_DIR
if not exist "c:\kafka-demo\zookeeper" (
  mkdir "c:\kafka-demo\zookeeper"
  if "!errorlevel!" EQU "0" (
    echo Successfully created folder "c:\kafka-demo\zookeeper".
  ) else (
    echo Error while creating folder "c:\kafka-demo\zookeeper".
    exit /b %errorlevel%
  )
)

@rem Create directory where kafka data will be stored
@rem If you change this directory remember to set same path at .env file at KIBANA_DATA_DIR
if not exist "c:\kafka-demo\kafka" (
  mkdir "c:\kafka-demo\kafka"
  if "!errorlevel!" EQU "0" (
    echo Successfully created folder "c:\kafka-demo\kafka".
  ) else (
    echo Error while creating folder "c:\kafka-demo\kafka".
    exit /b %errorlevel%
  )
)

setlocal DisableDelayedExpansion

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

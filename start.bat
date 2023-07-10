 @rem Check administrative permissions using net session command.
net session >nul 2>&1
if %errorlevel% == 0 (
    echo Successfully checked administrative permissions.
) else (
    echo Run this script with administrative permissions.
    exit /b 1
)

docker-compose -f kafka-docker-compose.yaml up

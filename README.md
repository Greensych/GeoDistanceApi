# GeoDistanceApi

REST API для расчета расстояния между двумя географическими точками на основе формулы Haversine.

## Что делает

API принимает две географические координаты (широта и долгота) и возвращает расстояние между ними в километрах с использованием формулы Haversine, которая учитывает кривизну Земли.

## Требования

- .NET 8.0 или выше
- ASP.NET Core 8.0

## Как запустить

### Разработка

```bash
# Перейти в директорию приложения
cd src/GeoDistanceApi

# Запустить приложение
dotnet run
```

API будет доступен на `https://localhost:5001`
Swagger UI: `https://localhost:5001/swagger`

### Production

```bash
dotnet build --configuration Release
dotnet run --configuration Release
```

## API Endpoint

### POST `/api/distance/calculate`

Рассчитывает расстояние между двумя точками.

**Request:**
```json
{
  "from": {
    "latitude": 55.7558,
    "longitude": 37.6173
  },
  "to": {
    "latitude": 59.9311,
    "longitude": 30.3609
  }
}
```

**Success Response (200 OK):**
```json
{
  "distanceKm": 633.450,
  "from": {
    "latitude": 55.7558,
    "longitude": 37.6173
  },
  "to": {
    "latitude": 59.9311,
    "longitude": 30.3609
  }
}
```

**Error Response (400 Bad Request):**
```json
{
  "message": "Координаты должны находиться в следующих диапазонах: Lat[-90,90], Lon[-180,180]",
  "details": null
}
```

### Constraints

- **Latitude**: -90 до 90 градусов
- **Longitude**: -180 до 180 градусов

## Примеры

### cURL

```bash
curl -X POST https://localhost:5001/api/distance/calculate \
  -H "Content-Type: application/json" \
  -d '{
    "from": {
      "latitude": 55.7558,
      "longitude": 37.6173
    },
    "to": {
      "latitude": 59.9311,
      "longitude": 30.3609
    }
  }'
```

### PowerShell

```powershell
$body = @{
    from = @{
        latitude = 55.7558
        longitude = 37.6173
    }
    to = @{
        latitude = 59.9311
        longitude = 30.3609
    }
} | ConvertTo-Json

Invoke-RestMethod -Uri 'https://localhost:5001/api/distance/calculate' `
  -Method 'POST' `
  -Headers @{'Content-Type'='application/json'} `
  -Body $body
```

## Тесты

### Запуск всех тестов

```bash
dotnet test
```

### Запуск с покрытием

```bash
dotnet test /p:CollectCoverage=true
```

### Тестовые сценарии

- Расстояние между одной и той же точкой = 0 км
- Расстояние Москва -> Санкт-Петербург ≈ 633 км
- Симметрия расстояния (A→B = B→A)
- Валидация отрицательных координат
- Обработка null-аргументов
- Обработка невалидных координат

## Архитектура

```
src/GeoDistanceApi/
├── Controllers/
│   └── DistanceController.cs      # HTTP контроллер
├── Services/
│   ├── IDistanceService.cs        # Интерфейс сервиса
│   └── DistanceService.cs         # Реализация сервиса
├── Models/
│   ├── Coordinate.cs              # Модель географической координаты
│   ├── DistanceRequest.cs         # Request DTO
│   ├── DistanceResponse.cs        # Response DTO
│   └── ErrorResponse.cs           # Error response DTO
├── Program.cs                     # Конфигурация приложения
└── appsettings.json               # Настройки
```

## Логирование

Приложение использует встроенный механизм логирования ASP.NET Core:

- **Debug**: детальная информация о расчётах
- **Warning**: попытки с невалидными данными
- **Error**: неожиданные ошибки

Уровень логирования управляется в `Program.cs`:
- **Development**: Debug и выше
- **Production**: Information и выше

## Обработка ошибок

API возвращает структурированные ошибки:

| Status Code | Причина |
|-------------|----------|
| 200 | Успешный расчёт |
| 400 | Невалидные данные или координаты |
| 500 | Внутренняя ошибка сервера |

## Производительность

- Расчёт выполняется в памяти без IO операций
- Типичное время ответа: < 5 мс
- Результаты округляются до 3 знаков после запятой

## Лицензия

MIT

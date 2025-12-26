# GeoDistanceApi

REST API для расчета расстояния между двумя точками на карте.

## Что делает

Принимает две точки (широта/долгота), возвращает расстояние в км. Используется формула Haversine.

## Как запустить

```bash
cd src/GeoDistanceApi
dotnet run
```

Swagger будет на `https://localhost:5001/swagger`

## Пример

```bash
curl -X POST https://localhost:5001/api/distance/calculate \
  -H "Content-Type: application/json" \
  -d '{
    "from": {"lat": 55.7558, "lon": 37.6173},
    "to": {"lat": 59.9311, "lon": 30.3609}
  }'
```

Результат: `{"distanceKm": 633.45}`

## Тесты

```
dotnet test
```

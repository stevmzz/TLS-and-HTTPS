# Cómo Probar la Aplicación

## Descripción del Sistema

Esta aplicación demuestra la comunicación segura entre diferentes protocolos de red:

**Componentes:**
- **HTTPSMessenger**: Servidor web que recibe peticiones HTTPS y las reenvía vía TCP
- **TLSMessenger**: Servidor TCP que procesa mensajes y simula comunicación TLS
- **Client**: Cliente HTTPS que envía mensajes al sistema

**Flujo de Comunicación:**
```
Client (HTTPS) → HTTPSMessenger (Web API) → TLSMessenger (TCP) → Respuesta
```

El cliente envía un mensaje vía HTTPS al servidor web, quien lo retransmite por TCP al servidor TLS. La respuesta regresa por el mismo camino, demostrando cómo diferentes protocolos pueden trabajar juntos de forma segura.

## Paso 1: Iniciar HTTPSMessenger

**En Visual Studio:**
1. Set `HTTPSMessenger` como startup project (click derecho > Set as Startup Project)
2. Presionar F5
3. Se abrirá Swagger UI en `https://localhost:7154` o pegar en navegador para abrir.

**O por línea de comandos:**
```bash
cd HTTPSMessenger
dotnet run
```

**Salida esperada:**
```
HTTPS Messenger iniciado
Puerto: https://localhost:7154
Enviará mensajes a TLS (localhost:8080)
```

## Paso 2: Iniciar TLSMessenger

**Abrir nueva terminal:**
```bash
cd TLSMessenger
dotnet run
```

**Salida esperada:**
```
TLS MESSENGER
Escuchando en puerto 8080
Esperando mensajes de HTTPS Messenger...
```

## Paso 3: Iniciar Client

**Abrir otra terminal:**
```bash
cd Client
dotnet run
```

**Salida esperada:**
```
CLIENTE HTTPS
COMUNICACIÓN HTTPS → TLS
1. Enviar mensaje (HTTPS → TLS)
2. Estado del servidor
3. Salir

Selecciona una opción:
```

## Paso 4: Probar Comunicación

1. En el Client, seleccionar opción `1`
2. Escribir mensaje: `Hola desde HTTPS`
3. Observar logs en las tres terminales

**Resultado esperado en Client:**
```
Comunicación exitosa:
Mensaje enviado: Hola desde HTTPS
Protocolo usado: HTTPS
Respuesta de TLS: TLS procesó: 'Hola desde HTTPS' | Hora: 14:30:15
```

## Métodos Alternativos de Prueba

### Usando Swagger UI

1. Ir a `https://localhost:7154`
2. Expandir `POST /api/Message/send`
3. Click "Try it out"
4. Usar este JSON:
```json
{
  "message": "Mensaje desde Swagger",
  "from": "Swagger UI"
}
```
5. Click "Execute"

### Usando curl

```bash
curl -X POST "https://localhost:7154/api/Message/send" \
     -H "Content-Type: application/json" \
     -d '{"message":"Hola desde curl","from":"Terminal"}' \
     -k
```

## Orden de Ejecución Correcto

1. **HTTPSMessenger** (primero)
2. **TLSMessenger** (segundo)  
3. **Client** (tercero)

Mantener las tres aplicaciones ejecutándose simultáneamente para pruebas completas.

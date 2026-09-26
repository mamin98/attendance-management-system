# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files first — layer caching means `dotnet restore` only
# re-runs when a .csproj actually changes, not on every code edit.
COPY ["AttendanceSystem.API/AttendanceSystem.API.csproj", "AttendanceSystem.API/"]
COPY ["AttendanceSystem.Application/AttendanceSystem.Application.csproj", "AttendanceSystem.Application/"]
COPY ["AttendanceSystem.Domain/AttendanceSystem.Domain.csproj", "AttendanceSystem.Domain/"]
COPY ["AttendanceSystem.Infrastructure/AttendanceSystem.Infrastructure.csproj", "AttendanceSystem.Infrastructure/"]

RUN dotnet restore "AttendanceSystem.API/AttendanceSystem.API.csproj"

COPY . .
WORKDIR /src/AttendanceSystem.API
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# curl is needed for the container HEALTHCHECK below
RUN apt-get update && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

# Non-root user — container security hardening
RUN groupadd -r appgroup && useradd -r -g appgroup appuser

COPY --from=build /app/publish .

RUN mkdir -p /app/Uploads /app/Logs \
    && chown -R appuser:appgroup /app

USER appuser

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=5s --start-period=20s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "AttendanceSystem.API.dll"]
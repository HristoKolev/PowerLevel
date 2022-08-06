FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as dotnet-build

# Restore
WORKDIR /work
COPY dotnet-project-files.tar .
RUN tar -xvf dotnet-project-files.tar
WORKDIR /work/server/
RUN dotnet restore

# Build
WORKDIR /work
COPY . .
WORKDIR /work/server/PowerLevel.Server/
RUN dotnet publish -c Release -o /build -r linux-musl-x64 --no-restore --self-contained

FROM alpine

# dotnet dependencies
RUN apk --no-cache add --update libstdc++ libintl icu-libs && \
    rm -rf /tmp/* /var/tmp/* /var/cache/apk/* /var/cache/distfiles/*

COPY --from=dotnet-build /build /app

ENTRYPOINT ["/app/powerlevel-server"]

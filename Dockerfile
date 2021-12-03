FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY . .
WORKDIR "/src/template-cs-mvc"
RUN dotnet restore "template-cs-mvc.csproj"
RUN dotnet build "template-cs-mvc.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "template-cs-mvc.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "template-cs-mvc.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ARG connection_string
ARG jwt_security_key
ARG lol_base_url
ARG tft_base_url
ARG lor_base_url
ENV DB_CONNECTION_STRING=$connection_string
ENV JWT_SECURITY_KEY=$jwt_security_key
ENV LOL_BASE_URL=$lol_base_url
ENV TFT_BASE_URL=$tft_base_url
ENV LOR_BASE_URL=$lor_base_url

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Dashboard/Dashboard.csproj", "Dashboard/"]
RUN dotnet restore "Dashboard/Dashboard.csproj"
COPY . .
WORKDIR "/src/Dashboard"
RUN dotnet build "Dashboard.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dashboard.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dashboard.dll"]

FROM microsoft/dotnet:2.1-sdk

WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy and build everything else
COPY . ./

RUN dotnet publish -c Release -o out

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "out/api-dados-paciente.dll"]
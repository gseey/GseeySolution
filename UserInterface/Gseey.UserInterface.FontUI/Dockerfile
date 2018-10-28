FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 3967
EXPOSE 44383

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["UserInterface/Gseey.UserInterface.FontUI/Gseey.UserInterface.FontUI.csproj", "UserInterface/Gseey.UserInterface.FontUI/"]
COPY ["Middleware/Gseey.Middleware.Weixin/Gseey.Middleware.Weixin.csproj", "Middleware/Gseey.Middleware.Weixin/"]
COPY ["Framework/Gseey.Framework.Common/Gseey.Framework.Common.csproj", "Framework/Gseey.Framework.Common/"]
COPY ["Framework/Gseey.Framework.BaseDTO/Gseey.Framework.BaseDTO.csproj", "Framework/Gseey.Framework.BaseDTO/"]
COPY ["Framework/Gseey.Framework.DataBase/Gseey.Framework.DataBase.csproj", "Framework/Gseey.Framework.DataBase/"]
RUN dotnet restore "UserInterface/Gseey.UserInterface.FontUI/Gseey.UserInterface.FontUI.csproj"
COPY . .
WORKDIR "/src/UserInterface/Gseey.UserInterface.FontUI"
RUN dotnet build "Gseey.UserInterface.FontUI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Gseey.UserInterface.FontUI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Gseey.UserInterface.FontUI.dll"]
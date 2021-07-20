FROM mcr.microsoft.com/dotnet/sdk:3.1
EXPOSE 9201
VOLUME ["/usr/app/data"]

WORKDIR /usr/app
COPY . .
CMD ["dotnet", "run"]

